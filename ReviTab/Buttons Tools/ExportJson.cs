using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ReviTab.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ExportJson : IExternalCommand
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

            IList<Element> curtainWallPanels = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_CurtainWallPanels).WhereElementIsNotElementType().ToElements();

            var w = new List<PanelData>();

            foreach (Element panel in curtainWallPanels)
            {
                w.Add(new PanelData
                {
                    ElementId = panel.Id.ToString(),
                    Type = "Type ???",
                    Width = panel.LookupParameter("Width").AsValueString(),
                    Height = panel.LookupParameter("Height").AsValueString(),
                    Angle = panel.LookupParameter("Anglecalc").AsValueString(),
                    Area = panel.LookupParameter("Area").AsValueString(),
                    Material = ""
                });
            }

            string m_finalPath = @"C:\Temp\modelData.json";

            using (StreamWriter sw = new StreamWriter(m_finalPath, false))
            {
                sw.WriteLine("{\"Items\":");
                sw.WriteLine(JsonConvert.SerializeObject(w));
                sw.WriteLine("}");
            }

            TaskDialog myDialog = new TaskDialog("Summary");
            myDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
            myDialog.MainContent = "Operation completed";

            myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, $"Json file saved here {m_finalPath}");

            TaskDialogResult res = myDialog.Show();

            if (TaskDialogResult.CommandLink4 == res)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = @"C:\Temp\",
                    UseShellExecute = true,
                    Verb = "open"
                });
            }

            return Result.Succeeded;

        }//close Execute


    }


    public class PanelData
    {
        public string ElementId { get; set; }
        public string Type { get; set; }
        public string Width{ get; set; }
        public string Height { get; set; }
        public string Angle { get; set; }
        public string Area { get; set; }        
        public string Material { get; set; }
    }

}
