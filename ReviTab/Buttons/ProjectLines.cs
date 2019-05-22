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
    public class ProjectLines : IExternalCommand
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


            //ICollection<Reference> selectedLines = uidoc.Selection.PickObjects(ObjectType.Element, "Select Lines");

            Reference refFace = uidoc.Selection.PickObject(ObjectType.Face, "Select Surface");
            Element selectedElement = doc.GetElement(refFace);


            GeometryObject obj = selectedElement.GetGeometryObjectFromReference(refFace);

            Face face = obj as Face;


            ICollection<Reference> refLines = uidoc.Selection.PickObjects(ObjectType.Element, "Select Line");
            
            Level lvl = null;

            ICollection<Element> allLevels = new FilteredElementCollector(doc).OfClass(typeof(Level)).ToElements();

            foreach (Level l in allLevels)
            {
                if (l.Name == "Level 1")
                    lvl = l;
            }

            using (Transaction t = new Transaction(doc, "test"))
            {

                t.Start();

                foreach (Reference refLine in refLines)
                {

                    try
                    {
                        LocationCurve locCurve = doc.GetElement(refLine.ElementId).Location as LocationCurve;
                        Line line = locCurve.Curve as Line;

                        XYZ q = line.GetEndPoint(1);
                        XYZ p = line.GetEndPoint(0);

                        XYZ v = q - p;

                        XYZ rayDirection = new XYZ(0, 0, 1);

                        XYZ normal = line.Direction.CrossProduct(rayDirection);

                        Plane verticalPlane = Plane.CreateByNormalAndOrigin(normal, p);

                        SketchPlane splane = SketchPlane.Create(doc, verticalPlane);
                        
                        XYZ qProjected = ProjectPoint(doc, refFace, q, rayDirection);

                        XYZ pProjected = ProjectPoint(doc, refFace, p, rayDirection);

                        Line projectedLine = Line.CreateBound(pProjected, qProjected);

                        ModelLine verticalmLine = doc.Create.NewModelCurve(projectedLine, splane) as ModelLine;

                        
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("result", ex.Message);
                    }



                }




                t.Commit();
            }

            return Result.Succeeded;
        }

        private XYZ ProjectPoint(Document doc, Reference refFace, XYZ point, XYZ rayDirection)
        {

            View3D active3D = doc.ActiveView as View3D;

            ReferenceIntersector refIntersector = new ReferenceIntersector(refFace.ElementId, FindReferenceTarget.Face, active3D);

            ReferenceWithContext referenceWithContext = refIntersector.FindNearest(point, rayDirection);

            Reference reference = referenceWithContext.GetReference();

            XYZ intersection = reference.GlobalPoint;

            return intersection;
        }
    }
}
