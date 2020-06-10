#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using Rhino.FileIO;
using Rhino.Geometry;
using System.Linq;
using System.Diagnostics;
using System;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class RhinoImport : IExternalCommand
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
            View activeView = doc.ActiveView;

            //duplicated in the helper file! remember to change both
            double scale = 304.8;

        //string path = @"C:\Users\gbrog\Desktop\test.3dm";

        string path = @"C:\Users\giovanni.brogiolo\Desktop\ST00-0221.3dm";

            File3dm rhinoModel = File3dm.Read(path);

            List<Rhino.DocObjects.Layer> m_layers = rhinoModel.AllLayers.ToList();

            List<string> layers = Get_RhinoLayerNames(rhinoModel);

            File3dmObject[] rhinoObjects = Get_RhinoObjects(rhinoModel);
            
            List<Rhino.Geometry.LineCurve> rh_lines = new List<Rhino.Geometry.LineCurve>();

            List<Rhino.Geometry.ArcCurve> rh_arc = new List<Rhino.Geometry.ArcCurve>();

            List<Rhino.Geometry.TextEntity> rh_text = new List<TextEntity>();

            List<Rhino.Geometry.Leader> rh_textLeader = new List<Rhino.Geometry.Leader>();

            List<Rhino.Geometry.LinearDimension> rh_linearDimension = new List<LinearDimension>();

            List<List<Rhino.Geometry.Point3d>> rh_polylineCurvePoints = new List<List<Point3d>>();

            List<Tuple<string, XYZ>> rh_Blocks = new List<Tuple<string, XYZ>>();

            foreach (var item in rhinoObjects)
            {
                GeometryBase geo = item.Geometry;

                // check if geometry is a curve
                if (geo is Rhino.Geometry.LineCurve)
                {
                    // add curve to list
                    Rhino.Geometry.LineCurve ln = geo as Rhino.Geometry.LineCurve;
                    rh_lines.Add(ln);
                }

                if (geo is Rhino.Geometry.ArcCurve)
                {
                    // add curve to list
                    Rhino.Geometry.ArcCurve arc = geo as Rhino.Geometry.ArcCurve;
                    rh_arc.Add(arc);
                }


                if (!item.Attributes.IsInstanceDefinitionObject && geo is Rhino.Geometry.PolylineCurve)
                {
                    PolylineCurve pc = geo as PolylineCurve;

                    Polyline pl = pc.ToPolyline();

                    rh_polylineCurvePoints.Add(pl.ToList());
                }

                if (geo is Rhino.Geometry.TextEntity)
                {
                    TextEntity te = geo as Rhino.Geometry.TextEntity;
                    rh_text.Add(te);
                }

                if (geo is Rhino.Geometry.Leader)
                {
                    rh_textLeader.Add(geo as Rhino.Geometry.Leader);

                    //var text = geo as Rhino.Geometry.Leader;

                    //TaskDialog.Show("r", text.PlainText);
                }

                if (geo is Rhino.Geometry.AnnotationBase)
                {
                    var text = geo as Rhino.Geometry.AnnotationBase;

                    //TaskDialog.Show("r", text.PlainText);
                }

                if (geo is Rhino.Geometry.LinearDimension)
                {
                    LinearDimension ld = geo as Rhino.Geometry.LinearDimension;
                    rh_linearDimension.Add(ld);
                }

                if (geo is Rhino.Geometry.InstanceReferenceGeometry)
                {
                    InstanceReferenceGeometry refGeo = (InstanceReferenceGeometry)geo;

                    // Lookup the parent block definition
                    System.Guid blockDefId = refGeo.ParentIdefId;
                    InstanceDefinitionGeometry def = rhinoModel.AllInstanceDefinitions.FindId(blockDefId);

                    // block definition name
                    string defname = def.Name;

                    //TaskDialog.Show("R", defname);

                    // transform data for block instance
                    Rhino.Geometry.Transform xform = refGeo.Xform;

                    double x = refGeo.Xform.M03/scale;
                    double y = refGeo.Xform.M13/scale;

                    XYZ placementPt = new XYZ(x, y, 0);

                    rh_Blocks.Add(new Tuple<string, XYZ>(defname, placementPt) );
                }
            }

            //TaskDialog.Show("r", rh_linearDimension.Count.ToString());

            Rhynamo.clsGeometryConversionUtils rh_ds = new Rhynamo.clsGeometryConversionUtils();

            using (Transaction t = new Transaction(doc, "Convert lines"))
            {
                t.Start();

                rh_ds.Convert_rhLinesToRevitDetailCurve(doc, rh_lines, "3 Arup Continuous Line");

                rh_ds.Convert_PolycurveToLines(doc, rh_polylineCurvePoints, "1 Arup Continuous Line");

                rh_ds.Convert_ArcsToDS(doc, rh_arc);

                rh_ds.RhinoTextToRevitNote(doc, rh_text);

                rh_ds.RhinoLeaderToRevitNote(doc, rh_textLeader);

                rh_ds.RhinoToRevitDimension(doc, rh_linearDimension);

                rh_ds.Convert_rhBlocks(doc, rh_Blocks);

                t.Commit();
            }

            TaskDialog.Show("r", "Done");

            return Result.Succeeded;
        }
        /// <summary>
        /// Gets Rhino File3dmObjects from a model using a layer name.
        /// </summary>
        /// <param name="RhinoModel">The Rhino model object (File3dm)</param>
        /// <param name="layer">The name of the layer</param>
        /// <returns name="RhinoObjects">The list of Rhino objects.</returns>
        /// <search>case,rhino,model,3dm,rhynamo</search>
        public static File3dmObject[] Get_RhinoObjectsByLayer(File3dm RhinoModel, string layer)
        {
            File3dmObject[] objects = RhinoModel.Objects.FindByLayer(layer);
            // return info
            return objects;
        }

        /// <summary>
        /// Gets Rhino layer names.
        /// </summary>
        /// <param name="RhinoModel">The Rhino model object</param>
        /// <returns name="LayerNames">List of Rhino layer names.</returns>
        /// <search>case,rhino,model,layers,3dm,rhynamo</search>
        public static List<string> Get_RhinoLayerNames(File3dm RhinoModel)
        {
            List<string> m_names = new List<string>();
            foreach (Rhino.DocObjects.Layer lay in RhinoModel.AllLayers)
            {
                m_names.Add(lay.Name);
            }
            // return info
            return m_names;
        }

        /// <summary>
        /// Gets Rhino all Rhino 3dm Objects
        /// </summary>
        /// <param name="RhinoModel">The Rhino model object (File3dm)</param>
        /// <returns name="RhinoObjects">The list of Rhino objects.</returns>
        /// <search>case,rhino,model,3dm,rhynamo</search>
        public static File3dmObject[] Get_RhinoObjects(File3dm RhinoModel)
        {
            List<File3dmObject> m_objslist = new List<File3dmObject>();
            List<string> m_names = new List<string>();

            // search through each layer
            foreach (Rhino.DocObjects.Layer lay in RhinoModel.AllLayers)
            {
                File3dmObject[] m_objs = RhinoModel.Objects.FindByLayer(lay.Name);
                foreach (File3dmObject obj in m_objs)
                {
                    m_objslist.Add(obj);
                }
            }

            // return info
            return m_objslist.ToArray();
        }

    }
}
