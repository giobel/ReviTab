using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace ReviTab
{
    class VoidByLineHelpers
    {

        public static Dictionary<double, int[]> IntersectionPoint(Document doc, Reference refBeam, IList<Reference> refLines)
        {

            Element ele = doc.GetElement(refBeam.ElementId);

            Face webFace = null;

            Options geomOptions = pickOptions(doc);

            GeometryElement beamGeom = ele.get_Geometry(geomOptions);

            Transform instTransform = null;

            Dictionary<int, Face> areas = new Dictionary<int, Face>();

            foreach (GeometryObject obj in beamGeom)
            {
                Solid geomSolid = obj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        try
                        {
                            areas.Add((int)geomFace.Area, geomFace);

                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    Helpers.GetSymbolGeometry(obj, areas, out instTransform);
                }
            }

            int total = areas.Keys.Count;

            int maxArea = areas.Keys.Max();
            webFace = areas[maxArea];

            XYZ location = null;
            XYZ beamDirection = null;

            Helpers.BeamStartUVPoint(ele, webFace, out location, out beamDirection);

            XYZ faceOrigin = null;


            // the element does not have available solid geometries. Need to use its geometry instance first and transform the point to the project coordinates.
            if (beamGeom.Count() == 1)
            {

                XYZ transformedPoint = instTransform.OfPoint(location);
                //LocationCurve lc = ele.Location as LocationCurve;
                //XYZ direction = lc.Curve.GetEndPoint(0) - lc.Curve.GetEndPoint(1);

                faceOrigin = new XYZ(transformedPoint.X, transformedPoint.Y, 0);


            }

            else
            {
                faceOrigin = new XYZ(location.X, location.Y, 0);

            }


            LocationCurve beamLine = ele.Location as LocationCurve;
            Line bl = beamLine.Curve as Line;

            XYZ origin = bl.GetEndPoint(0);
            /*
         	XYZ originProjected = new XYZ (origin.X, origin.Y, 0);
         	*/

            List<double> distances = new List<double>();

            Dictionary<double, int[]> penoDistAndSize = new Dictionary<double, int[]>();


            foreach (Reference r in refLines)
            {
                Element refLine = doc.GetElement(r.ElementId);
                LocationCurve lineCrv = refLine.Location as LocationCurve;
                Line l = lineCrv.Curve as Line;

                try
                {
                    //XYZ intersectionPoint = Intersection(bl, l);
                    XYZ intersectionPoint = GetIntersection(bl, l);
                    XYZ intersectionPointProjected = new XYZ(intersectionPoint.X, intersectionPoint.Y, 0);
                    double d = intersectionPointProjected.DistanceTo(faceOrigin);
                    distances.Add(d * 304.8);

                    int penoWidth = Int16.Parse(GetLinestyle(doc, r)[0]);

                    int penoDepth = Int16.Parse(GetLinestyle(doc, r)[1]);

                    penoDistAndSize.Add(d * 304.8, new int[] { penoWidth, penoDepth });

                }
                catch
                {
                    //do nothing	
                }
            }


            return penoDistAndSize;

        }//close method

        public static XYZ Intersection(Curve c1, Curve c2)
        {
            XYZ p1 = c1.GetEndPoint(0);
            XYZ q1 = c1.GetEndPoint(1);
            XYZ p2 = c2.GetEndPoint(0);
            XYZ q2 = c2.GetEndPoint(1);
            XYZ v1 = q1 - p1;
            XYZ v2 = q2 - p2;
            XYZ w = p2 - p1;

            XYZ p5 = null;

            double c = (v2.X * w.Y - v2.Y * w.X)
              / (v2.X * v1.Y - v2.Y * v1.X);

            if (!double.IsInfinity(c))
            {
                double x = p1.X + c * v1.X;
                double y = p1.Y + c * v1.Y;

                p5 = new XYZ(x, y, 0);
            }
            return p5;
        }

        public static XYZ GetIntersection(Line line1, Line line2)
        {
            IntersectionResultArray results;

            SetComparisonResult result
              = line1.Intersect(line2, out results);

            if (result != SetComparisonResult.Overlap)
                throw new InvalidOperationException(
                  "Input lines did not intersect.");

            if (results == null || results.Size != 1)
                throw new InvalidOperationException(
                  "Could not extract line intersection point.");

            IntersectionResult iResult
              = results.get_Item(0);

            return iResult.XYZPoint;
        }

        private static Options pickOptions(Document doc)
        {
            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.View = doc.ActiveView;

            return geomOptions;
        }

        public static string[] GetLinestyle(Document doc, Reference refLine)
        {

            //			width = 0;
            //			depth = 0;

            DetailLine line = doc.GetElement(refLine.ElementId) as DetailLine;

            string[] penoWidthDepth = line.LineStyle.Name.Split(' ')[3].Split('x');
            //string[] sa = line.LineStyle.Name.Split(' ');

            //			width = Int16.Parse(sa[0]);
            //			depth = Int16.Parse(sa[1]);

            return penoWidthDepth;
        }

        private static double SignedDistanceTo(Plane plane, XYZ p)
        {

            XYZ v = p - plane.Origin;

            return plane.Normal.DotProduct(v);
        }

        private static XYZ ProjectOnto(Plane plane, XYZ p)
        {
            double d = SignedDistanceTo(plane, p);

            //XYZ q = p + d * plane.Normal; // wrong according to Ruslan Hanza and Alexander Pekshev in their comments below

            XYZ q = p - d * plane.Normal;


            return q;
        }

        public static void DrawDimension(Document doc, Reference refPlaneLine, FamilyInstance fi1, double offset)
        {

            ReferencePlane refP = doc.GetElement(refPlaneLine) as ReferencePlane;

            //FamilyInstance fi1 = doc.GetElement(opening1) as FamilyInstance;
            IList<Reference> fir1 = fi1.GetReferences(FamilyInstanceReferenceType.WeakReference);

            XYZ refPlanePoint = refP.FreeEnd;//end point of reference plane

            LocationPoint lp = fi1.Location as LocationPoint;

            XYZ direction = refP.Normal;//perpendicular direction to reference plane


            Plane p = Plane.CreateByNormalAndOrigin(direction, refP.FreeEnd);

            XYZ startPoint = ProjectOnto(p, lp.Point) + offset * refP.Direction;

            double distance = 1000;

            //direction = direction / direction.GetLength();

            XYZ endPoint = startPoint + distance * direction;

            Line dimensionLine = Line.CreateBound(startPoint, endPoint);

            ReferenceArray references = new ReferenceArray();

            references.Append(fir1.First());
            references.Append(refPlaneLine);

            //			lockedAlign = doc.Create.NewAlignment(view, r1, r2)
            Dimension d = doc.Create.NewDimension(doc.ActiveView, dimensionLine, references);
            d.IsLocked = true;
        }

        private static XYZ Simplify(XYZ point)
        {

            return new XYZ(Math.Round(point.X, 3), Math.Round(point.Y, 3), 0);
        }

        public static bool IsParallel(FamilyInstance fi, ReferencePlane refP)
        {

            XYZ fiHand = Simplify(fi.HandOrientation);
            XYZ refPnormal = Simplify(refP.Normal);
            XYZ cp = fiHand.CrossProduct(refPnormal);

            if (cp.IsAlmostEqualTo(XYZ.Zero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
