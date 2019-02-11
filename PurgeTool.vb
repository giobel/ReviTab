#Region "Imported Namespaces"
Imports System
Imports System.Collections.Generic
Imports Autodesk.Revit.DB
#End Region

Public Class PurgeTool
    ''' <summary>
    ''' The guid of the 'Project contains unused families and types' PerformanceAdviserRuleId.
    ''' </summary>
    Const PurgeGuid As String = "e8c63650-70b7-435a-9010-ec97660c1bda"
    ''' <summary>
    ''' Get all purgeable elements.
    ''' Intended for Revit 2017+ as versions up to and including Revit 2016 throw an InternalException.
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="purgeableElementIds"></param>
    ''' <returns>True if successful.</returns>
    Shared Function GetPurgeableElements(doc As Document, ByRef purgeableElementIds As ICollection(Of ElementId)) As Boolean
        purgeableElementIds = New List(Of ElementId)()

        Try
            'create a new list of rules.
            Dim ruleIds As IList(Of PerformanceAdviserRuleId) = New List(Of PerformanceAdviserRuleId)
            Dim ruleId As PerformanceAdviserRuleId = Nothing
            'find the intended rule.
            If GetPerformanceAdvisorRuleId(PurgeGuid, ruleId) Then
                'add the rule to the new list.
                ruleIds.Add(ruleId)
            Else
                'cannot find rule.
                Return False
            End If
            'execute our chosen rule only.
            Dim failureMessages As IList(Of FailureMessage) = PerformanceAdviser.GetPerformanceAdviser().ExecuteRules(doc, ruleIds)
            If failureMessages.Count > 0 Then
                'If there are any purgeable elements, we should have a failure message.
                'the failure message should have a collection of failing elements - set to our byref collection
                purgeableElementIds = failureMessages.Item(0).GetFailingElements
            End If
            'no errors - return true.
            Return True
        Catch ex As Autodesk.Revit.Exceptions.InternalException
            'this exception gets thrown a lot in earlier versions of Revit - up to and including Revit 2016.

        End Try
        'likely thrown an internal exception
        Return False
    End Function
    ''' <summary>
    ''' Find a PerformanceAdviserRuleId with a guid that matches a supplied guid.
    ''' </summary>
    ''' <param name="guidStr"></param>
    ''' <param name="ruleId"></param>
    ''' <returns>true if successful, along with the rule as a byref.</returns>
    Private Shared Function GetPerformanceAdvisorRuleId(ByVal guidStr As String, ByRef ruleId As PerformanceAdviserRuleId) As Boolean
        ruleId = Nothing
        Dim guid As Guid = New Guid(guidStr)
        For Each rule As PerformanceAdviserRuleId In PerformanceAdviser.GetPerformanceAdviser().GetAllRuleIds
            'check if the rule Id matches our rule guid
            If rule.Guid.Equals(guid) Then
                'it does - set rule to our byref object
                ruleId = rule
                Return True
            End If
        Next
        'failed to find the rule matching our guid.
        Return Nothing
    End Function
End Class
