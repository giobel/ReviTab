using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ReviTab
{
	[Transaction(TransactionMode.Manual)]
	public class RhinoSpline : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;


			Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Select Element");

			Options geometryOptions = new Options();
			geometryOptions.ComputeReferences = false;

			GeometryElement geomElem = doc.GetElement(r).get_Geometry(geometryOptions);

			List<NurbSpline> cadSplines = new List<NurbSpline>();

			IList<XYZ> controlPoints = new List<XYZ>();
			List<double> weights = new List<double>();
			List<double> knots = new List<double>();

			if (null != geomElem)
			{
				foreach (var o in geomElem)
				{
					GeometryInstance gi = o as GeometryInstance;
					GeometryElement instanceGeometryElement = gi.GetInstanceGeometry();

					foreach (GeometryObject instanceObj in instanceGeometryElement)
					{
						if (instanceObj.GetType().ToString().Contains("NurbSpline"))
						{
							//TaskDialog.Show("r", instanceObj.GetType().ToString());	
							NurbSpline nurb = instanceObj as NurbSpline;
							cadSplines.Add(nurb);
							controlPoints = nurb.CtrlPoints;
							//weights = nurb.Weights;
							weights = nurb.Weights.Cast<double>().ToList();
							//knots = nurb.Knots;
							knots = nurb.Knots.Cast<double>().ToList();
						}
						break;
					}

				}
			}

			double scale = 0.3048;

			//List<XYZ> controlPoints = new List<XYZ>();
			//controlPoints.Add(new XYZ(0 / scale, 0 / scale, 0 / scale));
			//controlPoints.Add(new XYZ(5 / scale, 5 / scale, 2 / scale));
			//controlPoints.Add(new XYZ(10 / scale, 10 / scale, 5 / scale));
			//controlPoints.Add(new XYZ(15 / scale, 10 / scale, 5 / scale));
			//controlPoints.Add(new XYZ(20 / scale, 5 / scale, 4 / scale));
			//controlPoints.Add(new XYZ(25 / scale, 5 / scale, 3 / scale));

			//List<double> weights = new List<double>();
			//weights.Add(1.0);
			//weights.Add(1.0);
			//weights.Add(1.0);
			//weights.Add(1.0);
			//weights.Add(1.0);
			//weights.Add(1.0);

			//List<double> knots = new List<double>();
			//knots.Add(0); //1revit
			//knots.Add(0); //2
			//knots.Add(0); //3
			//knots.Add(0); //4
			//knots.Add(10.76); //5
			//knots.Add(21.51); //6
			//knots.Add(32.27); //7
			//knots.Add(32.27);
			//knots.Add(32.27); //9
			//knots.Add(32.27);//revit

			HermiteSpline hermspline = HermiteSpline.Create(controlPoints, false);

			//Curve nurbSpline = NurbSpline.Create(hermspline);
			Curve nurbSpline = NurbSpline.CreateCurve(3, knots, controlPoints, weights);

			//XYZ startPoint = nurbSpline.GetEndPoint(0);
			
			Transform nurbsTr = nurbSpline.ComputeDerivatives(0, true);

			XYZ startPoint = nurbsTr.Origin;
			//PrintPoint("a", nurbsTr.Origin);

			//Plane geomPlane = Autodesk.Revit.DB.Plane.CreateByOriginAndBasis(nurbsTr.Origin, nurbsTr.BasisY.Normalize(), nurbsTr.BasisZ.Normalize());
			//Plane geomPlane = Autodesk.Revit.DB.Plane.CreateByOriginAndBasis(nurbSpline.GetEndPoint(0), nurbsTr.BasisY.Normalize(), nurbsTr.BasisZ.Normalize());
			//Frame f = new Frame(nurbSpline.GetEndPoint(0), nurbsTr.BasisY.Normalize(), nurbsTr.BasisZ.Normalize(), nurbsTr.BasisX.Normalize());
			//Plane geomPlane = Autodesk.Revit.DB.Plane.CreateByThreePoints(XYZ.Zero, XYZ.BasisX, XYZ.BasisZ);
			//Plane geomPlane = Plane.CreateByNormalAndOrigin(nurbsTr.BasisX.Normalize(), nurbSpline.GetEndPoint(1)); funziona
			//Plane geomPlane = Plane.CreateByThreePoints(startPoint, startPoint + nurbsTr.BasisX.Normalize(), startPoint + XYZ.BasisZ);

			//XYZ curveDir = controlPoints[1] - controlPoints[0];
			XYZ curveDir = nurbsTr.BasisX;
			XYZ perpDir = curveDir.CrossProduct(startPoint + XYZ.BasisZ).Normalize();
			Plane perpPlane = Plane.CreateByNormalAndOrigin(curveDir, startPoint);
			//Plane vertPlane = Plane.CreateByThreePoints(startPoint, perpPlane.XVec, XYZ.BasisZ);
			Plane vertPlane = perpPlane;

			//PrintPoint("per", perpDir);

			List<PtCoord> pointsCoordinates = new List<PtCoord>(){new PtCoord(5,0), new PtCoord(2,2), new PtCoord(-14,0),
					new PtCoord(2,-2)};

			List<XYZ> pts = VertexPoints(nurbsTr.Origin, pointsCoordinates, vertPlane);

			XYZ pt1 = nurbsTr.Origin;
			XYZ pt2 = pt1 + vertPlane.XVec * 5;
			XYZ pt3 = pt2 + vertPlane.YVec * 2 + vertPlane.XVec * 2;
			XYZ pt4 = pt3 - vertPlane.XVec * 12;
			XYZ pt5 = pt4 - vertPlane.YVec * 2 + vertPlane.XVec * 2;

			

			Line l1 = Line.CreateBound(pt1, pt2);
			Line l2 = Line.CreateBound(pt2, pt3);
			Line l3 = Line.CreateBound(pt3, pt4);
			Line l4 = Line.CreateBound(pt4, pt5);
			Line l5 = Line.CreateBound(pt5, pt1);
			//			
			//			var profileLoop = CurveLoop.Create(new List<Curve>{l1, l2, l3, l4, l5});

			var profileLoop = LoopPoints(pts);

			double rotAngle = -15.216 * Math.PI / 180;
			var transform = Transform.CreateRotationAtPoint(nurbsTr.BasisX, rotAngle, nurbsTr.Origin);

			profileLoop.Transform(transform);

			var loops = new List<CurveLoop> { profileLoop };

			var path = CurveLoop.Create(new List<Curve> { nurbSpline });

			WireframeBuilder builder = new WireframeBuilder();

			builder.AddCurve(nurbSpline);


			//Solid solid = GeometryCreationUtilities.CreateSweptGeometry(path,0,nurbSpline.GetEndParameter(0),loops);
			Solid solid = GeometryCreationUtilities.CreateFixedReferenceSweptGeometry(path, 0, nurbSpline.GetEndParameter(0), loops, XYZ.BasisZ);


			using (Transaction t = new Transaction(doc, "create spline"))
			{
				t.Start();

				ElementId categoryId = new ElementId(BuiltInCategory.OST_GenericModel);

				DirectShape ds = DirectShape.CreateElement(doc, categoryId);

				ds.SetShape(builder);

				ds.Name = "RhinoSpline";

				SketchPlane sp = SketchPlane.Create(doc, vertPlane);
				uidoc.ActiveView.SketchPlane = sp;

				//uidoc.ActiveView.ShowActiveWorkPlane();     

				ModelLine line1 = doc.Create.NewModelCurve(l1, sp) as ModelLine;
				ModelLine line2 = doc.Create.NewModelCurve(l2, sp) as ModelLine;
				ModelLine line3 = doc.Create.NewModelCurve(l3, sp) as ModelLine;
				ModelLine line4 = doc.Create.NewModelCurve(l4, sp) as ModelLine;
				ModelLine line5 = doc.Create.NewModelCurve(l5, sp) as ModelLine;

				List<GeometryObject> gs = new List<GeometryObject>();
				gs.Add(solid);

				//DirectShape directShape = DirectShape.CreateElement(doc, categoryId);
				ds.AppendShape(gs);

				t.Commit();
			}

			return Result.Succeeded;
		}
		private List<XYZ> VertexPoints(XYZ startPoint, List<PtCoord> coordinates, Plane plane)
		{

			List<XYZ> result = new List<XYZ>() { startPoint };

			XYZ start = startPoint;

			foreach (PtCoord coord in coordinates)
			{
				XYZ current = start + plane.XVec * coord.xValue + plane.YVec * coord.yValue;
				
				result.Add(current);
				start = current;
			}
			return result;
		}

		private CurveLoop LoopPoints(List<XYZ> points)
		{

			IList<Curve> curves = new List<Curve>();

			for (int i = 0; i < points.Count - 1; i++)
			{
				Line l = Line.CreateBound(points[i], points[i + 1]);
				curves.Add(l);
			}
			curves.Add(Line.CreateBound(points.Last(), points.First()));
			return CurveLoop.Create(curves);
		}
	}
	public class PtCoord
	{

		public double xValue { get; set; }
		public double yValue { get; set; }

		public PtCoord(double x, double y)
		{
			xValue = x;
			yValue = y;
		}
	}
}


