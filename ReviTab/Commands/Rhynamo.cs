using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Rhino.FileIO;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Collections;

namespace Rhynamo
{
    class clsGeometryConversionUtils
    {
        public clsGeometryConversionUtils()
        {
            // widen scope
        }

        /// <summary>
        /// Converts Rhino lines to DesignScript lines.
        /// </summary>
        /// <param name="rh_lines">List of Rhino lines</param>
        /// <returns>List of DesignScript lines</returns>
        public List<DetailCurve> Convert_LinesToDS(Document doc, List<Rhino.Geometry.LineCurve> rh_lines, string layer)
        {

                List<DetailCurve> ds_lines = new List<DetailCurve>();
                foreach (Rhino.Geometry.LineCurve ln in rh_lines)
                {
                    // Rhino start and end points
                    Point3d rh_start = ln.PointAtStart;
                    Point3d rh_end = ln.PointAtEnd;

                    try
                    {
                        // convert end points
                        XYZ ds_start = new XYZ(rh_start.X, rh_start.Y, rh_start.Z);
                        XYZ ds_end = new XYZ(rh_end.X, rh_end.Y, rh_end.Z);

                        // make Revit line
                        Autodesk.Revit.DB.Line ds_ln = Autodesk.Revit.DB.Line.CreateBound(ds_start, ds_end);

                        DetailCurve dc = doc.Create.NewDetailCurve(doc.ActiveView, ds_ln);

                        var styles = dc.GetLineStyleIds();
                        foreach (var styleId in styles)
                        {
                            var styleEle = doc.GetElement(styleId);
                            if (styleEle.Name == layer)
                            {
                                dc.LineStyle = styleEle;
                                break;
                            }
                        }

                    ds_lines.Add(dc);
                    }
                    catch { ds_lines.Add(null); }
                }
                return ds_lines;

        }


