using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class OpenLastCreatedView : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            //Selection selection = uidoc.Selection;

            FilteredElementCollector instances = new FilteredElementCollector(doc).OfClass(typeof(View));
            //.OfClass( typeof( FamilyInstance ) );

            Options opt = new Options();

            int id_max = instances
                .Where(e => null != e.Category)
                //.Where( e => (null != e.LevelId && ElementId.InvalidElementId != e.LevelId) )
                //.Where( e => null != e.get_Geometry( opt ) )
                .Max<Element, int>(e => e.Id.IntegerValue);

            ElementId last_eid = new ElementId(id_max);

            //if (last_eid != null)
            //{
            //    selection.SetElementIds(
            //      new List<ElementId>(
            //        new ElementId[] { last_eid }));
            //}

            uidoc.ActiveView = doc.GetElement(last_eid) as View;

            return Result.Succeeded;
            

        }
    }
}
