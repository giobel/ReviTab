#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using rg = Rhino.Geometry;

#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class Intersections : IExternalCommand
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

            //Reference lineToProject = uidoc.Selection.PickObject(ObjectType.Element, "Cut Line");
            //Element lineElement = doc.GetElement(lineToProject);

            //CurveElement curveLine = lineElement as CurveElement;
            //LocationCurve lineLocation = lineElement.Location as LocationCurve;

            ////cut line endpoints
            //XYZ v1 = lineLocation.Curve.GetEndPoint(0);
            //XYZ v2 = lineLocation.Curve.GetEndPoint(0);
            ////rhino cut line endpoints
            //rg.Point3d rv0 = new rg.Point3d(0, 0, 0);
            //rg.Point3d rv1 = new rg.Point3d(15, 15, 0);
            //rg.Point3d rv2 = new rg.Point3d(0, 0, 150);
            ////rhino cut plane
            //rg.Plane cutPlane = new rg.Plane(rv0, rv1, rv2);

            ////wip points
            //rg.Point3d rv3 = new rg.Point3d(5, 0, 5);
            //rg.Point3d rv4 = new rg.Point3d(15, 0, 5);
            //rg.Point3d rv5 = new rg.Point3d(0, 15, 5);

            ////wip plane
            //rg.Plane rhinoPlane = new rg.Plane(rv3, rv4, rv5);

            //rg.Line intLine;

            //var result = rg.Intersect.Intersection.PlanePlane(cutPlane, rhinoPlane, out intLine);

            //TaskDialog.Show("r", intLine.ToString());



            ElementId materialId = new ElementId(39497);

            Reference face1ref = uidoc.Selection.PickObject(ObjectType.Element, "Select Adaptive element");

            UV midSrf = new UV(0.5, 0.5);

            PlanarFace pf = GetFace(doc.GetElement(face1ref));


            Reference wallRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Wall");

            Element wall = doc.GetElement(wallRef);

            LocationCurve lc = wall.Location as LocationCurve;

            Line wallLine = lc.Curve as Line;

            XYZ perpDir = wallLine.Direction.CrossProduct(XYZ.BasisZ);

            Options opt = new Options();

            GeometryElement geomElem = doc.GetElement(wallRef).get_Geometry(opt);

            using (Transaction t = new Transaction(doc, "Find intersection"))
            {

                t.Start();

                foreach (GeometryObject geomObj in geomElem)
                {
                    Solid geomSolid = geomObj as Solid;
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        XYZ faceNormal = geomFace.ComputeNormal(new UV(0.5, 0.5));
                        //select only the vertical faces
                        if (faceNormal.CrossProduct(perpDir).IsAlmostEqualTo(XYZ.Zero))
                        {
                            //doc.Paint(wallRef.ElementId, geomFace, materialId);	
                            //PrintPoint(new List<XYZ>{perpDir, faceNormal});
                            Curve intCrv = null;
                            pf.Intersect(geomFace, out intCrv);

                            Plane p = Plane.CreateByNormalAndOrigin(pf.FaceNormal, pf.Origin);
                            SketchPlane sp = SketchPlane.Create(doc, p);

                            doc.Create.NewModelCurve(intCrv, sp);
                        }

                    }
                }


                t.Commit();

            }



            return Result.Succeeded;
        }

        public PlanarFace GetFace(Element element)
        {
            Options opt = new Options();
            // Get geometry element of the selected element
            GeometryElement geoElement = element.get_Geometry(opt);

            PlanarFace pf = null;

            // Get geometry object
            foreach (GeometryObject geoObject in geoElement)
            {
                // Get the geometry instance which contains the geometry information
                Autodesk.Revit.DB.GeometryInstance instance = geoObject as Autodesk.Revit.DB.GeometryInstance;
                if (null != instance)
                {
                    GeometryElement instanceGeometryElement = instance.GetInstanceGeometry();
                    foreach (GeometryObject instObj in instanceGeometryElement)
                    {
                        Solid solid = instObj as Solid;
                        if (null == solid || 0 == solid.Faces.Size || 0 == solid.Edges.Size)
                        {
                            continue;
                        }
                        // Get the faces and edges from solid, and transform the formed points
                        foreach (Face face in solid.Faces)
                        {
                            //face.ComputeNormal(new UV(0.5, 0.5));
                            pf = face as PlanarFace;
                        }
                    }
                }
            }

            return pf;
        }
    }
}
