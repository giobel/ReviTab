using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class SelectAllTagsInViewport : IExternalCommand
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

            IList<Reference> refe = uidoc.Selection.PickObjects(ObjectType.Element, "Select Viewports");

            IList<ElementId> viewportViewIds = new List<ElementId>();

            foreach (Reference r in refe)
            {
                Viewport vp = doc.GetElement(r) as Viewport;
                viewportViewIds.Add(vp.ViewId);
            }

            ICollection<ElementId> toBeSelected = new List<ElementId>();

            foreach (ElementId viewId in viewportViewIds)
            {
                ICollection<ElementId> fec = new FilteredElementCollector(doc, viewId).OfCategory(BuiltInCategory.OST_StructuralColumnTags).WhereElementIsNotElementType().ToElementIds();

                foreach (ElementId item in fec)
                {
                    toBeSelected.Add(item);
                }
            }

            uidoc.Selection.SetElementIds(toBeSelected);

            return Result.Succeeded;
        }

    }

}



