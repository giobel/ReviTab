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
        public class PlaceTags : IExternalCommand
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

                ElementId defaultTextTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                IEnumerable<Element>tagLocationElements = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType().Where(x=>x.Name == "Tag Location");

            List<XYZ> tagLocationDistances = new List<XYZ>();

            foreach (Element item in tagLocationElements)
            {
                LocationPoint lp = item.Location as LocationPoint;
                tagLocationDistances.Add(lp.Point);
            }

            double refPointZ = tagLocationDistances.First().Z;

            //how to find the closest element to a tagLocation? 
            // Option 1: Create a plane with the element location curve and find the smallest distance between plane and point
            // Option 2: Raycast -> does not work in 2d
            // Option 3: Store the element location point in the tagElement and try to use that to find the element in future. What if the element moves by a lot?

            ICollection<Element> fecElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                                                                .Where(c => c.Category !=null)
                                                                .Where(x => x.Category.Name.Contains("Framing")).ToList();
            ICollection<ElementId> result = new List<ElementId>();

            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorn = TagOrientation.Horizontal;

            using (Transaction t = new Transaction(doc, "Place Tags"))
            {
                t.Start();

                foreach (Element item in fecElements)
                {
                    LocationCurve lc = item.Location as LocationCurve;
                    Curve crv = lc.Curve;

                    XYZ stPt = crv.GetEndPoint(0);
                    XYZ endPt = crv.GetEndPoint(1);

                    XYZ newSt = ProjectedZPoint(stPt, refPointZ);
                    XYZ newEnd = ProjectedZPoint(endPt, refPointZ);

                    Curve projectedCurve = Line.CreateBound(newSt, newEnd);

                    //XYZ zPoint = new XYZ(crv.GetEndPoint(0).X, crv.GetEndPoint(0).Y, 1);
                    //Plane pl = Plane.CreateByThreePoints(crv.GetEndPoint(0), crv.GetEndPoint(1), zPoint);
                    //XYZ closestPoint = ClosestPtToPlane(pl, tagLocationDistances);
                    XYZ closestPoint = ClosestPtToCurve(projectedCurve, tagLocationDistances);
                    if (closestPoint != null)
                    {
                        tagLocationDistances.Remove(closestPoint);
                        Reference refe = new Reference(item);
                        IndependentTag newTag = IndependentTag.Create(doc, doc.ActiveView.Id, refe, false, tagMode, tagorn, closestPoint);
                    }
                    
                    //TaskDialog.Show("R", resulta.X.ToString());
                    
                }

                t.Commit();
            }
                

            


            uidoc.Selection.SetElementIds(result);
            TaskDialog.Show("R", "done");
            return Result.Succeeded;
            }
        public XYZ ProjectedZPoint (XYZ pt, double z)
        {
            return new XYZ(pt.X, pt.Y, z);
        }
        public static XYZ ClosestPtToCurve(Curve crv, List<XYZ> points)
        {
            double distance = 1;
            XYZ pt = null;
            foreach (XYZ point in points)
            {
                double currentDistance = Math.Abs(crv.Distance(point));
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    pt = point;
                }
            }

            return pt;
        }

        public static XYZ ClosestPtToPlane(Plane plane, List<XYZ> points)
        {
            double distance = 1000;
            XYZ pt = null; 
            foreach (XYZ point in points)
            {
                double currentDistance = Math.Abs(SignedDistanceTo(plane, point));
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    pt = point;
                }
            }

            return pt;
        }

        public static double SignedDistanceTo(Plane plane, XYZ p) {
            XYZ v = p - plane.Origin;
            return plane.Normal.DotProduct(v);
        }

    }
    
}
