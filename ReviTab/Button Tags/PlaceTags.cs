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

            //how to find the closest element to a tagLocation? 
            // Option 1: Create a plane with the element location curve and find the smallest distance between plane and point
            // Option 2: Raycast -> does not work in 2d
            // Option 3: Store the element location point in the tagElement and try to use that to find the element in future. What if the element moves by a lot?

            ICollection<Element> fecElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                                                                .Where(c => c.Category !=null)
                                                                .Where(x => x.Category.Name.Contains("Framing")).ToList();

            Dictionary<Curve, Element> lineBasedElements = new Dictionary<Curve, Element>();

            foreach (Element item in fecElements)
            {
                LocationCurve lc = item.Location as LocationCurve;
                Curve crv = lc.Curve;
                lineBasedElements.Add(crv, item);
            }


            ICollection<ElementId> result = new List<ElementId>();

            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorn = TagOrientation.Horizontal;

            using (Transaction t = new Transaction(doc, "Place Tags"))
            {
                t.Start();

                foreach (Element item in tagLocationElements)
                {
                    
                    LocationPoint tagLp = item.Location as LocationPoint;
                    Curve closestCurve = ClosestPtToCurve(tagLp.Point, lineBasedElements.Keys.ToList());
                    
                    if (closestCurve != null)
                    {
                        //tagLocationDistances.Remove(closestPoint);
                        Reference refe = new Reference(lineBasedElements[closestCurve]);
                        IndependentTag newTag = IndependentTag.Create(doc, doc.ActiveView.Id, refe, false, tagMode, tagorn, tagLp.Point );
                    }
                    
                    //TaskDialog.Show("R", resulta.X.ToString());
                    
                }

                t.Commit();
            }
                

            


            uidoc.Selection.SetElementIds(result);
            TaskDialog.Show("R", "done");
            return Result.Succeeded;
            }
        public static XYZ ProjectedZPoint (XYZ pt, double z)
        {
            return new XYZ(pt.X, pt.Y, z);
        }


        public static Curve ClosestPtToCurve(XYZ pt, List<Curve> curves)
        {
            double distance = 500;
            Curve closestCurve = null;

            foreach (Curve crv in curves)
            {
                XYZ stPt = crv.GetEndPoint(0);
                XYZ endPt = crv.GetEndPoint(1);

                XYZ newSt = ProjectedZPoint(stPt, pt.Z);
                XYZ newEnd = ProjectedZPoint(endPt, pt.Z);

                Curve projectedCurve = Line.CreateBound(newSt, newEnd);

                double currentDistance = Math.Abs(projectedCurve.Distance(pt));
                if (currentDistance < distance)
                {
                    distance = currentDistance * 304.8; //in mm
                    closestCurve = crv;
                }
            }
            return closestCurve;
        }


        public static XYZ ClosestPtToCurve(Curve crv, List<XYZ> points)
        {
            double distance = 500;
            XYZ pt = null;
            foreach (XYZ point in points)
            {
                double currentDistance = Math.Abs(crv.Distance(point));
                if (currentDistance < distance)
                {
                    distance = currentDistance * 304.8; //in mm
                    pt = point;
                }
            }
            return pt;
        }

        public static XYZ ClosestPtToPlane(Plane plane, List<XYZ> points)
        {
            double distance = 2;
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
