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
    public class FlipSections : IExternalCommand
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
           
            IList<Reference> viewRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Select Section");

            List<Autodesk.Revit.DB.View> views = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View)).Cast<Autodesk.Revit.DB.View>().ToList();

            int flipped = 0;
            int errors = 0;

            using (Transaction t = new Transaction(doc, "MirrorSection"))
            {
                t.Start();

                foreach (Reference viewRef in viewRefList)
                {
                    try
                    {
                        Element section = doc.GetElement(viewRef);

                        View vs = Helpers.SectionElementToView(views, section);

                        Plane _plane = Plane.CreateByNormalAndOrigin(vs.ViewDirection, vs.Origin);

                        ElementTransformUtils.MirrorElements(doc, new List<ElementId> { section.Id }, _plane, false);

                        flipped++;
                    }
                    catch
                    {
                        errors++;
                    }

                }

                t.Commit();
            }

            TaskDialog.Show("Result", $"{flipped} sections flipped\n{errors} sections not flipped");

            return Result.Succeeded;

        }//close Execute

    }

}
