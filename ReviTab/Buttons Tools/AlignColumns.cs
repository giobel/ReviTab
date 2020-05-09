using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class AlignColumns : IExternalCommand
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
            
            IList<Reference> linkModelColumns = uidoc.Selection.PickObjects(ObjectType.LinkedElement, "Select Columns in Linked Model");

            if (linkModelColumns.Count == 0)
            {
                TaskDialog td = new TaskDialog("Warning");
                td.MainContent = "No columns in the linked model have been selected";
                td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                td.Show();
                return Result.Cancelled;
            }

            IList<Reference> currentModelColumns = uidoc.Selection.PickObjects(ObjectType.Element, "No columns in the current model have been selected");


            if (currentModelColumns.Count == 0)
            {
                TaskDialog td = new TaskDialog("Warning");
                td.MainContent = "Please select some columns the current model";
                td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                td.Show();
                return Result.Cancelled;

            }
            List<XYZ> linkedColumnsLocations = new List<XYZ>();

            foreach (Reference linkedColumn in linkModelColumns)
            {
                
                Element e = doc.GetElement(linkedColumn.ElementId);
                RevitLinkInstance revitLinkInst = e as RevitLinkInstance;
                Document linkRvtDoc = (e as RevitLinkInstance).GetLinkDocument();

                Element eLinked = linkRvtDoc.GetElement(linkedColumn.LinkedElementId);
                Transform transf = revitLinkInst.GetTransform();
                LocationPoint lp = eLinked.Location as LocationPoint;
                XYZ columnPoint = lp.Point;

                //linkedColumnsLocations.Add(columnPoint);
                linkedColumnsLocations.Add(TransformPoint(columnPoint, transf));

            }

                using (Transaction t = new Transaction(doc, "Move Columns"))
            {

                t.Start();

                try
                {
                    foreach (Reference currentColumnRef in currentModelColumns)
                    {
                        Element currentModelColumnElement = doc.GetElement(currentColumnRef);
                        LocationPoint currentModelColumnLocation = currentModelColumnElement.Location as LocationPoint;

                        XYZ closestPoint = FindClosestPointTolerance(currentModelColumnLocation.Point, linkedColumnsLocations, 3);

                        if (null != closestPoint)
                            currentModelColumnElement.Location.Move(closestPoint - currentModelColumnLocation.Point);
                        //ElementTransformUtils.MoveElement(doc, currentModelColumnElement.Id, closestPoint-currentModelColumnLocation.Point);

                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("e", ex.Message);
                }

                t.Commit();
            }

            return Result.Succeeded;

        }//close Execute


        public static XYZ TransformPoint(XYZ point, Transform transform)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;

            //transform basis of the old coordinate system in the new coordinate // system
            XYZ b0 = transform.get_Basis(0);
            XYZ b1 = transform.get_Basis(1);
            XYZ b2 = transform.get_Basis(2);
            XYZ origin = transform.Origin;

            //transform the origin of the old coordinate system in the new 
            //coordinate system
            double xTemp = x * b0.X + y * b1.X + z * b2.X + origin.X;
            double yTemp = x * b0.Y + y * b1.Y + z * b2.Y + origin.Y;
            double zTemp = x * b0.Z + y * b1.Z + z * b2.Z + origin.Z;

            return new XYZ(xTemp, yTemp, zTemp);
        }

        public XYZ FindClosestPointTolerance(XYZ point, List<XYZ> sourcePoints, double tolerance)
        {
            foreach (XYZ sourcePoint in sourcePoints)
            {
                if (point.DistanceTo(sourcePoint) < tolerance)
                {
                    return sourcePoint;
                }
            }

            return null;
        }
    }

}
