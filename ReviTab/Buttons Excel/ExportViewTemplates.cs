#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using forms = System.Windows.Forms;
using System.Linq;
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

            IEnumerable<View> allViews = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(View)).Cast<View>()
                            .Where(v => ViewHasTemplate(v))
                            .Where(v => v.Category.Name != "Sheets");

            BuiltInParameter bip_t = BuiltInParameter.VIEW_TEMPLATE_FOR_SCHEDULE;

            foreach (View v in allViews)
            {                
                string vtParam = v.get_Parameter(bip_t).AsValueString();
               sb.AppendLine($"{v.Id}, {v.Name}, {v.ViewTemplateId}, {vtParam}");       
            }

            File.WriteAllText(outputFile, "Element Id, View Name, ViewTemplateId, View Template\n");

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

        private View FindViewTemplate(IEnumerable<View> viewTemp, ElementId vtId)
        {
            foreach (View vt in viewTemp)
            {
                if (vt.Id == vtId)
                {
                    return vt;
                }
            }

            return null;
        }
        private ElementId GetViewTempId(Document doc, string vtname)
        {

            IEnumerable<View> viewTemp = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(View)).Cast<View>()
                                            .Where(v => v.IsTemplate);

            foreach (View v in viewTemp)
            {
                if (v.Name == vtname)
                {
                    return v.Id;
                }
            }
            return null;
        }
        private bool ViewHasTemplate(View v)
        {
            return !v.IsTemplate && (v.CanUseTemporaryVisibilityModes() || ((ViewType.Schedule == v.ViewType) && !((ViewSchedule)v).IsTitleblockRevisionSchedule));
        }
    }

}


