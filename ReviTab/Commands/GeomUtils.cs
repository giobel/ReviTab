using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    class GeomUtils
    {
        /*
        public static Plane GetPlaneFromCurve(Curve c, bool planarOnly)
        {
            //find the plane of the curve and generate a sketch plane
            double period = c.IsBound ? 0.0 : (c.IsCyclic ? c.Period : 1.0);

            var p0 = c.IsBound ? c.Evaluate(0.0, true) : c.Evaluate(0.0, false);
            var p1 = c.IsBound ? c.Evaluate(0.5, true) : c.Evaluate(0.25 * period, false);
            var p2 = c.IsBound ? c.Evaluate(1.0, true) : c.Evaluate(0.5 * period, false);

            if (IsLineLike(c))
            {
                XYZ norm = null;

                //keep old plane computations
                if (Math.Abs(p0.Z - p2.Z) < Tolerance)
                {
                    norm = XYZ.BasisZ;
                }
                else
                {
                    var v1 = p1 - p0;
                    var v2 = p2 - p0;

                    var p3 = new XYZ(p2.X, p2.Y, p0.Z);
                    var v3 = p3 - p0;
                    norm = v1.CrossProduct(v3);
                    if (norm.IsZeroLength())
                    {
                        norm = v2.CrossProduct(XYZ.BasisY);
                    }
                    norm = norm.Normalize();
                }

                return Plane.CreateByNormalAndOrigin(norm, p0);

            }

            var cLoop = new CurveLoop();
            cLoop.Append(c.Clone());
            if (cLoop.HasPlane())
            {
                return cLoop.GetPlane();
            }
            if (planarOnly)
                return null;

            // Get best fit plane using tesselation
            //var points = c.Tessellate().Select(x => x.ToPoint(false));

            //var bestFitPlane = Autodesk.DesignScript.Geometry.Plane.ByBestFitThroughPoints(points);

            return bestFitPlane.ToPlane(false);
        }*/
    }
}
