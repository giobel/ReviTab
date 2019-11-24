#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ExtendGrids : IExternalCommand
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

            Reference re = uidoc.Selection.PickObject(ObjectType.Element, "Select Grid");

            View source = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).ToElements()
                .Where(x => x.Name == "Level 1").First() as View;

            ISet<ElementId> par = new List<ElementId>() as ISet<ElementId>;

            ElementId destination = doc.ActiveView.Id;

            TaskDialog.Show("r", destination.ToString());

            par.Add(destination);

            TaskDialog.Show("r", par.Count.ToString());

            using (Transaction t = new Transaction(doc, "set grid"))
            {
                t.Start();

                Grid g = doc.GetElement(re) as Grid;
                TaskDialog.Show("r", g.Name);
                g.PropagateToViews(source, par);

                t.Commit();
            }


            TaskDialog.Show("r", source.Name);

            return Result.Succeeded;
        }
    }
}
