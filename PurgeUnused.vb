#Region "Imported Namespaces"
Imports System.Collections.Generic

Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.Attributes
#End Region

<Transaction(TransactionMode.Manual)>
<Regeneration(RegenerationOption.Manual)>
<Journaling(JournalingMode.UsingCommandData)>
Public Class PurgeUnused
    Implements IExternalCommand
    ''' <summary>
    ''' Use the perfomance adviser to purge unused families and types.
    ''' </summary>
    ''' <param name="commandData"></param>
    ''' <param name="message"></param>
    ''' <param name="elements"></param>
    ''' <returns></returns>
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim doc As Document = commandData.Application.ActiveUIDocument.Document
        Dim purgeableElements As ICollection(Of ElementId) = Nothing

        If PurgeTool.GetPurgeableElements(doc, purgeableElements) AndAlso purgeableElements.Count > 0 Then
            Using transaction As New Transaction(doc, "Purge Unused")
                transaction.Start()
                doc.Delete(purgeableElements)
                transaction.Commit()
                Return Result.Succeeded
            End Using
        Else
            Return Result.Failed
        End If
    End Function
End Class