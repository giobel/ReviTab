#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class RevisionCloudsSummary : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            View activeView = doc.ActiveView;


            string outputFile = @"C:\Temp\revCloudsSummary.csv";

            StringBuilder sb = new StringBuilder();

            ICollection<ElementId> fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RevisionClouds).WhereElementIsNotElementType().ToElementIds();

            foreach (ElementId eid in fec)
            {

                RevisionCloud cloud = doc.GetElement(eid) as RevisionCloud;
                
                View view = doc.GetElement(cloud.OwnerViewId) as View;
                string cloudDescr = cloud.LookupParameter("Revision Description").AsString();
                string sheetNumber = "";
                ViewSheet vs = null;

                if (view.ViewType == ViewType.DrawingSheet)
                {
                    vs = view as ViewSheet;                    
                }
                else
                {
                    vs = Helpers.FindViewSheetByName(doc, view.Name);
                }

                if (vs != null)
                    sheetNumber = vs.SheetNumber;

                sb.AppendLine($"{cloud.Id}, {view.Name}, {sheetNumber}, {cloudDescr}");
            }


            File.WriteAllText(outputFile, "CloudId, View Name, Sheet Number, Revision Cloud Description\n");

            File.AppendAllText(outputFile, sb.ToString());

            TaskDialog myDialog = new TaskDialog("Summary");
            myDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
            myDialog.MainContent = "Operation completed";

            myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, $"Open Log File {outputFile}");

            TaskDialogResult res = myDialog.Show();

            if (TaskDialogResult.CommandLink4 == res)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = outputFile;
                process.Start();
            }


            return Result.Succeeded;
        }

    }

}


