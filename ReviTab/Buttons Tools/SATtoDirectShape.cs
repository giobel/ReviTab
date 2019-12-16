#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class SATtoDirectShape : IExternalCommand
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

            List<Element> col = new List<Element>();

            Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Select Element");

            col.Add(doc.GetElement(r));

            Options geometryOptions = new Options();

            ElementId cat1Id = new ElementId(BuiltInCategory.OST_Walls);


            DirectShapeLibrary dsLib = DirectShapeLibrary.GetDirectShapeLibrary(doc);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Convert elements to DirectShapes");

                foreach (Element e in col)
                {
                    GeometryElement gelt = e.get_Geometry(
                      geometryOptions);

                    if (null != gelt)
                    {
                        string appDataGUID = e.Id.ToString();

                        try
                        {

                            string familyName = "MyFamily";
                            DirectShapeType dsType1 = DirectShapeType.Create(doc, familyName, cat1Id);
                            dsType1.SetShape(new List<GeometryObject>(gelt));
                            dsLib.AddDefinitionType(familyName, dsType1.Id);

                            Transform trs = Transform.Identity;

                            DirectShape ds1 = DirectShape.CreateElementInstance(doc, dsType1.Id, cat1Id, familyName, trs);

                            doc.Delete(e.Id);

                            TaskDialog.Show("Result", "Element Flattened");
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                }
                tx.Commit();

            }

            return Result.Succeeded;
        }
    }
}