        /*
        #region "Rhino to DesignScript"
        /// <summary>
        /// Converts Rhino points to DesignScript points
        /// </summary>
        /// <param name="rh_points">list of Rhino points</param>
        /// <returns>List of DesignScript points</returns>
        public List<Autodesk.Revit.DB.Point> Convert_PointsToDS(List<Rhino.Geometry.Point> rh_points)
        {
            try
            {
                List<Autodesk.Revit.DB.Point> ds_points = new List<Autodesk.Revit.DB.Point>();
                foreach (Rhino.Geometry.Point pt in rh_points)
                {
                    // get x, y, z coordinates
                    double x = pt.Location.X;
                    double y = pt.Location.Y;
                    double z = pt.Location.Z;

                    try
                    {
                        // create DesignScript point
                        Autodesk.Revit.DB.Point ds_pt = Autodesk.Revit.DB.Point.ByCoordinates(x, y, z);
                        ds_points.Add(ds_pt);
                    }
                    catch { ds_points.Add(null); }

                }
                return ds_points;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino lines to DesignScript lines.
        /// </summary>
        /// <param name="rh_lines">List of Rhino lines</param>
        /// <returns>List of DesignScript lines</returns>
        public List<Autodesk.Revit.DB.Line> Convert_LinesToDS(List<Rhino.Geometry.LineCurve> rh_lines)
        {
            try
            {
                List<Autodesk.Revit.DB.Line> ds_lines = new List<Autodesk.Revit.DB.Line>();
                foreach (Rhino.Geometry.LineCurve ln in rh_lines)
                {
                    // Rhino start and end points
                    Point3d rh_start = ln.PointAtStart;
                    Point3d rh_end = ln.PointAtEnd;

                    try
                    {
                        // convert end points
                        Autodesk.Revit.DB.Point ds_start = Autodesk.Revit.DB.Point.ByCoordinates(rh_start.X, rh_start.Y, rh_start.Z);
                        Autodesk.Revit.DB.Point ds_end = Autodesk.Revit.DB.Point.ByCoordinates(rh_end.X, rh_end.Y, rh_end.Z);

                        // make DesignScript line
                        Autodesk.Revit.DB.Line ds_ln = Autodesk.Revit.DB.Line.ByStartPointEndPoint(ds_start, ds_end);
                        ds_lines.Add(ds_ln);
                    }
                    catch { ds_lines.Add(null); }
                }
                return ds_lines;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino arcs to DesignScript arcs
        /// </summary>
        /// <param name="rh_arcs">List of Rhino arcs</param>
        /// <returns>List of DesignScript arcs</returns>
        public List<Autodesk.Revit.DB.Arc> Convert_ArcsToDS(List<Rhino.Geometry.ArcCurve> rh_arcs)
        {
            try
            {
                List<Autodesk.Revit.DB.Arc> ds_arcs = new List<Autodesk.Revit.DB.Arc>();
                foreach (Rhino.Geometry.ArcCurve arc in rh_arcs)
                {
                    //get start, mid, and end points
                    Point3d rh_center = arc.Arc.Center;
                    Point3d rh_start = arc.Arc.StartPoint;
                    Point3d rh_end = arc.Arc.EndPoint;

                    try
                    {
                        //convert points
                        List<Autodesk.Revit.DB.Point> ds_pts = new List<Autodesk.Revit.DB.Point>();
                        Autodesk.Revit.DB.Point ds_center = Autodesk.Revit.DB.Point.ByCoordinates(rh_center.X, rh_center.Y, rh_center.Z);
                        Autodesk.Revit.DB.Point ds_start = Autodesk.Revit.DB.Point.ByCoordinates(rh_start.X, rh_start.Y, rh_start.Z);
                        Autodesk.Revit.DB.Point ds_end = Autodesk.Revit.DB.Point.ByCoordinates(rh_end.X, rh_end.Y, rh_end.Z);

                        //creat DesignScript arc
                        Autodesk.Revit.DB.Arc ds_arc = Autodesk.Revit.DB.Arc.ByCenterPointStartPointEndPoint(ds_center, ds_start, ds_end);
                        ds_arcs.Add(ds_arc);
                    }
                    catch { ds_arcs.Add(null); }


                }
                return ds_arcs;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino circles to DesignScript circles
        /// </summary>
        /// <param name="rh_crvs">Circle curve</param>
        /// <returns>DesignScript circle.</returns>
        public List<Autodesk.Revit.DB.Circle> Convert_CirclesToDS(List<Rhino.Geometry.Curve> rh_crvs)
        {
            try
            {
                List<Autodesk.Revit.DB.Circle> ds_circles = new List<Autodesk.Revit.DB.Circle>();
                foreach (Rhino.Geometry.Curve rh_crv in rh_crvs)
                {
                    Rhino.Geometry.Circle rh_circle;
                    rh_crv.TryGetCircle(out rh_circle);
                    Rhino.Geometry.Plane rh_center = rh_circle.Plane;
                    double rh_radius = rh_circle.Radius;

                    try
                    {
                        // circle constructors
                        Autodesk.Revit.DB.Point origin = Autodesk.Revit.DB.Point.ByCoordinates(rh_center.Origin.X, rh_center.Origin.Y, rh_center.Origin.Z);
                        Autodesk.Revit.DB.Vector normal = Autodesk.Revit.DB.Vector.ByCoordinates(rh_center.ZAxis.X, rh_center.ZAxis.Y, rh_center.ZAxis.Z);
                        Autodesk.Revit.DB.Plane ds_plane = Autodesk.Revit.DB.Plane.ByOriginNormal(origin, normal);

                        // make designscript circle
                        Autodesk.Revit.DB.Circle ds_circle = Autodesk.Revit.DB.Circle.ByPlaneRadius(ds_plane, rh_radius);
                        ds_circles.Add(ds_circle);
                    }
                    catch { ds_circles.Add(null); }

                }

                return ds_circles;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino circles to DesignScript circles
        /// </summary>
        /// <param name="rh_crvs">Circle curve</param>
        /// <returns>DesignScript circle.</returns>
        public List<Autodesk.Revit.DB.Ellipse> Convert_EllipsesToDS(List<Rhino.Geometry.Curve> rh_crvs)
        {
            try
            {
                List<Autodesk.Revit.DB.Ellipse> ds_ellipses = new List<Autodesk.Revit.DB.Ellipse>();
                foreach (Rhino.Geometry.Curve rh_crv in rh_crvs)
                {
                    Rhino.Geometry.Ellipse rh_ellipse;
                    Rhino.Geometry.Plane rh_plane;

                    // get plan and ellipse.
                    rh_crv.TryGetPlane(out rh_plane);
                    rh_crv.TryGetEllipse(rh_plane, out rh_ellipse);

                    Rhino.Geometry.Plane rh_center = rh_ellipse.Plane;
                    double radius1 = rh_ellipse.Radius1;
                    double radius2 = rh_ellipse.Radius2;

                    try
                    {
                        // ellipse constructors
                        Autodesk.Revit.DB.Point origin = Autodesk.Revit.DB.Point.ByCoordinates(rh_center.Origin.X, rh_center.Origin.Y, rh_center.Origin.Z);
                        Autodesk.Revit.DB.Vector normal = Autodesk.Revit.DB.Vector.ByCoordinates(rh_center.ZAxis.X, rh_center.ZAxis.Y, rh_center.ZAxis.Z);
                        Autodesk.Revit.DB.Plane ds_plane = Autodesk.Revit.DB.Plane.ByOriginNormal(origin, normal);

                        // make designscript circle
                        Autodesk.Revit.DB.Ellipse ds_ellipse = Autodesk.Revit.DB.Ellipse.ByPlaneRadii(ds_plane, radius1, radius2);
                        ds_ellipses.Add(ds_ellipse);
                    }
                    catch { ds_ellipses.Add(null); }

                }

                return ds_ellipses;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino Nurbs curves to DesignScript Nurbs curves
        /// </summary>
        /// <param name="rh_nurbscrv">List of Rhino Nurbs curves</param>
        /// <returns>List of DesignScript Nurbs curves</returns>
        public List<Autodesk.Revit.DB.NurbsCurve> Convert_NurbsCurveToDS(List<Rhino.Geometry.NurbsCurve> rh_nurbscrv)
        {
            try
            {
                List<Autodesk.Revit.DB.NurbsCurve> ds_nurbs = new List<Autodesk.Revit.DB.NurbsCurve>();
                foreach (Rhino.Geometry.NurbsCurve nurbs in rh_nurbscrv)
                {
                    // get points & knots
                    NurbsCurvePointList nurbspts = nurbs.Points;
                    NurbsCurveKnotList nurbsknots = nurbs.Knots;
                    int nurbsdegree = nurbs.Degree;

                    // list for weights
                    List<Autodesk.Revit.DB.Point> ds_pts = new List<Autodesk.Revit.DB.Point>();
                    List<double> ds_knots = new List<double>();
                    List<double> ds_weights = new List<double>();

                    // points and weights
                    for (int i = 0; i < nurbspts.Count; i++)
                    {
                        Point3d nurbspt = nurbspts[i].Location;
                        double x = nurbspt.X;
                        double y = nurbspt.Y;
                        double z = nurbspt.Z;

                        Autodesk.Revit.DB.Point pt = Autodesk.Revit.DB.Point.ByCoordinates(x, y, z);
                        double weight = nurbspts[i].Weight;

                        ds_pts.Add(pt);
                        ds_weights.Add(weight);
                    }

                    // knots
                    ds_knots.Add(nurbsknots[0]);
                    for (int i = 0; i < nurbsknots.Count; i++)
                    {
                        double knot = nurbsknots[i];
                        ds_knots.Add(knot);
                    }
                    ds_knots.Add(nurbsknots[nurbsknots.Count - 1]);

                    try
                    {
                        // create new nurbs curve
                        Autodesk.Revit.DB.NurbsCurve ds_nurbscrv = Autodesk.Revit.DB.NurbsCurve.ByControlPointsWeightsKnots(ds_pts, ds_weights.ToArray(), ds_knots.ToArray(), nurbsdegree);
                        ds_nurbs.Add(ds_nurbscrv);
                    }
                    catch { ds_nurbs.Add(null); }

                }
                return ds_nurbs;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino Nurbs curves to DesignScript Nurbs curves
        /// </summary>
        /// <param name="rh_nurbscrv">List of Rhino Nurbs curves</param>
        /// <returns>List of DesignScript Nurbs curves</returns>
        public List<clsRhinoMultiSpanNurbs> Convert_MultiSpanNurbsCurveToDS(List<Rhino.Geometry.NurbsCurve> rh_nurbscrv)
        {
            try
            {
                List<clsRhinoMultiSpanNurbs> ds_polycurves = new List<clsRhinoMultiSpanNurbs>();
                foreach (Rhino.Geometry.NurbsCurve m_nurbs in rh_nurbscrv)
                {
                    List<Autodesk.Revit.DB.NurbsCurve> ds_nurbs = new List<Autodesk.Revit.DB.NurbsCurve>();

                    List<Rhino.Geometry.NurbsCurve> m_nurbsspan = new List<Rhino.Geometry.NurbsCurve>();
                    int m_spancount = m_nurbs.SpanCount;
                    for (int i = 0; i < m_nurbs.SpanCount; i++)
                    {
                        Interval m_interval = m_nurbs.SpanDomain(i);
                        m_nurbsspan.Add(m_nurbs.ToNurbsCurve(m_interval));
                    }
                    try
                    {
                        foreach (Rhino.Geometry.NurbsCurve nurbs in m_nurbsspan)
                        {
                            // get points & knots
                            NurbsCurvePointList nurbspts = nurbs.Points;
                            NurbsCurveKnotList nurbsknots = nurbs.Knots;
                            int nurbsdegree = nurbs.Degree;

                            // list for weights
                            List<Autodesk.Revit.DB.Point> ds_pts = new List<Autodesk.Revit.DB.Point>();
                            List<double> ds_knots = new List<double>();
                            List<double> ds_weights = new List<double>();

                            // points and weights
                            for (int i = 0; i < nurbspts.Count; i++)
                            {
                                Point3d nurbspt = nurbspts[i].Location;
                                double x = nurbspt.X;
                                double y = nurbspt.Y;
                                double z = nurbspt.Z;

                                Autodesk.Revit.DB.Point pt = Autodesk.Revit.DB.Point.ByCoordinates(x, y, z);
                                double weight = nurbspts[i].Weight;

                                ds_pts.Add(pt);
                                ds_weights.Add(weight);
                            }

                            // knots
                            ds_knots.Add(nurbsknots[0]);
                            for (int i = 0; i < nurbsknots.Count; i++)
                            {
                                double knot = nurbsknots[i];
                                ds_knots.Add(knot);
                            }
                            ds_knots.Add(nurbsknots[nurbsknots.Count - 1]);

                            // create new nurbs curve
                            Autodesk.Revit.DB.NurbsCurve ds_nurbscrv = Autodesk.Revit.DB.NurbsCurve.ByControlPointsWeightsKnots(ds_pts, ds_weights.ToArray(), ds_knots.ToArray(), nurbsdegree);
                            ds_nurbs.Add(ds_nurbscrv);
                        }
                        clsRhinoMultiSpanNurbs pcurve = new clsRhinoMultiSpanNurbs(ds_nurbs);
                        ds_polycurves.Add(pcurve);
                    }
                    catch { ds_polycurves.Add(null); }

                }
                return ds_polycurves;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino Polyline to DesignScript Polyline
        /// </summary>
        /// <param name="rh_polyline">Rhino Polyline</param>
        /// <returns>DesignScript Polyline</returns>
        public List<Autodesk.Revit.DB.PolyCurve> Convert_PolylineToDS(List<Rhino.Geometry.PolylineCurve> rh_polyline)
        {
            try
            {
                List<Autodesk.Revit.DB.PolyCurve> ds_polycurves = new List<Autodesk.Revit.DB.PolyCurve>();

                foreach (Rhino.Geometry.PolylineCurve pline in rh_polyline)
                {
                    List<Autodesk.Revit.DB.Curve> ds_segments = new List<Autodesk.Revit.DB.Curve>();
                    for (int i = 0; i < pline.PointCount - 1; i++)
                    {
                        // start point
                        double x1 = pline.Point(i).X;
                        double y1 = pline.Point(i).Y;
                        double z1 = pline.Point(i).Z;

                        // end point
                        double x2 = pline.Point(i + 1).X;
                        double y2 = pline.Point(i + 1).Y;
                        double z2 = pline.Point(i + 1).Z;

                        // line segment
                        Autodesk.Revit.DB.Point ds_pt1 = Autodesk.Revit.DB.Point.ByCoordinates(x1, y1, z1);
                        Autodesk.Revit.DB.Point ds_pt2 = Autodesk.Revit.DB.Point.ByCoordinates(x2, y2, z2);
                        Autodesk.Revit.DB.Line ds_line = Autodesk.Revit.DB.Line.ByStartPointEndPoint(ds_pt1, ds_pt2);
                        ds_segments.Add(ds_line);
                    }

                    try
                    {
                        // Create DesignScript polycurve
                        Autodesk.Revit.DB.PolyCurve ds_polycurve = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(ds_segments);
                        ds_polycurves.Add(ds_polycurve);
                    }
                    catch { ds_polycurves.Add(null); }
                }

                return ds_polycurves;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino PolyCurve to DesignScript PolyCurve
        /// </summary>
        /// <param name="rh_polycurve">Rhino PolyCurve</param>
        /// <returns>DesignScript PolyCurve</returns>
        public List<Autodesk.Revit.DB.PolyCurve> Convert_PolyCurveToDS(List<Rhino.Geometry.PolyCurve> rh_polycurve)
        {
            try
            {

                List<Autodesk.Revit.DB.PolyCurve> ds_polycurves = new List<Autodesk.Revit.DB.PolyCurve>();

                // get and organize segments
                foreach (Rhino.Geometry.PolyCurve pcrv in rh_polycurve)
                {
                    // segment types
                    List<Rhino.Geometry.LineCurve> rh_lns = new List<Rhino.Geometry.LineCurve>();
                    List<Rhino.Geometry.ArcCurve> rh_arcs = new List<Rhino.Geometry.ArcCurve>();
                    List<Rhino.Geometry.NurbsCurve> rh_nurbs = new List<Rhino.Geometry.NurbsCurve>();
                    List<Rhino.Geometry.NurbsCurve> rh_mspannurbs = new List<Rhino.Geometry.NurbsCurve>();
                    List<Rhino.Geometry.PolyCurve> rh_polycurves = new List<Rhino.Geometry.PolyCurve>();
                    List<Rhino.Geometry.PolylineCurve> rh_polylines = new List<Rhino.Geometry.PolylineCurve>();

                    List<Autodesk.Revit.DB.Curve> ds_segments = new List<Autodesk.Revit.DB.Curve>();

                    for (int i = 0; i < pcrv.SegmentCount; i++)
                    {
                        Rhino.Geometry.Curve crv = pcrv.SegmentCurve(i);
                        if (crv is Rhino.Geometry.LineCurve)
                        {
                            rh_lns.Add(crv as LineCurve);
                        }
                        else if (crv is Rhino.Geometry.ArcCurve)
                        {
                            rh_arcs.Add(crv as ArcCurve);
                        }
                        else if (crv is Rhino.Geometry.NurbsCurve && crv.SpanCount == 1)
                        {
                            rh_nurbs.Add(crv as Rhino.Geometry.NurbsCurve);
                        }
                        else if (crv is Rhino.Geometry.NurbsCurve && crv.SpanCount > 1)
                        {
                            rh_mspannurbs.Add(crv as Rhino.Geometry.NurbsCurve);
                        }
                        else if (crv is Rhino.Geometry.PolylineCurve)
                        {
                            rh_polylines.Add(crv as Rhino.Geometry.PolylineCurve);
                        }
                        else if (crv is Rhino.Geometry.PolyCurve)
                        {
                            rh_mspannurbs.Add(crv.ToNurbsCurve());
                        }
                    }

                    // convert lists of curve segments
                    Rhynamo.classes.clsGeometryConversionUtils rh_ds = new Rhynamo.classes.clsGeometryConversionUtils();
                    List<Autodesk.Revit.DB.Line> ds_lns = rh_ds.Convert_LinesToDS(rh_lns);
                    List<Autodesk.Revit.DB.Arc> ds_arcs = rh_ds.Convert_ArcsToDS(rh_arcs);
                    List<Autodesk.Revit.DB.PolyCurve> ds_polylines = rh_ds.Convert_PolylineToDS(rh_polylines);
                    List<Autodesk.Revit.DB.NurbsCurve> ds_nurbs = rh_ds.Convert_NurbsCurveToDS(rh_nurbs);
                    List<clsRhinoMultiSpanNurbs> ds_mspannurbs = rh_ds.Convert_MultiSpanNurbsCurveToDS(rh_mspannurbs);

                    // create single segement list
                    foreach (Autodesk.Revit.DB.Line ln in ds_lns)
                    {
                        ds_segments.Add(ln);
                    }

                    foreach (Autodesk.Revit.DB.Arc arc in ds_arcs)
                    {
                        ds_segments.Add(arc);
                    }

                    foreach (Autodesk.Revit.DB.NurbsCurve nurbs in ds_nurbs)
                    {
                        ds_segments.Add(nurbs);
                    }

                    foreach (clsRhinoMultiSpanNurbs pc in ds_mspannurbs)
                    {
                        //List<Autodesk.Revit.DB.Curve> ds_pcrvseg = new List<Autodesk.Revit.DB.Curve>();
                        //foreach (Autodesk.Revit.DB.NurbsCurve n in pc.MultiSpanNurbsList)
                        //{
                        //  ds_pcrvseg.Add(n);
                        //}
                        //Autodesk.Revit.DB.PolyCurve polycrv = IterativeJoinCurves(ds_pcrvseg);
                        //if (polycrv != null)
                        //{
                        //  ds_segments.Add(polycrv);
                        //}
                        foreach (Autodesk.Revit.DB.NurbsCurve n in pc.MultiSpanNurbsList)
                        {
                            ds_segments.Add(n);
                        }
                    }

                    try
                    {
                        // Create DesignScript polycurve
                        //remove duplicates
                        Autodesk.Revit.DB.PolyCurve ds_polycurve = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(ds_segments);
                        ds_polycurves.Add(ds_polycurve);
                    }
                    catch { ds_polycurves.Add(null); }
                }

                // return DesignScript polycurve list
                return ds_polycurves;
            }
            catch { return null; }

        }

        /// <summary>
        /// Converts all Rhino curve types to DesignScript Curve
        /// </summary>
        /// <param name="rh_curves">Rhino Curves</param>
        /// <returns>DesignScript Curves</returns>
        public List<Autodesk.Revit.DB.Curve> Convert_CurvesToDS(List<Rhino.Geometry.Curve> rh_curves)
        {
            try
            {
                // rhino curve list
                List<Rhino.Geometry.LineCurve> rh_lines = new List<Rhino.Geometry.LineCurve>();
                List<Rhino.Geometry.ArcCurve> rh_arcs = new List<Rhino.Geometry.ArcCurve>();
                List<Rhino.Geometry.NurbsCurve> rh_nurbscrvs = new List<Rhino.Geometry.NurbsCurve>();
                List<Rhino.Geometry.NurbsCurve> rh_mspannurbscrvs = new List<Rhino.Geometry.NurbsCurve>();
                List<Rhino.Geometry.PolylineCurve> rh_polylines = new List<Rhino.Geometry.PolylineCurve>();
                List<Rhino.Geometry.PolyCurve> rh_polycurves = new List<Rhino.Geometry.PolyCurve>();
                List<Rhino.Geometry.Curve> rh_circles = new List<Rhino.Geometry.Curve>();

                List<int> rh_lnint = new List<int>();
                List<int> rh_arcint = new List<int>();
                List<int> rh_nurbint = new List<int>();
                List<int> rh_mspanint = new List<int>();
                List<int> rh_plineint = new List<int>();
                List<int> rh_pcrvint = new List<int>();
                List<int> rh_circint = new List<int>();

                int count = 0;
                foreach (Rhino.Geometry.Curve geo in rh_curves)
                {

                    // check if geometry is a line curve
                    if (geo is Rhino.Geometry.LineCurve)
                    {
                        // add line curve to lines list
                        Rhino.Geometry.LineCurve ln = geo as Rhino.Geometry.LineCurve;
                        rh_lines.Add(ln);
                        rh_lnint.Add(count);
                    }

                    // check if geometry is an arc curve
                    if (geo is Rhino.Geometry.ArcCurve && geo.IsCircle() == false)
                    {
                        // add arc curve to arc list
                        Rhino.Geometry.ArcCurve arc = geo as Rhino.Geometry.ArcCurve;
                        rh_arcs.Add(arc);
                        rh_arcint.Add(count);
                    }

                    // check if the object is a nurbs curve (single span)
                    if (geo is Rhino.Geometry.NurbsCurve && geo.IsCircle() == false && geo.SpanCount == 1)
                    {
                        // add nurbs curve to nurbs list
                        Rhino.Geometry.NurbsCurve nurbs = geo as Rhino.Geometry.NurbsCurve;
                        rh_nurbscrvs.Add(nurbs);
                        rh_nurbint.Add(count);
                    }

                    // check if the object is a nurbs curve (multi span)
                    if (geo is Rhino.Geometry.NurbsCurve && geo.IsCircle() == false && geo.SpanCount > 1)
                    {
                        // add nurbs curve to nurbs list
                        Rhino.Geometry.NurbsCurve nurbs = geo as Rhino.Geometry.NurbsCurve;
                        rh_mspannurbscrvs.Add(nurbs);
                        rh_mspanint.Add(count);
                    }

                    // check if the object is a Polyline
                    if (geo is Rhino.Geometry.PolylineCurve)
                    {
                        // add polyline to Poly list
                        Rhino.Geometry.PolylineCurve polycrv = geo as Rhino.Geometry.PolylineCurve;
                        rh_polylines.Add(polycrv);
                        rh_plineint.Add(count);
                    }

                    // check if the object is a Poly curve
                    if (geo is Rhino.Geometry.PolyCurve)
                    {
                        // add polycurve to Poly list
                        Rhino.Geometry.PolyCurve polycrv = geo as Rhino.Geometry.PolyCurve;
                        rh_polycurves.Add(polycrv);
                        rh_pcrvint.Add(count);
                    }

                    // check if the object is a circle
                    if (geo.IsCircle() == true)
                    {
                        // add polycurve to Circle
                        Rhino.Geometry.Curve circle = (Rhino.Geometry.Curve)geo;
                        rh_circles.Add(circle);
                        rh_circint.Add(count);
                    }

                    count++;
                }

                // conversion to design script
                List<Autodesk.Revit.DB.Curve> ds_curves = new List<Autodesk.Revit.DB.Curve>();
                List<int> ds_ints = new List<int>();

                Rhynamo.classes.clsGeometryConversionUtils rh_ds = new Rhynamo.classes.clsGeometryConversionUtils();
                List<Autodesk.Revit.DB.Line> ds_lines = rh_ds.Convert_LinesToDS(rh_lines);
                List<Autodesk.Revit.DB.Arc> ds_arcs = rh_ds.Convert_ArcsToDS(rh_arcs);
                List<Autodesk.Revit.DB.NurbsCurve> ds_nurbscrvs = rh_ds.Convert_NurbsCurveToDS(rh_nurbscrvs);
                List<clsRhinoMultiSpanNurbs> ds_mspannurbs = rh_ds.Convert_MultiSpanNurbsCurveToDS(rh_mspannurbscrvs);
                List<Autodesk.Revit.DB.PolyCurve> ds_polylines = rh_ds.Convert_PolylineToDS(rh_polylines);
                List<Autodesk.Revit.DB.PolyCurve> ds_polycurves = rh_ds.Convert_PolyCurveToDS(rh_polycurves);
                List<Autodesk.Revit.DB.Circle> ds_circles = rh_ds.Convert_CirclesToDS(rh_circles);

                // add to master curve list
                for (int i = 0; i < ds_lines.Count; i++)
                {
                    ds_curves.Add(ds_lines[i]);
                    ds_ints.Add(rh_lnint[i]);
                }
                for (int i = 0; i < ds_arcs.Count; i++)
                {
                    ds_curves.Add(ds_arcs[i]);
                    ds_ints.Add(rh_arcint[i]);
                }
                for (int i = 0; i < ds_nurbscrvs.Count; i++)
                {
                    ds_curves.Add(ds_nurbscrvs[i]);
                    ds_ints.Add(rh_nurbint[i]);
                }
                for (int i = 0; i < ds_mspannurbs.Count; i++)
                {
                    Autodesk.Revit.DB.PolyCurve pc = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(ds_mspannurbs[i].MultiSpanNurbsList);
                    ds_curves.Add(pc);
                    ds_ints.Add(rh_mspanint[i]);
                }
                for (int i = 0; i < ds_polylines.Count; i++)
                {
                    ds_curves.Add(ds_polylines[i]);
                    ds_ints.Add(rh_plineint[i]);
                }
                for (int i = 0; i < ds_polycurves.Count; i++)
                {
                    ds_curves.Add(ds_polycurves[i]);
                    ds_ints.Add(rh_pcrvint[i]);
                }
                for (int i = 0; i < ds_circles.Count; i++)
                {
                    ds_curves.Add(ds_circles[i]);
                    ds_ints.Add(rh_circint[i]);
                }

                int[] indarr = ds_ints.ToArray();
                Autodesk.Revit.DB.Curve[] crvarr = ds_curves.ToArray();
                Array.Sort(indarr, crvarr);

                // return curve objects
                return crvarr.ToList();
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino Nurbs Surfaces to DesignScript Nurbs Surfaces.
        /// </summary>
        /// <param name="rh_nurbssurface">Rhino Nurbs Surfaces</param>
        /// <returns>DesignScript Nurbs Surfaces</returns>
        public List<Autodesk.Revit.DB.NurbsSurface> Convert_NurbsSurfaceToDS(List<Rhino.Geometry.NurbsSurface> rh_nurbssurface)
        {
            try
            {
                List<Autodesk.Revit.DB.NurbsSurface> ds_nurbssurfaces = new List<Autodesk.Revit.DB.NurbsSurface>();
                foreach (Rhino.Geometry.NurbsSurface srf in rh_nurbssurface)
                {
                    // surface properties
                    NurbsSurfacePointList points = srf.Points;
                    int dirU = points.CountU;
                    int dirV = points.CountV;
                    int degreeU = srf.Degree(0);
                    int degreeV = srf.Degree(1);

                    // knots
                    NurbsSurfaceKnotList knotsU = srf.KnotsU;
                    NurbsSurfaceKnotList knotsV = srf.KnotsV;

                    Autodesk.Revit.DB.Point[][] ds_pts = new Autodesk.Revit.DB.Point[dirU][];
                    double[][] ds_weights = new double[dirU][];
                    for (int u = 0; u < dirU; u++)
                    {
                        ds_pts[u] = new Autodesk.Revit.DB.Point[dirV];
                        ds_weights[u] = new double[dirV];
                        for (int v = 0; v < dirV; v++)
                        {
                            // point coordinates at u, v
                            Point3d pt = points.GetControlPoint(u, v).Location;
                            double x = pt.X;
                            double y = pt.Y;
                            double z = pt.Z;

                            ds_pts[u][v] = Autodesk.Revit.DB.Point.ByCoordinates(x, y, z);

                            // weights at u, v
                            ds_weights[u][v] = points.GetControlPoint(u, v).Weight;
                        }
                    }

                    // knots U
                    List<double> ds_knotsU = new List<double>();
                    ds_knotsU.Add(knotsU[0]);
                    for (int i = 0; i < knotsU.Count; i++)
                    {
                        ds_knotsU.Add(knotsU[i]);
                    }
                    ds_knotsU.Add(knotsU[knotsU.Count - 1]);

                    // knots V
                    List<double> ds_knotsV = new List<double>();
                    ds_knotsV.Add(knotsV[0]);
                    for (int i = 0; i < knotsV.Count; i++)
                    {
                        ds_knotsV.Add(knotsV[i]);
                    }
                    ds_knotsV.Add(knotsV[knotsV.Count - 1]);

                    bool success = false;
                    try
                    {
                        // create new nurbs surface
                        Autodesk.Revit.DB.NurbsSurface ds_nurbssurface = Autodesk.Revit.DB.NurbsSurface.ByControlPointsWeightsKnots(ds_pts, ds_weights, ds_knotsU.ToArray(), ds_knotsV.ToArray(), degreeU, degreeV);
                        ds_nurbssurfaces.Add(ds_nurbssurface);
                        success = true;
                    }
                    catch { success = false; }

                    if (success == false)
                    {
                        try
                        {
                            // create new nurbs surface

                            Autodesk.Revit.DB.NurbsSurface ds_nurbssurface = Autodesk.Revit.DB.NurbsSurface.ByControlPointsWeightsKnots(ds_pts, ds_weights, ds_knotsU.ToArray(), ds_knotsV.ToArray(), degreeU, degreeV + 1);
                            ds_nurbssurfaces.Add(ds_nurbssurface);

                            success = true;
                        }
                        catch { ds_nurbssurfaces.Add(null); }
                    }
                }
                return ds_nurbssurfaces;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino breps to DesignScript polysurfaces
        /// </summary>
        /// <param name="rh_breps">Rhino breps</param>
        /// <returns></returns>
        public List<Autodesk.Revit.DB.PolySurface> Convert_BrepsToDS(List<Rhino.Geometry.Brep> rh_breps)
        {
            try
            {
                List<Autodesk.Revit.DB.PolySurface> ds_polysurfaces = new List<Autodesk.Revit.DB.PolySurface>();
                foreach (Rhino.Geometry.Brep brep in rh_breps)
                {

                    List<Autodesk.Revit.DB.Surface> ds_faces = new List<Autodesk.Revit.DB.Surface>();

                    // get faces
                    foreach (BrepFace bf in brep.Faces)
                    {
                        List<Rhino.Geometry.NurbsSurface> m_nurbslist = new List<Rhino.Geometry.NurbsSurface>();
                        List<Rhynamo.classes.clsRhinoFace> m_rhinofacelist = new List<Rhynamo.classes.clsRhinoFace>();

                        bool m_issurface = false;
                        if (bf.IsSurface == true)
                        {
                            m_issurface = true;
                        }

                        Rhino.Geometry.NurbsSurface m_nurbsurface = bf.ToNurbsSurface();
                        m_nurbslist.Add(m_nurbsurface);

                        // get trimming loops
                        List<Rhino.Geometry.Curve> rh_loops = new List<Rhino.Geometry.Curve>();

                        //list of custom loops class
                        List<Rhynamo.classes.clsRhinoLoop> rh_clsloops = new List<Rhynamo.classes.clsRhinoLoop>();

                        if (bf.Loops.Count > 0)
                        {
                            foreach (BrepLoop bloop in bf.Loops)
                            {
                                // custom loops class
                                Rhynamo.classes.clsRhinoLoop rh_loopcls = new Rhynamo.classes.clsRhinoLoop(m_issurface);

                                List<Rhino.Geometry.Curve> m_trims = new List<Rhino.Geometry.Curve>();
                                foreach (BrepTrim t in bloop.Trims)
                                {
                                    if (t.TrimType == BrepTrimType.Boundary || t.TrimType == BrepTrimType.Mated) // ignore "seams"
                                    {
                                        Rhino.Geometry.Curve m_edgecurve = t.Edge.DuplicateCurve();
                                        rh_loopcls.LoopCurves.Add(m_edgecurve.ToNurbsCurve());
                                        rh_loops.Add(m_edgecurve.ToNurbsCurve());
                                    }
                                }

                                //list of loops
                                rh_clsloops.Add(rh_loopcls);
                                Rhynamo.classes.clsRhinoFace m_rhinoface = new Rhynamo.classes.clsRhinoFace(m_nurbsurface, rh_loopcls, m_issurface);
                                m_rhinofacelist.Add(m_rhinoface);
                            }
                        }

                        // Convert surface
                        List<Autodesk.Revit.DB.NurbsSurface> ds_nurbs = Convert_NurbsSurfaceToDS(m_nurbslist);

                        // Convert curve loops
                        List<Autodesk.Revit.DB.Curve> ds_loops = new List<Autodesk.Revit.DB.Curve>();

                        try
                        {
                            if (rh_clsloops.Count > 0)
                            {
                                foreach (Rhynamo.classes.clsRhinoLoop lp in rh_clsloops)
                                {
                                    // convert curves and clean up
                                    List<Autodesk.Revit.DB.Curve> ds_curves = Convert_CurvesToDS(lp.LoopCurves);
                                    //ds_curves = RemoveDuplicateCurves(ds_curves);
                                    ds_curves = RemoveSmallSegments(ds_curves, 0.001);

                                    List<Autodesk.Revit.DB.Curve> ds_joinedcrvs = new List<Autodesk.Revit.DB.Curve>();

                                    // test 1
                                    bool test = false;
                                    try
                                    {
                                        Autodesk.Revit.DB.Curve pcrv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(ds_curves);
                                        ds_joinedcrvs.Add(pcrv);
                                        test = true;
                                    }
                                    catch { test = false; }

                                    if (test == false)
                                    {
                                        ds_joinedcrvs = JoinLoopCurves(ds_curves);
                                    }

                                    foreach (Autodesk.Revit.DB.Curve c in ds_joinedcrvs)
                                    {
                                        ds_loops.Add(c);
                                    }
                                }
                            }
                        }
                        catch { }

                        // determine if polycurves
                        List<Autodesk.Revit.DB.PolyCurve> ds_polycurves = new List<Autodesk.Revit.DB.PolyCurve>();
                        try
                        {
                            foreach (Autodesk.Revit.DB.Curve c in ds_loops)
                            {
                                if (c.IsClosed == true)
                                {
                                    // cast as circle
                                    Autodesk.Revit.DB.Circle circ = c as Autodesk.Revit.DB.Circle;
                                    if (circ != null)
                                    {
                                        // split into segments
                                        List<double> parms = new List<double>();
                                        parms.Add(0.25);
                                        parms.Add(0.75);
                                        Autodesk.Revit.DB.Curve[] crvs = circ.SplitByParameter(parms.ToArray());

                                        // join as a polycurve
                                        Autodesk.Revit.DB.PolyCurve pcrv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(crvs);
                                        ds_polycurves.Add(pcrv);
                                    }

                                    // cast as circle
                                    Autodesk.Revit.DB.NurbsCurve nrbcrv = c as Autodesk.Revit.DB.NurbsCurve;
                                    if (nrbcrv != null)
                                    {
                                        // split into segments
                                        List<double> parms = new List<double>();
                                        parms.Add(0.25);
                                        parms.Add(0.75);
                                        Autodesk.Revit.DB.Curve[] crvs = nrbcrv.SplitByParameter(parms.ToArray());

                                        // join as a polycurve
                                        Autodesk.Revit.DB.PolyCurve pcrv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(crvs);
                                        ds_polycurves.Add(pcrv);
                                    }
                                    if (c is Autodesk.Revit.DB.PolyCurve)
                                    {
                                        ds_polycurves.Add((Autodesk.Revit.DB.PolyCurve)c);
                                    }

                                }
                            }
                        }
                        catch { }

                        // create designscript face
                        try
                        {
                            if (ds_polycurves.Count > 0 && ds_nurbs.Count > 0)
                            {
                                try
                                {
                                    Autodesk.Revit.DB.Surface ds_face = ds_nurbs[0].TrimWithEdgeLoops(ds_polycurves);
                                    ds_faces.Add(ds_face);
                                }
                                catch
                                {
                                    Autodesk.Revit.DB.Surface ds_face = ds_nurbs[0];
                                    ds_faces.Add(ds_face);
                                }

                            }
                            else if (ds_polycurves.Count == 0 && ds_nurbs.Count > 0)
                            {
                                Autodesk.Revit.DB.Surface ds_face = ds_nurbs[0];
                                ds_faces.Add(ds_face);
                            }
                        }
                        catch { }

                    }

                    // create designscript polysurface
                    try
                    {
                        if (ds_faces.Count > 0)
                        {
                            Autodesk.Revit.DB.PolySurface ds_polysurface = PolySurface.ByJoinedSurfaces(ds_faces);
                            ds_polysurfaces.Add(ds_polysurface);
                        }
                        else
                        {
                            ds_polysurfaces.Add(null);
                        }
                    }
                    catch { ds_polysurfaces.Add(null); }
                }

                return ds_polysurfaces;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts Rhino Extrusions to DesignScript polysurfaces
        /// </summary>
        /// <param name="rh_extrusions">Rhino extrusions</param>
        /// <returns></returns>
        public List<Autodesk.Revit.DB.PolySurface> Convert_ExtrusionToDS(List<Rhino.Geometry.Extrusion> rh_extrusions)
        {
            try
            {
                List<Autodesk.Revit.DB.PolySurface> ds_polysurfaces = new List<Autodesk.Revit.DB.PolySurface>();
                foreach (Rhino.Geometry.Extrusion ext in rh_extrusions)
                {
                    // convert to brep
                    Brep brep = ext.ToBrep();

                    List<Autodesk.Revit.DB.Surface> ds_faces = new List<Autodesk.Revit.DB.Surface>();

                    // get faces
                    foreach (BrepFace bf in brep.Faces)
                    {
                        List<Rhino.Geometry.NurbsSurface> m_nurbslist = new List<Rhino.Geometry.NurbsSurface>();
                        Rhino.Geometry.NurbsSurface nurbs = bf.ToNurbsSurface();
                        m_nurbslist.Add(nurbs);

                        // get trimming loops
                        List<Rhino.Geometry.Curve> rh_loops = new List<Rhino.Geometry.Curve>();
                        if (bf.Loops.Count > 0)
                        {
                            foreach (BrepLoop bloop in bf.Loops)
                            {
                                Rhino.Geometry.Curve loopcurve = bloop.To3dCurve();
                                Rhino.Geometry.Curve pcurve = (Rhino.Geometry.Curve)bloop.To3dCurve();
                                if (pcurve != null)
                                {
                                    rh_loops.Add(pcurve);
                                }
                            }
                        }

                        // convert surface and loops
                        List<Autodesk.Revit.DB.NurbsSurface> ds_nurbs = Convert_NurbsSurfaceToDS(m_nurbslist);
                        List<Autodesk.Revit.DB.Curve> ds_curves = Convert_CurvesToDS(rh_loops);

                        // determine if polycurves
                        List<Autodesk.Revit.DB.PolyCurve> ds_polycurves = new List<Autodesk.Revit.DB.PolyCurve>();
                        try
                        {
                            foreach (Autodesk.Revit.DB.Curve c in ds_curves)
                            {
                                if (c.IsClosed == true)
                                {
                                    // cast as circle
                                    Autodesk.Revit.DB.Circle circ = c as Autodesk.Revit.DB.Circle;
                                    if (circ != null)
                                    {
                                        // split into segments
                                        List<double> parms = new List<double>();
                                        parms.Add(0.25);
                                        parms.Add(0.75);
                                        Autodesk.Revit.DB.Curve[] crvs = circ.SplitByParameter(parms.ToArray());

                                        // join as a polycurve
                                        Autodesk.Revit.DB.PolyCurve pcrv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(crvs);
                                        ds_polycurves.Add(pcrv);
                                    }

                                    // cast as circle
                                    Autodesk.Revit.DB.NurbsCurve nrbcrv = c as Autodesk.Revit.DB.NurbsCurve;
                                    if (nrbcrv != null)
                                    {
                                        // split into segments
                                        List<double> parms = new List<double>();
                                        parms.Add(0.25);
                                        parms.Add(0.75);
                                        Autodesk.Revit.DB.Curve[] crvs = nrbcrv.SplitByParameter(parms.ToArray());

                                        // join as a polycurve
                                        Autodesk.Revit.DB.PolyCurve pcrv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(crvs);
                                        ds_polycurves.Add(pcrv);
                                    }
                                    if (c is Autodesk.Revit.DB.PolyCurve)
                                    {
                                        ds_polycurves.Add((Autodesk.Revit.DB.PolyCurve)c);
                                    }

                                }
                            }
                        }
                        catch { }


                        // create designscript face
                        try
                        {
                            if (ds_polycurves.Count > 0 && ds_nurbs.Count > 0)
                            {
                                Autodesk.Revit.DB.Surface ds_face = ds_nurbs[0].TrimWithEdgeLoops(ds_polycurves);
                                ds_faces.Add(ds_face);
                            }
                            else if (ds_polycurves.Count == 0 && ds_nurbs.Count > 0)
                            {
                                Autodesk.Revit.DB.Surface ds_face = ds_nurbs[0];
                                ds_faces.Add(ds_face);
                            }
                        }
                        catch { }

                    }

                    // create designscript polysurface
                    try
                    {
                        if (ds_faces.Count > 0)
                        {
                            Autodesk.Revit.DB.PolySurface ds_polysurface = PolySurface.ByJoinedSurfaces(ds_faces);
                            ds_polysurfaces.Add(ds_polysurface);
                        }
                    }
                    catch { ds_polysurfaces.Add(null); }

                }

                return ds_polysurfaces;
            }
            catch { return null; }
        }

        /// <summary>
        /// Convert a Rhino mesh to a DesignScript mesh
        /// </summary>
        /// <param name="rh_meshes">Rhino meshes</param>
        /// <returns>DesignScript meshes</returns>
        public List<Autodesk.Revit.DB.Mesh> Convert_MeshToDS(List<Rhino.Geometry.Mesh> rh_meshes)
        {
            try
            {
                List<Autodesk.Revit.DB.Mesh> ds_meshes = new List<Autodesk.Revit.DB.Mesh>();
                foreach (Rhino.Geometry.Mesh msh in rh_meshes)
                {
                    MeshVertexList mvl = msh.Vertices;
                    MeshFaceList mfl = msh.Faces;

                    List<Autodesk.Revit.DB.Point> ds_verts = new List<Autodesk.Revit.DB.Point>();
                    for (int i = 0; i < mvl.Count; i++)
                    {
                        double x = mvl[i].X;
                        double y = mvl[i].Y;
                        double z = mvl[i].Z;

                        Autodesk.Revit.DB.Point pt = Autodesk.Revit.DB.Point.ByCoordinates(x, y, z);
                        ds_verts.Add(pt);
                    }

                    List<Autodesk.Revit.DB.IndexGroup> ds_faces = new List<Autodesk.Revit.DB.IndexGroup>();
                    for (int i = 0; i < mfl.Count; i++)
                    {
                        MeshFace mf = mfl[i];
                        if (mf.IsTriangle)
                        {
                            uint A = (uint)mf.A;
                            uint B = (uint)mf.B;
                            uint C = (uint)mf.C;

                            IndexGroup ig = IndexGroup.ByIndices(A, B, C);
                            ds_faces.Add(ig);
                        }
                        else if (mf.IsQuad)
                        {
                            uint A = (uint)mf.A;
                            uint B = (uint)mf.B;
                            uint C = (uint)mf.C;
                            uint D = (uint)mf.D;

                            IndexGroup ig = IndexGroup.ByIndices(A, B, C, D);
                            ds_faces.Add(ig);
                        }
                    }

                    try
                    {
                        Autodesk.Revit.DB.Mesh ds_mesh = Autodesk.Revit.DB.Mesh.ByPointsFaceIndices(ds_verts, ds_faces);
                        ds_meshes.Add(ds_mesh);
                    }
                    catch { ds_meshes.Add(null); }

                }
                return ds_meshes;
            }
            catch { return null; }
        }
        #endregion

        #region "DesignScript to Rhino"
        /// <summary>
        /// Converts DesignScript points to Rhino points
        /// </summary>
        /// <param name="pt">DesignScript points</param>
        /// <returns>Rhino points</returns>
        public Rhino.Geometry.Point Convert_PointsTo3dm(Autodesk.Revit.DB.Point pt)
        {
            try
            {
                // get x, y, z coordinates
                double x = Math.Round(pt.X, 3);
                double y = Math.Round(pt.Y, 3);
                double z = Math.Round(pt.Z, 3);

                // create Rhino point
                Rhino.Geometry.Point3d rh_pt3d = new Rhino.Geometry.Point3d(x, y, z);
                Rhino.Geometry.Point rh_pt = new Rhino.Geometry.Point(rh_pt3d);

                return rh_pt;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript curves to Rhino curves
        /// </summary>
        /// <param name="crv">DesignScript curves</param>
        /// <returns>Rhino curves</returns>
        public Rhino.Geometry.Curve Convert_CurvesTo3dm(Autodesk.Revit.DB.Curve crv)
        {
            try
            {
                // master list of Rhino curves
                Rhino.Geometry.Curve rh_curve = null;

                // is line?
                Autodesk.Revit.DB.Line lntest = null;
                try
                {
                    lntest = crv as Autodesk.Revit.DB.Line;
                }
                catch { }
                // is arc?
                Autodesk.Revit.DB.Arc arctest = null;
                try
                {
                    arctest = crv as Autodesk.Revit.DB.Arc;
                }
                catch { }
                // is NurbsCurve?
                Autodesk.Revit.DB.NurbsCurve nurbstest = null;
                try
                {
                    nurbstest = crv as Autodesk.Revit.DB.NurbsCurve;
                }
                catch { }
                // is PolyCurve?
                Autodesk.Revit.DB.PolyCurve pcrvtest = null;
                try
                {
                    pcrvtest = crv as Autodesk.Revit.DB.PolyCurve;
                }
                catch { }

                bool success = false;
                if (crv is Autodesk.Revit.DB.Line || lntest != null)
                {
                    // Rhino line curve
                    Rhino.Geometry.LineCurve rh_lncrv = Convert_LineTo3dm(crv as Autodesk.Revit.DB.Line);

                    // set converted curve
                    rh_curve = rh_lncrv;

                    success = true;
                }
                else if (crv is Autodesk.Revit.DB.Arc || arctest != null)
                {
                    // rhino arc curve
                    Rhino.Geometry.ArcCurve rh_arccrv = Convert_ArcTo3dm(crv as Autodesk.Revit.DB.Arc);

                    // set converted curve
                    rh_curve = rh_arccrv;

                    success = true;
                }
                else if (crv is Autodesk.Revit.DB.NurbsCurve || nurbstest != null)
                {
                    // rhino nurbs curve
                    Rhino.Geometry.NurbsCurve rh_nurbscrv = Convert_NurbsCurveTo3dm(crv as Autodesk.Revit.DB.NurbsCurve);

                    // set converted curve
                    rh_curve = rh_nurbscrv;

                    success = true;
                }
                else if (crv is Autodesk.Revit.DB.PolyCurve || pcrvtest != null)
                {
                    if (crv is Autodesk.Revit.DB.Rectangle)
                    {
                        // rhino nurbs curve
                        Rhino.Geometry.Rectangle3d rh_rect = Convert_RectangleTo3dm(crv as Autodesk.Revit.DB.Rectangle);

                        // set converted curve
                        rh_curve = rh_rect.ToNurbsCurve();

                        success = true;
                    }
                    else
                    {
                        // rhino nurbs curve
                        Rhino.Geometry.PolyCurve rh_polycrv = Convert_PolyCurveTo3dm(crv as Autodesk.Revit.DB.PolyCurve);

                        // set converted curve
                        rh_curve = rh_polycrv;

                        success = true;
                    }

                }

                // last ditch effort
                if (success == false)
                {
                    Autodesk.Revit.DB.NurbsCurve ds_nurbs = crv.ToNurbsCurve();

                    // rhino nurbs curve
                    Rhino.Geometry.NurbsCurve rh_nurbscrv = Convert_NurbsCurveTo3dm(ds_nurbs);

                    // set converted curve
                    rh_curve = rh_nurbscrv;

                }

                return rh_curve;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript surfaces to Rhino surfaces
        /// </summary>
        /// <param name="srf">DesignScript surface</param>
        /// <returns>Rhino surface</returns>
        public Rhino.Geometry.NurbsSurface Convert_SurfacesTo3dm(Autodesk.Revit.DB.NurbsSurface srf)
        {
            try
            {
                Rhino.Geometry.NurbsSurface rh_surfaces = null;

                // define the nurbs surface
                Rhino.Geometry.NurbsSurface rh_srf = Rhino.Geometry.NurbsSurface.Create(3, false, srf.DegreeU + 1, srf.DegreeV + 1, srf.NumControlPointsU, srf.NumControlPointsV);

                // add the knots
                double[] m_knotsU = srf.UKnots();
                double[] m_knotsV = srf.VKnots();
                for (int i = 1; i < m_knotsU.Length - 1; i++)
                {
                    rh_srf.KnotsU[i - 1] = m_knotsU[i];
                }
                for (int j = 1; j < m_knotsV.Length - 1; j++)
                {
                    rh_srf.KnotsV[j - 1] = m_knotsV[j];
                }

                // add the control points
                Autodesk.Revit.DB.Point[][] m_ctrlpts = srf.ControlPoints();
                int numU = srf.NumControlPointsU;
                int numV = srf.NumControlPointsV;

                for (int i = 0; i < numU; i++)
                {
                    for (int j = 0; j < numV; j++)
                    {
                        Autodesk.Revit.DB.Point ctrlpt = m_ctrlpts[i][j];

                        double x = ctrlpt.X;
                        double y = ctrlpt.Y;
                        double z = ctrlpt.Z;
                        Point3d pt = new Point3d(x, y, z);

                        // create the control point
                        ControlPoint rh_ctrlpt = new ControlPoint(pt);
                        rh_srf.Points.SetControlPoint(i, j, rh_ctrlpt);
                    }
                }

                // set domains
                Interval m_uint = new Interval(0, 1);
                Interval m_vint = new Interval(0, 1);
                rh_srf.SetDomain(0, m_uint);
                rh_srf.SetDomain(1, m_vint);

                rh_surfaces = rh_srf;
                return rh_surfaces;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript PolySurfaces to Rhino breps
        /// </summary>
        /// <param name="ds_polysurface"></param>
        /// <returns></returns>
        public Rhino.Geometry.Brep Convert_PolySurfaceTo3dm(Autodesk.Revit.DB.PolySurface ds_polysurface)
        {
            try
            {
                // DesignSript PolySurface Components
                Autodesk.Revit.DB.Surface[] ds_surfaces = ds_polysurface.Surfaces();

                // Rhino Brep
                Rhino.Geometry.Brep rh_brep = new Rhino.Geometry.Brep();

                foreach (Autodesk.Revit.DB.Surface surf in ds_surfaces)
                {
                    // get edge geometry
                    Autodesk.Revit.DB.Curve[] ds_3dedges = surf.PerimeterCurves();
                    Autodesk.Revit.DB.Curve[] ds_2dedges = Make2DParameterCurves(surf.ToNurbsSurface(), ds_3dedges);

                    // get edge geometry
                    List<Rhino.Geometry.Curve> rh_3dcrvs = new List<Rhino.Geometry.Curve>();
                    foreach (Autodesk.Revit.DB.Curve crv in ds_3dedges)
                    {
                        // convert DesignScript edge loops to 3DM
                        Rhino.Geometry.Curve rh_crv = Convert_CurvesTo3dm(crv.ToNurbsCurve());
                        rh_3dcrvs.Add(rh_crv);
                    }

                    // get trim geometry
                    List<Rhino.Geometry.Curve> rh_2dcrvs = new List<Rhino.Geometry.Curve>();
                    foreach (Autodesk.Revit.DB.Curve crv in ds_2dedges)
                    {
                        // convert DesignScript trim loops to 3DM
                        Rhino.Geometry.Curve rh_crv = Convert_CurvesTo3dm(crv.ToNurbsCurve());
                        rh_2dcrvs.Add(rh_crv);
                    }

                    // get nurbs surface geometry
                    Autodesk.Revit.DB.NurbsSurface ds_nurbssurface = surf.ToNurbsSurface();
                    Rhino.Geometry.NurbsSurface rh_nurbssurface = Convert_SurfacesTo3dm(ds_nurbssurface);

                    // face
                    Rhino.Geometry.Brep rh_face = new Rhino.Geometry.Brep();

                    // add surface
                    rh_face.AddSurface(rh_nurbssurface);

                    // join perimeter curves into polycurve loops
                    List<Rhino.Geometry.PolyCurve> rh_3dloops = JoinPerimeterCurves(rh_3dcrvs);
                    List<Rhino.Geometry.PolyCurve> rh_2dloops = JoinPerimeterCurves(rh_2dcrvs);

                    // sort all edge curves based on inner/outer loops
                    clsRhinoBrepUtils m_sortededges = new clsRhinoBrepUtils(rh_2dloops, rh_3dloops, rh_nurbssurface);
                    rh_2dcrvs = m_sortededges.Curve2DSegments;
                    rh_3dcrvs = m_sortededges.Curve3DSegments;
                    rh_2dloops = m_sortededges.Curve2DLoops;
                    rh_3dloops = m_sortededges.Curve3DLoops;

                    // add vertices to brep face
                    try
                    {
                        foreach (Rhino.Geometry.Curve c in rh_3dcrvs)
                        {
                            Point3d pt = c.PointAtStart;
                            rh_face.Vertices.Add(pt, 0.0);
                        }
                    }
                    catch { }

                    // add edges to brep face
                    try
                    {
                        int m_segindex = 0;
                        foreach (Rhino.Geometry.PolyCurve pc in rh_3dloops)
                        {
                            int start = m_segindex;
                            int end;
                            for (int i = 0; i < pc.SegmentCount; i++)
                            {
                                if (i + 1 > pc.SegmentCount - 1)
                                {
                                    start = m_segindex;
                                    end = m_segindex - (pc.SegmentCount - 1);
                                }
                                else
                                {
                                    start = m_segindex;
                                    end = m_segindex + 1;
                                }

                                // add curves
                                rh_face.AddEdgeCurve(rh_3dcrvs[m_segindex]);

                                // add edge
                                rh_face.Edges.Add(start, end, m_segindex, 0.0);

                                m_segindex++;
                            }
                        }
                    }
                    catch { }

                    //add trim curves to brep face
                    try
                    {
                        foreach (Rhino.Geometry.Curve c in rh_2dcrvs)
                        {
                            // add curve
                            rh_face.AddTrimCurve(c);
                        }
                    }
                    catch { }

                    // add the face
                    rh_face.Faces.Add(0);

                    // add trims & loops
                    try
                    {
                        int m_segindex = 0;
                        int m_loopindex = 0;
                        foreach (Rhino.Geometry.PolyCurve pc in rh_3dloops)
                        {
                            BrepLoop m_loop = null;
                            if (m_loopindex == rh_3dloops.Count - 1)
                            {
                                m_loop = rh_face.Loops.Add(BrepLoopType.Outer, rh_face.Faces[0]);
                            }
                            else
                            {
                                m_loop = rh_face.Loops.Add(BrepLoopType.Inner, rh_face.Faces[0]);
                            }

                            for (int i = 0; i < pc.SegmentCount; i++)
                            {
                                BrepEdge m_edge = rh_face.Edges[m_segindex];
                                bool rev3d = false;

                                // make new trim
                                BrepTrim m_trim = rh_face.Trims.Add(m_edge, rev3d, m_loop, m_segindex);
                                m_trim.TrimType = BrepTrimType.Boundary;
                                m_trim.SetTolerances(0.0, 0.0);

                                m_segindex++;
                            }
                            m_loopindex++;
                        }
                    }
                    catch { }

                    // add face to brep
                    rh_brep.Append(rh_face);
                }

                // Rhino Brep
                if (rh_brep.Surfaces.Count > 0 || rh_brep.Faces.Count > 0)
                {
                    return rh_brep;
                }
                else
                {
                    return null;
                }

            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript meshes to Rhino meshes
        /// </summary>
        /// <param name="ds_mesh">DesignScript mesh</param>
        /// <returns>Rhino mesh</returns>
        public Rhino.Geometry.Mesh Convert_MeshTo3dm(Autodesk.Revit.DB.Mesh ds_mesh)
        {
            try
            {
                // DesignScript mesh components
                Autodesk.Revit.DB.Point[] ds_verts = ds_mesh.VertexPositions;
                IndexGroup[] ds_faces = ds_mesh.FaceIndices;

                // Rhino Mesh
                Rhino.Geometry.Mesh rh_mesh = new Rhino.Geometry.Mesh();

                // Populate vertices
                foreach (Autodesk.Revit.DB.Point pt in ds_verts)
                {
                    double x = pt.X;
                    double y = pt.Y;
                    double z = pt.Z;
                    rh_mesh.Vertices.Add(new Point3d(x, y, z));
                }

                // Populate faces
                foreach (Autodesk.Revit.DB.IndexGroup ig in ds_faces)
                {
                    // 3-point face (Triangle)
                    if (ig.Count == 3)
                    {
                        int A = (int)ig.A;
                        int B = (int)ig.B;
                        int C = (int)ig.C;
                        rh_mesh.Faces.AddFace(A, B, C);
                    }
                    // 4-point face (Quad)
                    if (ig.Count == 4)
                    {
                        int A = (int)ig.A;
                        int B = (int)ig.B;
                        int C = (int)ig.C;
                        int D = (int)ig.D;
                        rh_mesh.Faces.AddFace(A, B, C, D);
                    }
                }
                return rh_mesh;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript Line curves to Rhino Line curves
        /// </summary>
        /// <param name="ds_ln">DesignScript Line</param>
        /// <returns>Rhino Line Curve</returns>
        private Rhino.Geometry.LineCurve Convert_LineTo3dm(Autodesk.Revit.DB.Line ds_ln)
        {
            try
            {
                // point 1
                double x1 = Math.Round(ds_ln.StartPoint.X, 3);
                double y1 = Math.Round(ds_ln.StartPoint.Y, 3);
                double z1 = Math.Round(ds_ln.StartPoint.Z, 3);
                Point3d pt1 = new Point3d(x1, y1, z1);

                // point 2
                double x2 = Math.Round(ds_ln.EndPoint.X, 3);
                double y2 = Math.Round(ds_ln.EndPoint.Y, 3);
                double z2 = Math.Round(ds_ln.EndPoint.Z, 3);
                Point3d pt2 = new Point3d(x2, y2, z2);

                // rhino line
                Rhino.Geometry.Line rh_ln = new Rhino.Geometry.Line(pt1, pt2);
                Rhino.Geometry.LineCurve rh_lncrv = new Rhino.Geometry.LineCurve(rh_ln);

                return rh_lncrv;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript Arc curves to Rhino Arc curves
        /// </summary>
        /// <param name="ds_arc">DesignScript Arc</param>
        /// <returns>Rhino Arc curve</returns>
        private Rhino.Geometry.ArcCurve Convert_ArcTo3dm(Autodesk.Revit.DB.Arc ds_arc)
        {
            try
            {
                // point 1
                double x1 = Math.Round(ds_arc.StartPoint.X, 3);
                double y1 = Math.Round(ds_arc.StartPoint.Y, 3);
                double z1 = Math.Round(ds_arc.StartPoint.Z, 3);
                Point3d startpt = new Point3d(x1, y1, z1);

                // point 2
                double x2 = Math.Round(ds_arc.EndPoint.X, 3);
                double y2 = Math.Round(ds_arc.EndPoint.Y, 3);
                double z2 = Math.Round(ds_arc.EndPoint.Z, 3);
                Point3d endpt = new Point3d(x2, y2, z2);

                // middle point
                double x3 = Math.Round(ds_arc.PointAtParameter(0.5).X, 3);
                double y3 = Math.Round(ds_arc.PointAtParameter(0.5).Y, 3);
                double z3 = Math.Round(ds_arc.PointAtParameter(0.5).Z, 3);
                Point3d midpt = new Point3d(x3, y3, z3);

                // rhino line
                Rhino.Geometry.Arc rh_arc = new Rhino.Geometry.Arc(startpt, midpt, endpt);
                Rhino.Geometry.ArcCurve rh_arccrv = new Rhino.Geometry.ArcCurve(rh_arc);

                return rh_arccrv;
            }
            catch { return null; }
        }

        /// <summary>
        /// Converts DesignScript Nurbs curves to Rhino Nurbs curves
        /// </summary>
        /// <param name="ds_nurbs">DesignScript NurbsCurve</param>
        /// <returns>Rhino Nurbs curve</returns>
        private Rhino.Geometry.NurbsCurve Convert_NurbsCurveTo3dm(Autodesk.Revit.DB.NurbsCurve ds_nurbs)
        {
            try
            {
                // get points & knots
                Autodesk.Revit.DB.Point[] nurbspts = ds_nurbs.ControlPoints();
                double[] nurbsweights = ds_nurbs.Weights();
                double[] nurbsknots = ds_nurbs.Knots();
                int nurbsdegree = ds_nurbs.Degree;

                // list for weights
                List<Rhino.Geometry.Point3d> rh_pts = new List<Rhino.Geometry.Point3d>();

                // points
                for (int i = 0; i < nurbspts.Length; i++)
                {
                    Autodesk.Revit.DB.Point nurbspt = nurbspts[i];
                    double x = Math.Round(nurbspt.X, 3);
                    double y = Math.Round(nurbspt.Y, 3);
                    double z = Math.Round(nurbspt.Z, 3);

                    Rhino.Geometry.Point3d rh_pt = new Rhino.Geometry.Point3d(x, y, z);
                    rh_pts.Add(rh_pt);
                }

                // create new nurbs curve
                Rhino.Geometry.NurbsCurve rh_nurbscrv = Rhino.Geometry.NurbsCurve.Create(false, nurbsdegree, rh_pts);

                // add knots
                for (int i = 1; i < nurbsknots.Length - 1; i++)
                {
                    rh_nurbscrv.Knots[i - 1] = nurbsknots[i];
                }

                // add weights
                //for (int i = 0; i < nurbsweights.Length; i++)
                //{
                //  NurbsCurvePointList cpts = rh_nurbscrv.Points;
                //  cpts[i] = nurbsweights[i];

                //}

                return rh_nurbscrv;
            }
            catch { return null; }
        }

        /// <summary>
        /// Convert DesignScript Rectangles to Rhino Rectangle3D
        /// </summary>
        /// <param name="ds_rect"></param>
        /// <returns></returns>
        private Rhino.Geometry.Rectangle3d Convert_RectangleTo3dm(Autodesk.Revit.DB.Rectangle ds_rect)
        {
            try
            {
                //plane def
                Rhino.Geometry.Point3d m_origin = new Point3d(ds_rect.BasePlane().Origin.X, ds_rect.BasePlane().Origin.Y, ds_rect.BasePlane().Origin.Z);
                Rhino.Geometry.Vector3d m_xaxis = new Vector3d(ds_rect.BasePlane().XAxis.X, ds_rect.BasePlane().XAxis.Y, ds_rect.BasePlane().XAxis.Z);
                Rhino.Geometry.Vector3d m_yaxis = new Vector3d(ds_rect.BasePlane().YAxis.X, ds_rect.BasePlane().YAxis.Y, ds_rect.BasePlane().YAxis.Z);
                Rhino.Geometry.Plane m_plane = new Rhino.Geometry.Plane(m_origin, m_xaxis, m_yaxis);

                Rhino.Geometry.Rectangle3d rh_rect = new Rhino.Geometry.Rectangle3d(m_plane, ds_rect.Width, ds_rect.Height);

                return rh_rect;
            }
            catch { return new Rhino.Geometry.Rectangle3d(); }
        }

        /// <summary>
        /// Converts DesignScript polycurves to Rhino polycurves
        /// </summary>
        /// <param name="ds_polycrv"></param>
        /// <returns></returns>
        private Rhino.Geometry.PolyCurve Convert_PolyCurveTo3dm(Autodesk.Revit.DB.PolyCurve ds_polycrv)
        {
            try
            {
                Rhino.Geometry.PolyCurve rh_polycrv = new Rhino.Geometry.PolyCurve();

                foreach (Autodesk.Revit.DB.Curve crv in ds_polycrv.Curves())
                {

                    Autodesk.Revit.DB.NurbsCurve ds_nurbs = crv.ToNurbsCurve();
                    Rhino.Geometry.NurbsCurve rh_nurbs = Convert_NurbsCurveTo3dm(ds_nurbs);
                    rh_polycrv.Append(rh_nurbs);

                    // something is busted with DesignScript... can't perform tests to see if a curve is a line, arc, etc.
                    //////// is line?
                    //////Autodesk.Revit.DB.Line lntest = null;

                    //////try
                    //////{
                    //////  lntest = crv as Autodesk.Revit.DB.Line;
                    //////}
                    //////catch { }
                    //////if (crv is Autodesk.Revit.DB.Line || lntest != null)
                    //////{
                    //////  // Rhino line curve
                    //////  Rhino.Geometry.LineCurve rh_lncrv = Convert_LineTo3dm(crv as Autodesk.Revit.DB.Line);

                    //////  // add to rhino curves list
                    //////  rh_polycrv.Append(rh_lncrv);
                    //////}

                    //////// is arc?
                    //////Autodesk.Revit.DB.Arc arctest = null;
                    //////try
                    //////{
                    //////  arctest = crv as Autodesk.Revit.DB.Arc;
                    //////}
                    //////catch { }
                    //////if (crv is Autodesk.Revit.DB.Arc || arctest != null)
                    //////{
                    //////  // rhino arc curve
                    //////  Rhino.Geometry.ArcCurve rh_arccrv = Convert_ArcTo3dm(crv as Autodesk.Revit.DB.Arc);

                    //////  // add to rhino curves list
                    //////  rh_polycrv.Append(rh_arccrv);
                    //////}

                    //////// is NurbsCurve?
                    //////// is arc?
                    //////Autodesk.Revit.DB.NurbsCurve nurbstest = null;
                    //////try
                    //////{
                    //////  nurbstest = crv as Autodesk.Revit.DB.NurbsCurve;
                    //////}
                    //////catch { }
                    //////if (crv is Autodesk.Revit.DB.NurbsCurve || nurbstest != null)
                    //////{
                    //////  // rhino nurbs curve
                    //////  Rhino.Geometry.NurbsCurve rh_nurbscrv = Convert_NurbsCurveTo3dm(crv as Autodesk.Revit.DB.NurbsCurve);

                    //////  // add to rhino curves list
                    //////  rh_polycrv.Append(rh_nurbscrv);
                    //////}
                }
                return rh_polycrv;
            }
            catch { return null; }
        }
        #endregion

        #region "Private Members"
        /// <summary>
        /// Takes 3D edge curves and defines 2D parameter space curves relative to a surface
        /// </summary>
        /// <param name="srf">base surface</param>
        /// <param name="ds_3dcurves">3D curves</param>
        /// <returns>2d parameter curves</returns>
        private Autodesk.Revit.DB.Curve[] Make2DParameterCurves(Autodesk.Revit.DB.NurbsSurface srf, Autodesk.Revit.DB.Curve[] ds_3dcurves)
        {

            Rhino.Geometry.NurbsSurface rh_srf = Convert_SurfacesTo3dm(srf);
            Interval rh_Uinterval = new Interval(rh_srf.Domain(0).Min, rh_srf.Domain(0).Max);
            Interval rh_Vinterval = new Interval(rh_srf.Domain(1).Min, rh_srf.Domain(1).Max);

            List<Autodesk.Revit.DB.Curve> ds_2dcurves = new List<Autodesk.Revit.DB.Curve>();

            foreach (Autodesk.Revit.DB.Curve crv in ds_3dcurves)
            {
                Autodesk.Revit.DB.NurbsCurve ds_nurbscrv = crv.ToNurbsCurve();

                // knots and degree
                double[] ds_knots = ds_nurbscrv.Knots();
                List<double> ds_weights = new List<double>();
                int nurbsdegree = ds_nurbscrv.Degree;

                // 2d uv points
                List<Autodesk.Revit.DB.Point> ds_2dpts = new List<Autodesk.Revit.DB.Point>();

                // control points
                Autodesk.Revit.DB.Point[] ds_points = ds_nurbscrv.ControlPoints();
                foreach (Autodesk.Revit.DB.Point pt in ds_points)
                {
                    // find UV point on surface
                    Autodesk.Revit.DB.Point ds_srfpt = srf.ClosestPointTo(pt);
                    Autodesk.Revit.DB.UV ds_uv = srf.UVParameterAtPoint(ds_srfpt);

                    // 2d points
                    Autodesk.Revit.DB.Point ds_2dpt = Autodesk.Revit.DB.Point.ByCoordinates(rh_Uinterval.Max * ds_uv.U, rh_Vinterval.Max * ds_uv.V, 0);
                    ds_2dpts.Add(ds_2dpt);
                }

                // create new nurbs curve
                Autodesk.Revit.DB.NurbsCurve ds_2dnurbscrv = Autodesk.Revit.DB.NurbsCurve.ByControlPointsWeightsKnots(ds_2dpts, ds_weights.ToArray(), ds_knots.ToArray(), nurbsdegree);
                ds_2dcurves.Add(ds_2dnurbscrv);
            }

            return ds_2dcurves.ToArray();
        }

        /// <summary>
        /// Iteratively Create PolyCurve "ByJoinedCurves" (reduce occurence of branching segments)
        /// </summary>
        /// <param name="m_crvs"></param>
        /// <returns></returns>
        private List<Autodesk.Revit.DB.Curve> JoinLoopCurves(List<Autodesk.Revit.DB.Curve> m_crvs)
        {
            try
            {

                List<Autodesk.Revit.DB.Curve> m_pcrvs = new List<Autodesk.Revit.DB.Curve>();
                List<Autodesk.Revit.DB.Curve> m_pcrvsclean = new List<Autodesk.Revit.DB.Curve>();

                List<Autodesk.Revit.DB.Curve> m_crvcopy = new List<Autodesk.Revit.DB.Curve>();
                foreach (Autodesk.Revit.DB.Curve c in m_crvs)
                {
                    m_crvcopy.Add(c);
                }
                m_crvcopy.RemoveAt(0);

                for (int i = 0; i < m_crvs.Count; i++)
                {
                    Autodesk.Revit.DB.Curve m_crv = m_crvs[i];

                    List<int> m_remove = new List<int>(); // remove integers
                    for (int j = 0; j < m_crvcopy.Count; j++)
                    {
                        double comp1 = Math.Round(m_crv.StartPoint.X + m_crv.StartPoint.Y + m_crv.StartPoint.Z + m_crv.EndPoint.X + m_crv.EndPoint.Y + m_crv.EndPoint.Z + m_crv.Length, 3);
                        double comp2 = Math.Round(m_crvcopy[j].StartPoint.X + m_crvcopy[j].StartPoint.Y + m_crvcopy[j].StartPoint.Z + m_crvcopy[j].EndPoint.X + m_crvcopy[j].EndPoint.Y + m_crvcopy[j].EndPoint.Z + m_crvcopy[j].Length, 3);
                        if (comp1 != comp2)
                        {
                            List<Autodesk.Revit.DB.Curve> m_connection = new List<Autodesk.Revit.DB.Curve>();
                            m_connection.Add(m_crv);
                            m_connection.Add(m_crvcopy[j]);

                            try
                            {
                                m_crv = Autodesk.Revit.DB.PolyCurve.ByJoinedCurves(m_connection);
                                m_remove.Add(j);
                                m_crvcopy.RemoveAt(j);
                                j--;
                            }
                            catch { }
                        }

                    }

                    if (m_crv.IsClosed == true)
                    {
                        m_pcrvs.Add(m_crv);
                    }
                }

                // remove duplicates
                m_pcrvs = RemoveDuplicateCurves(m_pcrvs);
                foreach (Autodesk.Revit.DB.Curve c in m_pcrvs)
                {
                    m_pcrvsclean.Add(c);
                }

                return m_pcrvsclean;
            }
            catch { return null; }
        }

        /// <summary>
        /// Remove very small segments from a curve list
        /// </summary>
        /// <param name="m_crvs"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        private List<Autodesk.Revit.DB.Curve> RemoveSmallSegments(List<Autodesk.Revit.DB.Curve> m_crvs, double tol)
        {
            List<Autodesk.Revit.DB.Curve> m_longcrvs = new List<Autodesk.Revit.DB.Curve>();
            foreach (Autodesk.Revit.DB.Curve c in m_crvs)
            {
                if (c.Length > tol)
                {
                    m_longcrvs.Add(c);
                }
            }
            return m_longcrvs;
        }

        /// <summary>
        /// Return unique list of curves without duplicates
        /// </summary>
        /// <param name="m_crvs"></param>
        /// <returns></returns>
        private List<Autodesk.Revit.DB.Curve> RemoveDuplicateCurves(List<Autodesk.Revit.DB.Curve> m_crvs)
        {
            try
            {
                List<Autodesk.Revit.DB.Curve> m_crvs1 = m_crvs;
                List<Autodesk.Revit.DB.Curve> m_crvs2 = m_crvs;

                List<Autodesk.Revit.DB.Curve> m_crvsunique = new List<Autodesk.Revit.DB.Curve>();

                // sort through curves
                for (int i = 0; i < m_crvs1.Count; i++)
                {
                    Autodesk.Revit.DB.Curve c1 = m_crvs1[i];
                    int count = 0;
                    List<int> indeces = new List<int>();
                    for (int j = 0; j < m_crvs2.Count; j++)
                    {
                        Autodesk.Revit.DB.Curve c2 = m_crvs2[j];

                        double comp1 = Math.Round(c1.StartPoint.X + c1.StartPoint.Y + c1.StartPoint.Z + c1.EndPoint.X + c1.EndPoint.Y + c1.EndPoint.Z + c1.Length, 3);
                        double comp2 = Math.Round(c2.StartPoint.X + c2.StartPoint.Y + c2.StartPoint.Z + c2.EndPoint.X + c2.EndPoint.Y + c2.EndPoint.Z + c2.Length, 3);

                        bool m_dup = c1.IsAlmostEqualTo(c2);
                        if (comp1 == comp2)
                        {
                            if (count == 0)
                            {
                                m_crvsunique.Add(c1); // add to unique curve list
                            }
                            count++;
                        }
                    }
                }

                return m_crvsunique;
            }
            catch { return null; }
        }

        /// <summary>
        /// Join Perimeter curves into close loops
        /// </summary>
        /// <param name="rh_curves"></param>
        /// <returns></returns>
        private List<Rhino.Geometry.PolyCurve> JoinPerimeterCurves(List<Rhino.Geometry.Curve> rh_curves)
        {
            // polycurve list
            List<Rhino.Geometry.PolyCurve> rh_polycurves = new List<Rhino.Geometry.PolyCurve>();

            int count = rh_curves.Count;
            Rhino.Geometry.PolyCurve pcurve = new Rhino.Geometry.PolyCurve();
            Rhino.Geometry.PolyCurve pcurvetest = new Rhino.Geometry.PolyCurve();
            foreach (Rhino.Geometry.Curve c in rh_curves)
            {
                pcurvetest.Append(c);
                if (pcurvetest.IsClosed == false)
                {
                    pcurve.Append(c);
                }
                else
                {
                    pcurve.Append(c);
                    rh_polycurves.Add(pcurve);
                    pcurve = new Rhino.Geometry.PolyCurve();
                    pcurvetest = new Rhino.Geometry.PolyCurve();
                }

            }
            return rh_polycurves;
        }

        /// <summary>
        /// Join Perimeter curves into close loops
        /// </summary>
        /// <param name="rh_curves"></param>
        /// <returns></returns>
        private List<Rhino.Geometry.PolyCurve> JoinCurves(List<Rhino.Geometry.Curve> rh_curves)
        {

            // polycurve list
            List<Rhino.Geometry.PolyCurve> rh_polycurves = new List<Rhino.Geometry.PolyCurve>();

            int count = rh_curves.Count;
            Rhino.Geometry.PolyCurve pcurve = new Rhino.Geometry.PolyCurve();
            Rhino.Geometry.PolyCurve pcurvetest = new Rhino.Geometry.PolyCurve();

            foreach (Rhino.Geometry.Curve c in rh_curves)
            {
                if (c.IsClosed == false)
                {
                    pcurvetest.Append(c);
                    if (pcurvetest.IsClosed == false)
                    {
                        pcurve.Append(c);
                    }
                    else
                    {
                        pcurve.Append(c);
                        rh_polycurves.Add(pcurve);
                        pcurve = new Rhino.Geometry.PolyCurve();
                        pcurvetest = new Rhino.Geometry.PolyCurve();
                    }
                }
                else
                {
                    Rhino.Geometry.PolyCurve pc = new Rhino.Geometry.PolyCurve();
                    pc.Append(c);
                    rh_polycurves.Add(pc);
                }

            }
            return rh_polycurves;
        }
        #endregion
        */
    }
}