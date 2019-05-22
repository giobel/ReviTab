#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class DrawObjectAxis : IExternalCommand
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

            Reference r = uidoc.Selection.PickObject(ObjectType.Face, "Please pick a point on a face");
            Element e = doc.GetElement(r.ElementId);

            FamilyInstance fi = e as FamilyInstance;

            Transform transf = fi.GetTotalTransform();

            XYZ pntCenter = r.GlobalPoint;


            using (Transaction t = new Transaction(doc, "Draw Axis"))
            {
                t.Start();

                DrawAxisPerpendicularToDirections(doc, pntCenter, transf.BasisY, transf.BasisX, 1000, new Color(0, 255, 0)); //perpendicular
                DrawAxisPerpendicularToDirections(doc, pntCenter, transf.BasisX, transf.BasisZ, 1000, new Color(250, 10, 10)); //V direction
                DrawAxisPerpendicularToDirections(doc, pntCenter, transf.BasisZ, transf.BasisY, 1000, new Color(10, 10, 250)); //U direction

                DrawAxisPerpendicularToDirections(doc, pntCenter, XYZ.BasisY, XYZ.BasisX, 500, new Color(10, 150, 10)); //perpendicular
                DrawAxisPerpendicularToDirections(doc, pntCenter, XYZ.BasisX, XYZ.BasisZ, 500, new Color(150, 10, 10)); //V direction
                DrawAxisPerpendicularToDirections(doc, pntCenter, XYZ.BasisZ, XYZ.BasisY, 500, new Color(10, 10, 150)); //U direction



                //doc.ActiveView.SketchPlane = perpSplane;


                t.Commit();
            }

            return Result.Succeeded;
        }

        private void DrawAxisPerpendicularToDirections(Document doc, XYZ origin, XYZ dir1, XYZ dir2, double length, Color color)
        {

            XYZ pntCenter = origin;
            XYZ cross = dir2.CrossProduct(dir1);
            XYZ pntEnd = pntCenter + cross.Multiply(length / 304.8);
            Plane perpPlane = Plane.CreateByThreePoints(pntCenter, pntEnd, dir1);
            SketchPlane perpSplane = SketchPlane.Create(doc, perpPlane);
            Line line2 = Autodesk.Revit.DB.Line.CreateBound(pntCenter, pntEnd);
            ModelLine perpLine = doc.Create.NewModelCurve(line2, perpSplane) as ModelLine;

            try
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                ogs.SetProjectionLineColor(color); // or other here
                doc.ActiveView.SetElementOverrides(perpLine.Id, ogs);
            }
            catch
            {

            }

        }

    }

}
