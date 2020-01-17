#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class VersionInfo : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			string date = DateTime.Today.ToShortDateString();

			IEnumerable<Element> listOfElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TextNotes)
				.WhereElementIsNotElementType()
				.ToElements().Where(e => e.Name == "FolderLink");

			string currentPdfPath = null;
			string completedMarkups = null;
			string tobedoneMarkups = null;
			string incomingFolder = null;


			foreach (var element in listOfElements)
			{

				TextElement te = element as TextElement;

				string destination = te.Text.Split(':')[0].Trim();
				string path = te.Text.Split(':')[1].Trim();

				if (destination.Contains("PDF"))
				{
					currentPdfPath = path;
				}
				else if (destination.Contains("completed"))
				{
					completedMarkups = path;
				}
				else if (destination.Contains("Incoming"))
				{
					incomingFolder = path;
				}
				else if (destination.Contains("new"))
				{
					tobedoneMarkups = path;
				}

			}

			TaskDialog myDialog = new TaskDialog("Summary");
			myDialog.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
			myDialog.MainContent = "Project folders:";

			myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, String.Format("Open Markups to be done"));

			myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, String.Format("Open Markups done"));

			myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, String.Format("Current Pdf folder"));

			myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, String.Format("Incoming folder"));

			myDialog.FooterText = "Version 1.0.6 \nCompiled on " + date;

			TaskDialogResult res = myDialog.Show();

			if (TaskDialogResult.CommandLink1 == res)
			{
				System.Diagnostics.Process.Start(tobedoneMarkups);
			}
			else if (TaskDialogResult.CommandLink2 == res)
			{
				System.Diagnostics.Process.Start(completedMarkups);
			}
			else if (TaskDialogResult.CommandLink3 == res)
			{
				System.Diagnostics.Process.Start(currentPdfPath);
			}
			else if (TaskDialogResult.CommandLink4 == res)
			{
				System.Diagnostics.Process.Start(incomingFolder);
			}

			return Result.Succeeded;
        }
    }
}

