#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class AlignViews : IExternalCommand
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

            ICollection<ElementId> refe = uidoc.Selection.GetElementIds();

            uidoc.ActiveView = uidoc.ActiveGraphicalView;
            
            int count = 0;

            using (Transaction t = new Transaction(doc, "Align viewports"))
            {
                t.Start();

                XYZ vpCenter = uidoc.Selection.PickPoint("Select center point");

                foreach (var item in refe)
                {
                    View targetView = doc.GetElement(item) as View;
                    Viewport targetViewport = FindViewportFromView(doc, targetView);

                    targetViewport.SetBoxCenter(vpCenter);

                    count += 1;
                }


                t.Commit();
            }

            TaskDialog.Show("Result", $"{count}/{refe.Count} viewport updated");
            
            return Result.Succeeded;
        }

        private Viewport FindViewportFromView(Document doc, View view)
        {
            //find corresponding viewport
            IEnumerable<Viewport> fec = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewports).WhereElementIsNotElementType().Cast<Viewport>();

            return fec.Where(x => x.ViewId == view.Id).First();
        }


    }
}
