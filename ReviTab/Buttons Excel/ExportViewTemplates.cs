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
    public class ExportViewTemplates : IExternalCommand
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


            string outputFile = @"C:\Temp\viewTemplates.csv";

            StringBuilder sb = new StringBuilder();

            ICollection<ElementId> fec = new FilteredElementCollector(doc).OfClass(typeof(View)).ToElementIds();

            foreach (ElementId eid in fec)
            {
                View v = doc.GetElement(eid) as View;
                if (v.IsTemplate)
                    sb.AppendLine($"{eid}, {v.Name}");
            }


            File.WriteAllText(outputFile, "Element Id, View Name\n");

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


