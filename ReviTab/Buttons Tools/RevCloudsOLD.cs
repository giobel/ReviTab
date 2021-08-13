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
    public class RevClouds : IExternalCommand
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

            IList<ElementId> rev = Revision.GetAllRevisionIds(doc);
            // get elements from user selection
            List<Element> selectedElements = new List<Element>();

            foreach (Reference r in uidoc.Selection.PickObjects(ObjectType.Element))
            {
                Element e = doc.GetElement(r);
                selectedElements.Add(e);
                //TaskDialog.Show("r", GetElementViewSheet(doc, e).Name);
            }

            using (TransactionGroup transGroup = new TransactionGroup(doc))
            {

                using (Transaction t = new Transaction(doc))
                {

                    try
                    {
                        transGroup.Start("Add Revision Clouds");


                        List<Curve> curvesSheet = new List<Curve>();
                        //THIS ONLY WORKS BY ACTIVATING THE VIEWPORT AND SELECTING THE ELEMENTS. AUTOMATICALLY FIND THE SELECTED ELEMENT VIEW -> TBC
                        ViewSheet selectedViewSheet = GetElementViewSheet(doc, doc.ActiveView);

                        foreach (Element e in selectedElements)
                        {

                            View elementView = doc.GetElement(e.OwnerViewId) as View;

                            if (elementView == null)
                            {
                                elementView = doc.ActiveView;
                            }

                            //MOVED OUT THE FOREACH LOOP -> ALL CLOUDS ARE GROUPED. WHAT IF WE HAVE MULTIPLE SHEETS?
                            //ViewSheet selectedViewSheet = GetElementViewSheet(doc, elementView);

                            //AUTOMATICALLY FIND THE SELECTED ELEMENT VIEW -> TBC
                            //View elementView = doc.GetElement(e.OwnerViewId) as View;


                            //FIND VIEWPORT
                            ICollection<ElementId> viewportIds = selectedViewSheet.GetAllViewports();
                            Viewport viewport = null;

                            foreach (ElementId eid in viewportIds)
                            {
                                Viewport vp = doc.GetElement(eid) as Viewport;
                                //TaskDialog.Show("Check", String.Format("{0} : {1}", vp.ViewId.ToString(), elementView.Id.ToString()));
                                if (vp.ViewId.ToString() == elementView.Id.ToString())
                                {
                                    viewport = vp;
                                }
                            }

                            //TaskDialog.Show("Viewport Name",String.Format("Viewport name: {0}",viewport.Name));

                            ICollection<ElementId> categoryToIsolate = new List<ElementId>();
                            Categories groups = doc.Settings.Categories;
                            categoryToIsolate.Add(groups.get_Item(BuiltInCategory.OST_Loads).Id);

                            XYZ viewCenter = null;
                            XYZ changedVPcenter = null;

                            t.Start("Hide Categories");

                            elementView.IsolateCategoriesTemporary(categoryToIsolate);

                            //Use the annotation crop region to find the view centroid
                            ViewCropRegionShapeManager vcr = elementView.GetCropRegionShapeManager();
                            //Set the annotation offset to the minimum (3mm)
                            vcr.BottomAnnotationCropOffset = 3 / 304.8;
                            vcr.TopAnnotationCropOffset = 3 / 304.8;
                            vcr.LeftAnnotationCropOffset = 3 / 304.8;
                            vcr.RightAnnotationCropOffset = 3 / 304.8;
                            //Get the Viewport Center. This will match the View centroid
                            changedVPcenter = viewport.GetBoxCenter();

                            //Find the view centroid using the annotation crop shape (it should always be a rectangle, while the cropbox shape can be a polygon).
                            CurveLoop cloop = vcr.GetAnnotationCropShape();
                            List<XYZ> pts = new List<XYZ>();

                            foreach (Curve crv in cloop)
                            {
                                pts.Add(crv.GetEndPoint(0));
                                pts.Add(crv.GetEndPoint(1));
                            }

                            //View centroid with elements hidden
                            viewCenter = GetCentroid(pts, pts.Count);

                            t.RollBack();


                            int scale = elementView.Scale;


                            t.Start("Add rev cloud");

                            //VIEWPORT CENTER							
                            //XYZ vpMax = viewport.GetBoxOutline().MaximumPoint;
                            //XYZ vpMin = viewport.GetBoxOutline().MinimumPoint;
                            //XYZ vpCen = (vpMax + vpMin)/2;
                            XYZ vpCen = changedVPcenter;


                            //FIND ELEMENT BBOX FOR REVISION CLOUDS
                            BoundingBoxXYZ bbox = e.get_BoundingBox(doc.ActiveView);

                            XYZ pt1 = bbox.Min;
                            XYZ pt3 = bbox.Max;

                            //BOUNDING BOX MIN-MAX
                            if (doc.ActiveView.SketchPlane != null)
                            {
                                Plane p = doc.ActiveView.SketchPlane.GetPlane();
                                pt1 = ProjectOnto(p, pt1);
                                pt3 = ProjectOnto(p, pt3);
                                viewCenter = ProjectOnto(p, viewCenter);
                                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(pt1, pt3));
                            }
                            else
                            {
                                //TaskDialog.Show("r", "Not a plan view!");
                                Plane p = Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection, doc.ActiveView.Origin);
                                pt1 = ProjectOnto(p, pt1);
                                pt3 = ProjectOnto(p, pt3);
                                viewCenter = (pts[0] + pts[3])/2;
                                doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(pt1, pt3));
                            }

                            
                            XYZ pt2 = new XYZ(pt3.X, pt1.Y, pt3.Z);
                            XYZ pt4 = new XYZ(pt1.X, pt3.Y, pt1.Z);
                            //bbox center
                            XYZ bboxCenter = (pt3 + pt1) / 2;

                            //OFFSETS
                            double xOffset = (viewCenter.X - bboxCenter.X) / scale;
                            double yOffset = (viewCenter.Y - bboxCenter.Y) / scale;
                            double zOffset = (viewCenter.Z - bboxCenter.Z) / scale;

                            XYZ pt1Sheet = bbox.Min / scale;
                            XYZ pt3Sheet = bbox.Max / scale;
                            XYZ pt2Sheet = new XYZ(pt3Sheet.X, pt1Sheet.Y, pt3Sheet.Z);
                            XYZ pt4Sheet = new XYZ(pt1Sheet.X, pt3Sheet.Y, pt1Sheet.Z);
                            XYZ ptSheetMid = (pt3Sheet + pt1Sheet) / 2;


                            //MOVE POINTS
                            XYZ moveToVPCenter = vpCen - ptSheetMid;
                            pt1Sheet = pt1Sheet.Add(moveToVPCenter);
                            pt3Sheet = pt3Sheet.Add(moveToVPCenter);
                            pt2Sheet = pt2Sheet.Add(moveToVPCenter);
                            pt4Sheet = pt4Sheet.Add(moveToVPCenter);

                            XYZ moveToViewLocation = new XYZ(-xOffset, -yOffset, -zOffset);
                            pt1Sheet = pt1Sheet.Add(moveToViewLocation);
                            pt3Sheet = pt3Sheet.Add(moveToViewLocation);
                            pt2Sheet = pt2Sheet.Add(moveToViewLocation);
                            pt4Sheet = pt4Sheet.Add(moveToViewLocation);
                            //ElementTransformUtils.MoveElement(doc, cloudSheet.Id, vpCen - ptSheetMid);
                            //ElementTransformUtils.MoveElement(doc, cloudSheet.Id, new XYZ(-xOffset, -yOffset,0));



                            //CHECK
                            //VIEW AnnotationCropShape DIAGONAL
                            Line l = Line.CreateBound(pts[0], pts[3]);
                            doc.Create.NewDetailCurve(doc.ActiveView, l);

                            //BBOX CENTER TO VIEW CENTER
                            doc.Create.NewDetailCurve(doc.ActiveView, Line.CreateBound(viewCenter, bboxCenter));

                            // SHEET 0,0,0 TO VIEWPORT CENTER
                            doc.Create.NewDetailCurve(selectedViewSheet, Line.CreateBound(XYZ.Zero, new XYZ(changedVPcenter.X, changedVPcenter.Y, 0)));

                            //VIEWPORT CENTRE
                            //viewport.GetBoxCenter()
                            //doc.Create.NewDetailCurve(selectedViewSheet, Line.CreateBound(new XYZ(0,0,0), FlattenPoint(vpCen)));
                            //doc.Create.NewDetailCurve(selectedViewSheet, Line.CreateBound(new XYZ(0,0,0), FlattenPoint(viewport.GetBoxCenter())));

                            //INSIDE VIEW

                            List<Curve> curves = new List<Curve>();

                            curves.Add(Line.CreateBound(pt1, pt4));
                            curves.Add(Line.CreateBound(pt4, pt3));
                            curves.Add(Line.CreateBound(pt3, pt2));
                            curves.Add(Line.CreateBound(pt2, pt1));

                            RevisionCloud cloud = RevisionCloud.Create(doc, doc.ActiveView, rev[0], curves);


                            //ON SHEET
                            //MOVED OUTSIDE THE FOREACH LOOP SO COULDS WILL BE GROUPED TOGETHER
                            //List<Curve> curvesSheet = new List<Curve>();

                            curvesSheet.Add(Line.CreateBound(pt1Sheet, pt4Sheet));
                            curvesSheet.Add(Line.CreateBound(pt4Sheet, pt3Sheet));
                            curvesSheet.Add(Line.CreateBound(pt3Sheet, pt2Sheet));
                            curvesSheet.Add(Line.CreateBound(pt2Sheet, pt1Sheet));

                            //RevisionCloud cloudSheet = RevisionCloud.Create(doc, selectedViewSheet, rev[0], curvesSheet);

                            //MOVE THE SINGLE CLOUD AFTER CREATION
                            //ElementTransformUtils.MoveElement(doc, cloudSheet.Id, vpCen - ptSheetMid);
                            //ElementTransformUtils.MoveElement(doc, cloudSheet.Id, new XYZ(-xOffset, -yOffset,0));
                            t.Commit();
                        }

                        t.Start("Draw Clouds on Sheet");


                        RevisionCloud cloudSheet = RevisionCloud.Create(doc, selectedViewSheet, rev[0], curvesSheet);
                        t.Commit();
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", ex.Message);
                    }
                    finally
                    {

                    }

                    transGroup.Assimilate();
                }
            


        }

            

            return Result.Succeeded;

        }//close Execute

        public static XYZ ProjectOnto(Plane plane,  XYZ p)
        {
            double d = plane.SignedDistanceTo(p);

            //XYZ q = p + d * plane.Normal; // wrong according to Ruslan Hanza and Alexander Pekshev in their comments below

            XYZ q = p - d * plane.Normal;

            return q;
        }

        public XYZ FlattenPoint(XYZ point)
        {

            XYZ newPoint = new XYZ(point.X, point.Y, 0);

            return newPoint;
        }

        public void DrawRectangle(Document doc, View vpPlan, XYZ _max, XYZ _min)
        {

            XYZ min = new XYZ(_min.X, _min.Y, 0);
            XYZ max = new XYZ(_max.X, _max.Y, 0);

            Line l1 = Line.CreateBound(min, new XYZ(max.X, min.Y, 0));
            Line l2 = Line.CreateBound(min, new XYZ(min.X, max.Y, 0));
            Line l3 = Line.CreateBound(new XYZ(min.X, max.Y, 0), max);
            Line l4 = Line.CreateBound(max, new XYZ(max.X, min.Y, 0));
            Line l5 = Line.CreateBound(min, max);

            doc.Create.NewDetailCurve(vpPlan, l1);
            doc.Create.NewDetailCurve(vpPlan, l2);
            doc.Create.NewDetailCurve(vpPlan, l3);
            doc.Create.NewDetailCurve(vpPlan, l4);
            doc.Create.NewDetailCurve(vpPlan, l5);
        }
        public ViewSheet GetElementViewSheet(Document doc, View elementView)
        {
            ViewSheet viewSh = null;
            
            Parameter sheetNumber = elementView.LookupParameter("Sheet Number");

            IList<Element> sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToElements();

            for (int i = 0; i < sheets.Count(); i++)
            {
                ViewSheet sht = sheets[i] as ViewSheet;
                if (sht.SheetNumber.ToString() == sheetNumber.AsString())
                {
                    viewSh = sht;
                    break;
                }

            }
            return viewSh;
        }

        public static XYZ GetCentroid(List<XYZ> nodes, int count)
        {
            double x = 0, y = 0, area = 0, k;
            XYZ a, b = nodes[count - 1];

            for (int i = 0; i < count; i++)
            {
                a = nodes[i];

                k = a.Y * b.X - a.X * b.Y;
                area += k;
                x += (a.X + b.X) * k;
                y += (a.Y + b.Y) * k;

                b = a;
            }
            area *= 3;

            //return (area == 0) ? XYZ.Zero : new XYZ(x /= area, y /= area, nodes.First().Z);
            return (area == 0) ? XYZ.Zero : new XYZ(x /= area, y /= area, 0);
        }

    }
}
