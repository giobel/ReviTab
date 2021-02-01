using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using RG = Rhino.Geometry;

namespace ReviTab.Buttons_Tools
{
    [Transaction(TransactionMode.Manual)]
    public class CreateNurbsModelCurve : IExternalCommand
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

            List<RG.Point3d> pts = new List<RG.Point3d> { new RG.Point3d(0, 0, 0), new RG.Point3d(5, 10, 0), new RG.Point3d(15, 0, 0), new RG.Point3d(20, 0, 0) };


            RG.PolylineCurve pc = new RG.PolylineCurve(pts);
                        

            RG.NurbsCurve nurb = pc.ToNurbsCurve();

            nurb.IncreaseDegree(3);

            var knots = ToDoubleArray(nurb.Knots, nurb.Degree);
            var controlPoints = ToXYZArray(nurb.Points, 1);
            var weights = nurb.Points.ConvertAll(x => x.Weight);

            XYZ normal = new XYZ(0, 0, 1);
            XYZ origin = new XYZ(0, 0, 0);

            Plane rvtPlane = Plane.CreateByNormalAndOrigin(normal, origin);

            //var plane = sketchPlane.GetPlane().ToPlane();
            Curve rvtN = NurbSpline.CreateCurve(nurb.Degree, knots, controlPoints);

            using (Transaction t = new Transaction(doc, "a"))
            {
                t.Start();
                SketchPlane sketchPlane = SketchPlane.Create(doc, rvtPlane);
                ModelCurve mc = doc.Create.NewModelCurve(rvtN, sketchPlane);
                TaskDialog.Show("r", mc.Id.ToString());
                t.Commit();
            }

            

            return Result.Succeeded;
        }

        #region FROM RHINO INSIDE REVIT GITHUB CODE
        static double[] ToDoubleArray(Rhino.Geometry.Collections.NurbsCurveKnotList list, int degree)
        {
            var count = list.Count;
            var knots = new double[count + 2];

            var min = list[0];
            var max = list[count - 1];
            var mid = 0.5 * (min + max);
            var factor = 1.0 / (max - min); // normalized

            // End knot
            knots[count + 1] = /*(list[count - 1] - max) * factor +*/ 1.0;
            for (int k = count - 1; k >= count - degree; --k)
                knots[k + 1] = /*(list[k] - max) * factor +*/ 1.0;

            // Interior knots (in reverse order)
            int multiplicity = degree + 1;
            for (int k = count - degree - 1; k >= degree; --k)
            {
                double current = list[k] <= mid ?
                  (list[k] - min) * factor + 0.0 :
                  (list[k] - max) * factor + 1.0;

                double next = knots[k + 2];
                if (KnotAlmostEqualTo(next, current))
                {
                    multiplicity++;
                    if (multiplicity > degree - 2)
                        current = KnotPrevNotEqual(next);
                    else
                        current = next;
                }
                else multiplicity = 1;

                knots[k + 1] = current;
            }

            // Start knot
            for (int k = degree - 1; k >= 0; --k)
                knots[k + 1] = /*(list[k] - min) * factor +*/ 0.0;
            knots[0] = /*(list[0] - min) * factor +*/ 0.0;

            return knots;
        }
        static double KnotPrevNotEqual(double max) => KnotPrevNotEqual(max, 1.0000000E-9 * 1000.0);
        static double KnotPrevNotEqual(double max, double tol)
        {
            const double delta2 = 2.0 * 1E-16;
            var value = max - tol - delta2;

            if (!KnotAlmostEqualTo(max, value, tol))
                return value;

            return max - (max * (tol + delta2));
        }
        static XYZ[] ToXYZArray(Rhino.Geometry.Collections.NurbsCurvePointList list, double factor)
        {
            var count = list.Count;
            var points = new XYZ[count];

            int p = 0;
            if (factor == 1.0)
            {
                while (p < count)
                {
                    var location = list[p].Location;
                    points[p++] = new XYZ(location.X, location.Y, location.Z);
                }
            }
            else
            {
                while (p < count)
                {
                    var location = list[p].Location;
                    points[p++] = new XYZ(location.X * factor, location.Y * factor, location.Z * factor);
                }
            }

            return points;
        }
        static bool KnotAlmostEqualTo(double max, double min) => KnotAlmostEqualTo(max, min, 1.0e-09);
        static bool KnotAlmostEqualTo(double max, double min, double tol)
        {
            var length = max - min;
            if (length <= tol)
                return true;

            return length <= max * tol;
        }
        #endregion
    }
}
