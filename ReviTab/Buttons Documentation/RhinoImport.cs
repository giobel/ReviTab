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

            string path = @"C:\Users\giovanni.brogiolo\Desktop\ST00-0221.3dm";

            File3dm rhinoModel = File3dm.Read(path);

            List<Rhino.DocObjects.Layer> m_layers = rhinoModel.AllLayers.ToList();

            List<string> layers = Get_RhinoLayerNames(rhinoModel);

            File3dmObject[] rhinoObjects = Get_RhinoObjects(rhinoModel);
            
            List<Rhino.Geometry.LineCurve> rh_lines = new List<Rhino.Geometry.LineCurve>();

            List<Rhino.Geometry.TextEntity> rh_text = new List<TextEntity>();

            List<Rhino.Geometry.LinearDimension> rh_linearDimension = new List<LinearDimension>();

            List<Rhino.Geometry.Leader> rh_LeaderText = new List<Rhino.Geometry.Leader>();


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

                if (geo is Rhino.Geometry.TextEntity)
                {
                    TextEntity te = geo as Rhino.Geometry.TextEntity;
                    rh_text.Add(te);
                }

                if (geo is Rhino.Geometry.LinearDimension)
                {
                    LinearDimension ld = geo as Rhino.Geometry.LinearDimension;
                    rh_linearDimension.Add(ld);
                }

                if (geo is Rhino.Geometry.Leader)
                {
                    Rhino.Geometry.Leader leader = geo as Rhino.Geometry.Leader;
                    rh_LeaderText.Add(leader);
                }

            }

            //TaskDialog.Show("r", rh_linearDimension.Count.ToString());

            Rhynamo.clsGeometryConversionUtils rh_ds = new Rhynamo.clsGeometryConversionUtils();

            using (Transaction t = new Transaction(doc, "Convert lines"))
            {
                t.Start();

                rh_ds.Convert_rhLinesToRevitDetailCurve(doc, rh_lines, "3 Arup Continuous Line");

                rh_ds.RhinoTextToRevitNote(doc, rh_text);

                rh_ds.RhinoTextToRevitNote(doc, rh_LeaderText);

                Debug.WriteLine("Draw dimensions");

                rh_ds.RhinoToRevitDimension(doc, rh_linearDimension);
                
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