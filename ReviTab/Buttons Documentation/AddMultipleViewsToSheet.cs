﻿#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;
using System.Linq;

#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class AddMultipleViewsToSheet : IExternalCommand
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



            ICollection<ElementId> refe = uidoc.Selection.GetElementIds();

            List<Element> sectionViews = new List<Element>();

            

            foreach (ElementId eid in refe)
            {
                sectionViews.Add(doc.GetElement(eid));
                
            }

            sectionViews = sectionViews.OrderBy(s => s.Name).ToList();

            ViewSheet viewSh = null;

            FilteredElementCollector sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

            if (refe.Count == 0)
            {
                TaskDialog.Show("Warning", "Please select some sheets in the Project Browser before launching the command. \nFor Revit <2019 make sure the Project" +
                    "Browser is docked.");
            }

            using (var form = new FormAddMultipleViews(uidoc))
            {
                form.ShowDialog();

                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                foreach (ViewSheet sht in sheets)
                {
                    if (sht.SheetNumber == form.SheetNumber)
                        viewSh = sht;
                }


                int count = 0;

                XYZ selectedCenter = form.centerpoint;

                int space = form.Spacing;


                try
                {

                    uidoc.ActiveView = viewSh;

                    using (Transaction t = new Transaction(doc, "Add views"))
                {
                    t.Start();
                        
                        //TaskDialog.Show("result", doc.ActiveView.Name);

                        //Line line1 = Line.CreateBound(XYZ.Zero, XYZ.BasisX);
                        //DetailCurve dc1 = doc.Create.NewDetailCurve(doc.ActiveView, line1) as DetailLine;
                        //ElementId id = dc1.Id;
                        //doc.Delete(id);

                        /*
                        XYZ rigth = doc.ActiveView.RightDirection;
                        XYZ up = doc.ActiveView.UpDirection;
                        XYZ origin = doc.ActiveView.Origin;

                        //Plane plane =  Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection,doc.ActiveView.Origin);
                        Plane plane = Plane.CreateByThreePoints(origin, rigth, up);

                        SketchPlane sp = SketchPlane.Create(doc, plane);
                        
                        doc.ActiveView.SketchPlane = sp;
                        */
                        if (form.isCenterpoint == true)
                        {
                            selectedCenter = uidoc.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Pick the first view center point");
                        }

                        foreach (Element e in sectionViews)
                    {
                        
                        //Viewport vp = Viewport.Create(doc, viewSh.Id, e, new XYZ(1.38, .974, 0)); //this is the center of the sheet
                        Viewport vp = Viewport.Create(doc, viewSh.Id, e.Id, selectedCenter);

                        Outline vpOutline = vp.GetBoxOutline();
                        double vpWidth = (vpOutline.MaximumPoint.X - vpOutline.MinimumPoint.X);
                        //XYZ newCenter = new XYZ((vp.GetBoxCenter().X + vpWidth / 2)+count*(vpWidth*2), .974, 0);
                        XYZ newCenter = new XYZ(selectedCenter.X + count * (space / 304.8), selectedCenter.Y, 0);

                        vp.SetBoxCenter(newCenter);
                        count += 1;
                    }


                    t.Commit();
                }

                }

                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                }
            }

            

            return Result.Succeeded;

        }
    }
}
