#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            List<Tuple<View, ViewSheet>> legendsOnSheet = new List<Tuple<View, ViewSheet>>();

            //find clouds in legends
            foreach (ViewSheet vs in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>())
            {
                foreach (ElementId eid in vs.GetAllPlacedViews())
                {
                    View v = doc.GetElement(eid) as View;

                    if (v.ViewType == ViewType.Legend)
                    {
                        legendsOnSheet.Add(new Tuple<View, ViewSheet> (v, vs));
                        //vs.LookupParameter("Sheet Number").AsString();
                    }
                }
            }


            ICollection<ElementId> fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RevisionClouds).WhereElementIsNotElementType().ToElementIds();

            foreach (ElementId eid in fec)
            {

                RevisionCloud cloud = doc.GetElement(eid) as RevisionCloud;
                
                View view = doc.GetElement(cloud.OwnerViewId) as View;
                string cloudDescr = cloud.LookupParameter("Revision Description").AsString();
                
                List<ViewSheet> vs = new List<ViewSheet>();

                if (view.ViewType == ViewType.DrawingSheet)
                {
                    vs.Add(view as ViewSheet);                    
                }
                //Legends can be placed on multiple sheets
                else if (view.ViewType == ViewType.Legend)
                {
                    foreach (Tuple<View, ViewSheet> legend in legendsOnSheet)
                    {
                        if (legend.Item1.Name == view.Name)
                        {
                            vs.Add(legend.Item2);
                        }
                    }
                }
                else
                {
                    vs.Add(Helpers.FindViewSheetByName(doc, view.Name));
                }

                if (vs.Count > 1)
                {
                    foreach (ViewSheet viewSheet in vs)
                    {
                        sb.AppendLine($"{cloud.Id}, {view.Name}, {viewSheet.SheetNumber}, {cloudDescr}");
                    }
                }
                else if (vs[0] != null)
                {
                    sb.AppendLine($"{cloud.Id}, {view.Name}, {vs[0].SheetNumber}, {cloudDescr}");
                }

                
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


