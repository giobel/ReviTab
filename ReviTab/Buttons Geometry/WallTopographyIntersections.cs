#region Namespaces
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class WallTopographyIntersections : IExternalCommand
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

            //WALLS FACES
            List<PlanarFace> wallFaces = new List<PlanarFace>();

            //WALLS
            Reference wallRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Wall");

            Element wall = doc.GetElement(wallRef);

            LocationCurve lc = wall.Location as LocationCurve;

            Line wallLine = lc.Curve as Line;

            XYZ perpDir = wallLine.Direction.CrossProduct(XYZ.BasisZ);

            Options opt = new Options();
            opt.ComputeReferences = true;
            opt.IncludeNonVisibleObjects = false;
            opt.View = doc.ActiveView;

            GeometryElement geomElem = doc.GetElement(wallRef).get_Geometry(opt);

            foreach (GeometryObject geomObj in geomElem)
            {
                Solid geomSolid = geomObj as Solid;
                foreach (Face geomFace in geomSolid.Faces)
                {

                    XYZ faceNormal = geomFace.ComputeNormal(new UV(0.5, 0.5));

                    //select only the vertical faces
                    if (faceNormal.CrossProduct(perpDir).IsAlmostEqualTo(XYZ.Zero))
                    {

                        PlanarFace pf = geomFace as PlanarFace;
                        wallFaces.Add(pf);

                    }

                }
            }


            //GET MESH
            Reference meshRef = uidoc.Selection.PickObject(ObjectType.Element, "Select Mesh");
            Element e = doc.GetElement(meshRef);

            GeometryElement geomElemSurvey = e.get_Geometry(opt);

            Mesh geomMesh = null;

            foreach (GeometryObject geomObj in geomElemSurvey)
            {
                try
                {
                    GeometryInstance gi = geomObj as GeometryInstance;
                    foreach (GeometryObject element in gi.GetInstanceGeometry())
                    {
                        geomMesh = element as Mesh;
                    }

                }
                catch
                {

                    geomMesh = geomObj as Mesh;
                }

            }


            using (Transaction t = new Transaction(doc, "Find intersection"))
            {

                t.Start();

                List<XYZ> intPts = new List<XYZ>();

                for (int i = 0; i < geomMesh.NumTriangles; i++)
                {
                    MeshTriangle triangle = geomMesh.get_Triangle(i);
                    XYZ vertex1 = triangle.get_Vertex(0);
                    XYZ vertex2 = triangle.get_Vertex(1);
                    XYZ vertex3 = triangle.get_Vertex(2);

                    Line[] edgeList = new Line[3];

                    try
                    {
                        Line[] edges = new Line[] { Line.CreateBound(vertex1, vertex2), Line.CreateBound(vertex1, vertex3), Line.CreateBound(vertex2, vertex3) };

                        for (int k = 0; k < edgeList.Length; k++)
                        {
                            edgeList[k] = edges[k];
                        }

                    }
                    catch { }


                    //		        Plane facePlane = Plane.CreateByThreePoints(vertex1, vertex2, vertex3);
                    //		        SketchPlane sp = SketchPlane.Create(doc, facePlane);
                    //		        doc.Create.NewModelCurve(edge12, sp);
                    //		        doc.Create.NewModelCurve(edge13, sp);
                    //		        doc.Create.NewModelCurve(edge23, sp);



                    foreach (PlanarFace pf in wallFaces)
                    {

                        XYZ[] pts = new XYZ[2];

                        int count = 0;

                        if (edgeList[0] != null)
                        {
                            foreach (Line edge in edgeList)
                            {
                                IntersectionResultArray intersections = null;
                                SetComparisonResult scr = pf.Intersect(edge, out intersections);



                                if (scr == SetComparisonResult.Overlap && intersections.Size == 1)
                                {

                                    for (int j = 0; j < intersections.Size; j++)
                                    {

                                        //TaskDialog.Show("r", intersections.get_Item(i));
                                    }

                                    //TaskDialog.Show("r", intersections.Size.ToString());

                                    IntersectionResult iResult = intersections.get_Item(0);
                                    intPts.Add(iResult.XYZPoint);
                                    pts[count] = iResult.XYZPoint;
                                    count = 1;

                                    if (pts.Length == 2 && pts[1] != null)
                                    {
                                        //		        		TaskDialog.Show("r", String.Format("{0}\n{1}",pts[0], pts[1]));
                                        try
                                        {
                                            Plane wallPlane = Plane.CreateByNormalAndOrigin(pf.FaceNormal, pf.Origin);
                                            SketchPlane spWall = SketchPlane.Create(doc, wallPlane);    //PROBABLY BETTER TO USE ONLY 1 SKETCHPLANE (THE SAME FOR ALL THE CURVES -> MOVE IT OUT OF THE LOOP)
                                            doc.Create.NewModelCurve(Line.CreateBound(pts[0], pts[1]), spWall);
                                        }
                                        catch { }
                                    }

                                }

                            }

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


        #region WIP
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



    //    ElementId materialId = new ElementId(39497);

    //    Reference face1ref = uidoc.Selection.PickObject(ObjectType.Element, "Select Adaptive element");

    //    UV midSrf = new UV(0.5, 0.5);

    //    PlanarFace pf = GetFace(doc.GetElement(face1ref));


    //    Reference wallRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Wall");

    //    Element wall = doc.GetElement(wallRef);

    //    LocationCurve lc = wall.Location as LocationCurve;

    //    Line wallLine = lc.Curve as Line;

    //    XYZ perpDir = wallLine.Direction.CrossProduct(XYZ.BasisZ);

    //    Options opt = new Options();

    //    GeometryElement geomElem = doc.GetElement(wallRef).get_Geometry(opt);

    //        using (Transaction t = new Transaction(doc, "Find intersection"))
    //        {

    //            t.Start();

    //            foreach (GeometryObject geomObj in geomElem)
    //            {
    //                Solid geomSolid = geomObj as Solid;
    //                foreach (Face geomFace in geomSolid.Faces)
    //                {
    //                    XYZ faceNormal = geomFace.ComputeNormal(new UV(0.5, 0.5));
    //                    //select only the vertical faces
    //                    if (faceNormal.CrossProduct(perpDir).IsAlmostEqualTo(XYZ.Zero))
    //                    {
    //                        //doc.Paint(wallRef.ElementId, geomFace, materialId);	
    //                        //PrintPoint(new List<XYZ>{perpDir, faceNormal});
    //                        Curve intCrv = null;
    //pf.Intersect(geomFace, out intCrv);

    //                        Plane p = Plane.CreateByNormalAndOrigin(pf.FaceNormal, pf.Origin);
    //SketchPlane sp = SketchPlane.Create(doc, p);

    //doc.Create.NewModelCurve(intCrv, sp);
    //                    }

    //                }
    //            }


    //            t.Commit();

    //        }

        #endregion
    }
}
