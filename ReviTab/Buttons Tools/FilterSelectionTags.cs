#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class FilterSelectionTags : IExternalCommand
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


            ISelectionFilter tagFilter = new TagsSelectionFilter();

            IList<Reference> refs = uidoc.Selection.PickObjects(ObjectType.Element, tagFilter, "Select some Text Notes");

            uidoc.Selection.SetElementIds(refs.Select(x => doc.GetElement(x).Id).ToList());

            return Result.Succeeded;
        }
    }
}
