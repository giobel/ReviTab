#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System;
using Autodesk.Revit.UI.Selection;
//using ComponentManager = Autodesk.Windows.ComponentManager;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]

    public class ExportToSVG : IExternalCommand
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

            IList<Reference> collection = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select something");

            XYZ newOrigin = uidoc.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Select the new origin");

            double angle = 8;

            using (Transaction t = new Transaction(doc, "draw Lines"))
            {
                t.Start();

                StringBuilder sb = new StringBuilder();

                foreach (Reference eleRef in collection)
                {
                    Element e = doc.GetElement(eleRef);

                    sb.AppendLine(SvgWall(doc.ActiveView, e, angle, newOrigin));
                    //sb.AppendLine(SvgWall(e, doc));
                }

                File.WriteAllText(@"C:\Temp\svgRoom.txt", sb.ToString());

                t.Commit();
            }



            //TaskDialog.Show("r", path);

            return Result.Succeeded;
        }

        public XYZ MyTransform(XYZ pt, double alfa, XYZ newOrigine)
        {

            double radiantAngle = alfa * Math.PI / 180;

            TaskDialog.Show("r", radiantAngle.ToString());

            double a = newOrigine.X - XYZ.Zero.X;
            double b = newOrigine.Y - XYZ.Zero.Y;

            //TaskDialog.Show("r", "a=" + a.ToString() + "b=" + b.ToString());

            //TaskDialog.Show("r", WritePoint(new List<XYZ>{pt}, 1));

            double xtransformed = (pt.X - a) * Math.Cos(radiantAngle) - (pt.Y - b) * Math.Sin(radiantAngle);

            //TaskDialog.Show("r", "x transf: " + xtransformed.ToString());

            double ytransformed = (pt.X - a) * Math.Sin(radiantAngle) + (pt.Y - b) * Math.Cos(radiantAngle);

            //TaskDialog.Show("r", "y transf: " + ytransformed.ToString());

            pt = new XYZ(xtransformed, ytransformed, 0);

            return pt;
        }

        public Face GetElementTopFace(View view, Element elem)
        {
            //List<Solid> solidsFound = new List<Solid>();
            Face topFace = null;

            Options options = new Options()
            {
                View = view,
                ComputeReferences = false,
                IncludeNonVisibleObjects = false
            };

            // get the solid geometry from the element
            GeometryElement baseGeomElem = elem.get_Geometry(options);
            foreach (GeometryObject geomObj in baseGeomElem)
            {
                Solid solid = geomObj as Solid;
                if (solid != null && solid.Faces.Size != 0 && solid.Edges.Size != 0)
                {
                    foreach (Face fa in solid.Faces)
                    {
                        //TaskDialog.Show( "r", WritePoint(new List<XYZ> { fa.ComputeNormal(new UV(0.5, 0.5)) }, 1) );
                        if (fa.ComputeNormal(new UV(0.5, 0.5)).IsAlmostEqualTo(XYZ.BasisZ))
                        {
                            topFace = fa;
                            break;
                        }
                    }

                    //solidsFound.Add(solid);
                }
            }

            return topFace;
        }

        public string SvgWallOld(Element elementWall, Document doc)
        {
            Wall wall = elementWall as Wall;

            LocationCurve lc = elementWall.Location as LocationCurve;

            Line l = lc.Curve as Line;

            double scale = 304.8;

            XYZ startPoint = new XYZ(l.GetEndPoint(0).X * scale, l.GetEndPoint(0).Y * scale, 0);
            XYZ endPoint = new XYZ(l.GetEndPoint(1).X * scale, l.GetEndPoint(1).Y * scale, 0);



            XYZ lineDirection = (endPoint - startPoint).Normalize();
            XYZ perpDirection = lineDirection.CrossProduct(XYZ.BasisZ);

            double width = wall.Width * scale;


            XYZ corner1 = startPoint + perpDirection * width / 2;
            XYZ corner2 = endPoint + perpDirection * width / 2;
            XYZ corner3 = endPoint - perpDirection * width / 2;
            XYZ corner4 = startPoint - perpDirection * width / 2;

            //TaskDialog.Show("r", WritePoint(new List<XYZ> { corner1, corner2, corner3, corner4}));



            Line l1 = Line.CreateBound(corner1, corner2);
            doc.Create.NewDetailCurve(doc.ActiveView, l1);

            Line l2 = Line.CreateBound(corner2, corner3);
            doc.Create.NewDetailCurve(doc.ActiveView, l2);

            Line l3 = Line.CreateBound(corner3, corner4);
            doc.Create.NewDetailCurve(doc.ActiveView, l3);

            Line l4 = Line.CreateBound(corner4, corner1);
            doc.Create.NewDetailCurve(doc.ActiveView, l4);


            //return $"<rect x=\"{xPos}\" y=\"{yPos}\" width=\"{width}\" height=\"{height}\" class=\"wall_object\" \n/>";


            return $"\n<polygon points=\"{Math.Round(corner1.X, 3)},{Math.Round(corner1.Y, 3) }" +
                $" {Math.Round(corner2.X, 3)},{Math.Round(corner2.Y, 3)}" +
                $" {Math.Round(corner3.X, 3)},{Math.Round(corner3.Y, 3)} " +
                $" {Math.Round(corner4.X, 3)},{Math.Round(corner4.Y, 3)}\" />";
        }

        public string SvgWall(View view, Element elementWall, double rotAngle, XYZ newOrigin)
        {
            Face wallTopFace = GetElementTopFace(view, elementWall);

            IList<XYZ> surfacePoints = wallTopFace.Triangulate().Vertices;

            IList<XYZ> transformedPoints = new List<XYZ>();

            foreach (XYZ item in surfacePoints)
            {
                transformedPoints.Add(MyTransform(item, rotAngle, newOrigin));
            }

            return WriteSVG(transformedPoints, 304.8, "polygon points");
        }

        //public double scale = 304.8;
        public string WriteSVG(IList<XYZ> pts, double scale, string svgType)
        {
            //<polygon points="5140.274,-2404.297 5140.274,-1689.247 5040.274,-1689.247  5040.274,-2404.297" />
            string result = $"<{svgType}=\"";
            foreach (XYZ point in pts)
            {
                result += $"{Math.Round(point.X * scale, 3)},{Math.Round(point.Y * scale, 3)} ";
            };
            return $"{result}\"/>";
        }
    }
}
