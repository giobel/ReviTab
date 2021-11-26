using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rg = Rhino.Geometry;

namespace ReviTab
{
	[Transaction(TransactionMode.Manual)]
	class RhinoMesh : IExternalCommand
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

			//double scale = 304.8;
			#region TEST

			////cut line endpoints
			//XYZ v1 = new XYZ(0, 2000, 0)/scale;
			//XYZ v2 = new XYZ(6018, 2000, 0) / scale;
			
            
			////rhino cut line endpoints            
   //         rg.Point3d rv0 = new rg.Point3d(0, 2000, 0);
   //         rg.Point3d rv1 = new rg.Point3d(6018, 2000, 0);
			//rg.Point3d rv2 = new rg.Point3d(0, 2000, 1);

			////rhino cut plane
			//rg.Plane cutPlane = new rg.Plane(rv0, rv1, rv2);



			////wip points
			//rg.Point3d rv3 = new rg.Point3d(857, 203, 4597);
			//rg.Point3d rv4 = new rg.Point3d(857, 6221, 4597);
			//rg.Point3d rv5 = new rg.Point3d(9818, 6221, 4597);
			

   //         //wip plane
   //         rg.Plane rhinoPlane = new rg.Plane(rv3, rv4, rv5);

   //         rg.Line intLine;

   //         var result = rg.Intersect.Intersection.PlanePlane(cutPlane, rhinoPlane, out intLine);

   //         using (Transaction t = new Transaction(doc, "Project Line"))
   //         {
   //             t.Start();

   //             SketchPlane sp = SketchPlane.Create(doc, RhinoToRevitPlane(cutPlane));

   //             doc.Create.NewModelCurve(RhinoToRevitLine(intLine), sp);

   //             t.Commit();
   //         }

   //         TaskDialog.Show("r", "Done");

            #endregion


            #region Revit
            
			//	SELECT THE LINE TO PROJECT AND CONSTRUCT THE PLANE TO INTERSECT THE MESH
			Reference lineToProject = uidoc.Selection.PickObject(ObjectType.Element, "Select cut Line");
			Element lineElement = doc.GetElement(lineToProject);

			CurveElement curveLine = lineElement as CurveElement;
			LocationCurve lineLocation = lineElement.Location as LocationCurve;

			Line line = lineLocation.Curve as Line;

			rg.Line rhinoCutLine = new rg.Line(RevitToRhinoPoint(line.GetEndPoint(0)), RevitToRhinoPoint(line.GetEndPoint(1)));

			XYZ perpDir = line.Direction.CrossProduct(XYZ.BasisZ).Normalize();

			Plane cutPlane = Plane.CreateByNormalAndOrigin(perpDir, line.GetEndPoint(0));

			// GET THE MESH GEOMETRY

			Reference meshRef = uidoc.Selection.PickObject(ObjectType.Element, "Select Mesh");
			Element e = doc.GetElement(meshRef);

			Options opt = new Options();
			opt.ComputeReferences = true;
			opt.IncludeNonVisibleObjects = false;
			opt.View = doc.ActiveView;

			GeometryElement geomElem = e.get_Geometry(opt);

			Mesh geomMesh = null;

			foreach (GeometryObject geomObj in geomElem)
			{
				GeometryInstance gi = geomObj as GeometryInstance;

				if (gi != null)
                {
					foreach (GeometryObject element in gi.GetInstanceGeometry())
					{
						geomMesh = element as Mesh;
					}
				}
                else
                {
					geomMesh = geomObj as Mesh;
				}
			}
		

			int counter = 0;

			using (Transaction t = new Transaction(doc, "Project Line"))
			{
				t.Start();
				SketchPlane sp = SketchPlane.Create(doc, cutPlane);
				
				XYZ spOrigin = sp.GetPlane().Origin;
				Debug.Write($"Sketch plane origin: {spOrigin}\n");

				XYZ spNormal = sp.GetPlane().Normal;
				Debug.Write($"Sketch plane normal: {spNormal}\n");

				Line spX = Line.CreateBound(spOrigin, spOrigin + XYZ.BasisZ * 100);
				Debug.Write($"line start: {spX.GetEndParameter(0)}\n");
				Debug.Write($"line end: {spX.GetEndParameter(1)}\n");

				ModelCurve mc = doc.Create.NewModelCurve(spX, sp);
				Debug.Write($"mc: {mc.Id}\n");

				List<XYZ> intersectionPoints = new List<XYZ>();

				for (int i = 0; i < geomMesh.NumTriangles; i++)
				{
					MeshTriangle triangle = geomMesh.get_Triangle(i);
					XYZ vertex1 = triangle.get_Vertex(0);
					XYZ vertex2 = triangle.get_Vertex(1);
					XYZ vertex3 = triangle.get_Vertex(2);

					rg.Point3d rhinoVertex1 = RevitToRhinoPoint(vertex1);
					rg.Point3d rhinoVertex2 = RevitToRhinoPoint(vertex2);
					rg.Point3d rhinoVertex3 = RevitToRhinoPoint(vertex3);

					List<rg.Line> rhinoEdges = new List<rg.Line> { new rg.Line(rhinoVertex1, rhinoVertex2), new rg.Line(rhinoVertex1, rhinoVertex2), new rg.Line(rhinoVertex1, rhinoVertex2) };

					XYZ[] pts = new XYZ[2];

					int ptscount = 0;

					foreach (rg.Line edge in rhinoEdges)
                    {
						double a = 0;
						double b = 0;

						bool result = rg.Intersect.Intersection.LineLine(edge, rhinoCutLine, out a, out b);

						if (result)
						{
							intersectionPoints.Add(RhinoToRevitPoint(edge.PointAt(a)));
							pts[ptscount] = RhinoToRevitPoint(edge.PointAt(a));
							ptscount = 1;
							counter++;
						}
					}

					if (pts[0] != null)
                    {
						try
						{
							Line temp = Line.CreateBound(pts[0], pts[0] + XYZ.BasisZ * 100);
							//Line li = Line.CreateBound(pts[0], new XYZ(pts[0].X, pts[0].Y, pts[0].Z +100));
							doc.Create.NewModelCurve(temp, sp);
						}
						catch { }
					}

				}


				t.Commit();
			}
			TaskDialog.Show("Intersections found", counter.ToString());
			
            #endregion

            return Result.Succeeded;
		}

		//RHINO TO REVIT
		public Line RhinoToRevitLine(rg.Line line)
		{

			return Line.CreateBound(RhinoToRevitPoint(line.From), RhinoToRevitPoint(line.To));
		}

		public XYZ RhinoToRevitPoint(rg.Point3d pt)
		{
			return new XYZ(pt.X, pt.Y, pt.Z);
		}

		public XYZ RhinoToRevitPoint(rg.Vector3d pt)
		{
			return new XYZ(pt.X, pt.Y, pt.Z);
		}

		public Plane RhinoToRevitPlane(rg.Plane plane)
        {
			return Plane.CreateByNormalAndOrigin(RhinoToRevitPoint(plane.Normal), RhinoToRevitPoint(plane.Origin));
        }

		//REVIT TO RHINO
		public rg.Point3d RevitToRhinoPoint(XYZ pt)
		{
			return new rg.Point3d(pt.X, pt.Y, pt.Z);
		}

		public rg.Vector3d RevitToRhinoVector(XYZ pt)
		{
			return new rg.Vector3d(pt.X, pt.Y, pt.Z);
		}


	}


}
