#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using RG = Rhino.Geometry;



#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class IntersectPlaneMesh : IExternalCommand
    {
        static bool KnotAlmostEqualTo(double max, double min) => KnotAlmostEqualTo(max, min, 1.0e-09);

        static bool KnotAlmostEqualTo(double max, double min, double tol)
        {
            var length = max - min;
            if (length <= tol)
                return true;

            return length <= max * tol;
        }

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Reference adaptive = uidoc.Selection.PickObject(ObjectType.Element, "Select adaptive family");

            //Element adaptiveEle = doc.GetElement(adaptive);

            //PlanarFace pf = GetFace(adaptiveEle);

            //TaskDialog.Show("tr", "I am done");

            //TaskDialog.Show("tr", "so done");

            //TaskDialog.Show("tr", "done done");


            List<RG.Point3d> pts = new List<RG.Point3d> { new RG.Point3d(0, 0, 0), new RG.Point3d(5, 10, 0), new RG.Point3d(15, 5, 0), new RG.Point3d(20, 0, 0) };


            RG.PolylineCurve pc = new RG.PolylineCurve(pts);

            RG.Interval d = pc.Domain;

            //TaskDialog.Show("r", d.ToString());

            RG.NurbsCurve value = pc.ToNurbsCurve();

            value.IncreaseDegree(3);

            int newDegree = 3;
            var degree = value.Degree;
            var knots = ToDoubleArray(value.Knots, newDegree);
            var controlPoints = ToXYZArray(value.Points, 1);
            var weights = value.Points.ConvertAll(x => x.Weight);

            XYZ normal = new XYZ(0, 0, 1);
            XYZ origin = new XYZ(0, 0, 0);

            Plane rvtPlane = Plane.CreateByNormalAndOrigin(normal, origin);


            string knot = "";

            foreach (var item in knots)
            {
                knot += item.ToString() + "\n";
            }

            //TaskDialog.Show("r", knot);

            //TaskDialog.Show("R", $"ControlPoints > Degree: {controlPoints.Length} > {degree}\nKnots = degree + control points + 1 = {controlPoints.Length + degree + 1} ");

            Curve rvtN = NurbSpline.CreateCurve(newDegree, knots, controlPoints);

            //Trace.WriteLine()
            using (Transaction t = new Transaction(doc, "a"))
            {
                t.Start();
                SketchPlane sketchPlane = SketchPlane.Create(doc, rvtPlane);
                doc.Create.NewModelCurve(rvtN, sketchPlane);

                t.Commit();
            }

            return Result.Succeeded;
        }

        public static Curve ToCurve(RG.Curve value, double factor)
        {
            switch (value)
            {
                case RG.NurbsCurve nurbsCurve:
                    return ToCurve(nurbsCurve, factor);

                default:
                    return ToCurve(value.ToNurbsCurve(), factor);
            }
        }
        public static XYZ ToXYZ(RG.Point3d value, double factor)
        {
            return factor == 1.0 ?
              new XYZ(value.X, value.Y, value.Z) :
              new XYZ(value.X * factor, value.Y * factor, value.Z * factor);
        }
        public static Line ToLine(RG.Line value, double factor)
        {
            return Line.CreateBound(ToXYZ(value.From, factor), ToXYZ(value.To, factor));
        }

        internal static Curve ToNurbsSpline(RG.NurbsCurve value, double factor)
        {
            var degree = value.Degree;
            var knots = ToDoubleArray(value.Knots, degree);
            var controlPoints = ToXYZArray(value.Points, factor);

            if (value.IsRational)
            {
                var weights = value.Points.ConvertAll(x => x.Weight);
                return NurbSpline.CreateCurve(value.Degree, knots, controlPoints, weights);
            }
            else
            {
                return NurbSpline.CreateCurve(value.Degree, knots, controlPoints);
            }
        }

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


    public static class IListConverter
    {
        /// <summary>
        /// Converts an IList of one type to an IList of another type.
        /// </summary>
        /// <typeparam name="TInput">The type of the elements of the source IList.</typeparam>
        /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
        /// <param name="input">The <see cref="IList{T}"/> to convert to a target type.</param>
        /// <param name="converter">A <see cref="Converter{TInput, TOutput}"/> that converts each element from one type to another type.</param>
        /// <returns>An IList of the target type containing the converted elements from the source IList.</returns>
        public static IList<TOutput> ConvertAll<TInput, TOutput>(this IList<TInput> input, Converter<TInput, TOutput> converter)
        {
            if (input is TInput[] array)
                return Array.ConvertAll(array, converter);

            if (input is List<TInput> list)
                return list.ConvertAll(converter);

            var count = input.Count;
            var output = new TOutput[count];

            for (int i = 0; i < count; ++i)
                output[i] = converter(input[i]);

            return output;
        }
    }
}
