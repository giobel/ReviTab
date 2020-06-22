using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ReviTab.Buttons_Geometry
{
    [Transaction(TransactionMode.Manual)]
    public class CreateExtrusion : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

                //EDGE PATH
                Reference edgeBack = uidoc.Selection.PickObject(ObjectType.Edge, "Select long edge");

                //EDGE DIRECTION
                Reference edgeDirRef = uidoc.Selection.PickObject(ObjectType.Edge, "Select edge for direction");

                //EDGE 
                Element edgeBackElement = doc.GetElement(edgeBack);
                GeometryObject edgeBackObject = edgeBackElement.GetGeometryObjectFromReference(edgeBack);
                Edge elemEdge = edgeBackObject as Edge;
                Curve edgeBackCurve = elemEdge.AsCurve();

                XYZ edgeBackDir = edgeBackCurve.GetEndPoint(1) - edgeBackCurve.GetEndPoint(0);


            //POINT ORDER: 1-5 back edge - extrusion path, 0-1 walkway width
            
            /*
            3
            |       2 
            |       |
            |       |
            |       |            5
            0       |
                    1
            */



            XYZ pt5 = edgeBackCurve.GetEndPoint(0);

            //pt5 should be the lowest Z point
           if (pt5.Z > edgeBackCurve.GetEndPoint(1).Z)
            {
                pt5 = edgeBackCurve.GetEndPoint(1);
            }

                //EDGE DIRECTION
                Element edgeDirElement = doc.GetElement(edgeDirRef);
                GeometryObject edgeDirObject = edgeDirElement.GetGeometryObjectFromReference(edgeDirRef);
                Edge edgeDirEdge = edgeDirObject as Edge;
                Curve edgeDirCurve = edgeDirEdge.AsCurve();

                XYZ edgeDirDir = edgeDirCurve.GetEndPoint(1) - edgeDirCurve.GetEndPoint(0);

                double scale = 304.8;
                double offset = 1030 / scale; //distance from back edge to baseplate
                double width = 850 / scale; //egress width

                //PROFILE
                XYZ pt0 = edgeDirCurve.GetEndPoint(0); //towards tunnel
                XYZ pt1 = edgeBackCurve.GetEndPoint(0); //towards earth
                //XYZ pt2 = new XYZ(pt1.X, pt1.Y, pt1.Z+(2100/scale));


                if (edgeDirCurve.GetEndPoint(0).DistanceTo(edgeBackCurve.GetEndPoint(0))<
                    edgeDirCurve.GetEndPoint(1).DistanceTo(edgeBackCurve.GetEndPoint(0)))
                {
                    pt1 = edgeDirCurve.GetEndPoint(0);
                    pt0 = edgeDirCurve.GetEndPoint(1);
                }
                else
                {
                    pt1 = edgeDirCurve.GetEndPoint(1);
                    pt0 = edgeDirCurve.GetEndPoint(0);
                }

            //XYZ perpVector = edgeDirDir.CrossProduct(edgeBackDir).Normalize();
            XYZ perpVector = (pt5 - pt1).CrossProduct(pt0 - pt1).Normalize();

            pt1 = pt1 + (pt0 - pt1).Normalize() * (offset - width);
            pt0 = pt1 + (pt0 - pt1).Normalize() * width;

                XYZ pt2 = pt1 + perpVector * 2100 / scale;
                //XYZ pt3 = new XYZ(pt0.X, pt0.Y, pt0.Z+(2100/scale));
                XYZ pt3 = pt0 + perpVector * 2100 / scale;

                //TOP FACE PLANE
                Plane topPlane = Plane.CreateByThreePoints(pt0, pt1, pt5);

                CurveLoop path = CurveLoop.Create(new List<Curve> { edgeBackCurve });

                //START CURVELOOP

                Line crv0 = Line.CreateBound(pt1, pt0);
                Line crv1 = Line.CreateBound(pt0, pt3);
                Line crv2 = Line.CreateBound(pt3, pt2);
                Line crv3 = Line.CreateBound(pt2, pt1);

                CurveLoop profileLoop = CurveLoop.Create(new List<Curve> {crv0,crv1,crv2,crv3});
            
                //VERTICAL PLANE
                Plane p = Plane.CreateByThreePoints(pt0, pt1, pt2);


            //END CURVE LOOP
            Transform tf = Transform.CreateTranslation(edgeBackDir);

            Curve crv0tr = crv0.CreateTransformed(tf);
            Curve crv1tr = crv1.CreateTransformed(tf);
            Curve crv2tr = crv2.CreateTransformed(tf);
            Curve crv3tr = crv3.CreateTransformed(tf);

            CurveLoop profileLoopEnd = CurveLoop.Create(new List<Curve> { crv0tr, crv1tr, crv2tr, crv3tr });

            Plane pEnd = Plane.CreateByThreePoints(crv0tr.GetEndPoint(0), crv0tr.GetEndPoint(1), crv1tr.GetEndPoint(1));

            //			WireframeBuilder builder = new WireframeBuilder();
            //
            //			builder.AddCurve(edgeBackCurve);

            ElementId categoryId = new ElementId(BuiltInCategory.OST_GenericModel);

            SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);
            

            using (Transaction t = new Transaction(doc, "Create model curves"))
                {
                    t.Start();

                    try
                    {
                    //Solid sweep = GeometryCreationUtilities.CreateSweptGeometry(path, 0, edgeBackCurve.ComputeRawParameter(0.5), new List<CurveLoop> { profileLoop });
                    Solid loop = GeometryCreationUtilities.CreateLoftGeometry(new List<CurveLoop> { profileLoop, profileLoopEnd }, options);
                    
                    DirectShape ds = DirectShape.CreateElement(doc, categoryId);

                        //ds.SetShape(builder);

                        ds.SetShape(new GeometryObject[] { loop });
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", ex.Message);
                    }

                    SketchPlane sp = SketchPlane.Create(doc, p);
                    SketchPlane spTop = SketchPlane.Create(doc, topPlane);
                    SketchPlane spEnd = SketchPlane.Create(doc, pEnd);
                

                doc.Create.NewModelCurve(edgeBackCurve, spTop);
                    doc.Create.NewModelCurve(crv0, sp);
                    doc.Create.NewModelCurve(crv1, sp);
                    doc.Create.NewModelCurve(crv2, sp);
                    doc.Create.NewModelCurve(crv3, sp);

                doc.Create.NewModelCurve(crv0tr, spEnd);
                doc.Create.NewModelCurve(crv1tr, spEnd);
                doc.Create.NewModelCurve(crv2tr, spEnd);
                doc.Create.NewModelCurve(crv3tr, spEnd);

                t.Commit();
                }


                TaskDialog.Show("R", PrintPoint(edgeBackCurve.GetEndPoint(0), scale) + ";\n" +
                PrintPoint(edgeBackCurve.GetEndPoint(1), scale) + ";\n" +
                PrintPoint(edgeDirCurve.GetEndPoint(0), scale));

                return Result.Succeeded;

            
            
        }
        private string PrintPoint(XYZ point, double scale)
        {

            return String.Format("{0:00}, {1:00}, {2:00}", point.X * scale, point.Y * scale, point.Z * scale);

        }
    }
}
