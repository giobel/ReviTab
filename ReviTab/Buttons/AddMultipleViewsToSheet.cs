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

            ViewSheet viewSh = null;

            FilteredElementCollector sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

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

                string output = "";

                int count = 1;

                XYZ selectedCenter = form.centerpoint;

                int space = form.Spacing;

                try
                {

                
                using (Transaction t = new Transaction(doc, "Add views"))
                {
                    if (form.isCenterpoint == true)
                        {
                            selectedCenter = uidoc.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Pick the first view center point");
                        }
                        
                    t.Start();


                        Line line1 = Line.CreateBound(XYZ.Zero, XYZ.BasisX);
                        DetailCurve dc1 = doc.Create.NewDetailCurve(doc.ActiveView, line1);
                        ElementId id = dc1.Id;
                        doc.Delete(id);



                        foreach (ElementId e in refe)
                    {
                        output += e.ToString() + "\n";
                        //Viewport vp = Viewport.Create(doc, viewSh.Id, e, new XYZ(1.38, .974, 0)); //this is the center of the sheet
                        Viewport vp = Viewport.Create(doc, viewSh.Id, e, selectedCenter);

                        Outline vpOutline = vp.GetBoxOutline();
                        double vpWidth = (vpOutline.MaximumPoint.X - vpOutline.MinimumPoint.X);
                        //XYZ newCenter = new XYZ((vp.GetBoxCenter().X + vpWidth / 2)+count*(vpWidth*2), .974, 0);
                        XYZ newCenter = new XYZ((selectedCenter.X + vpWidth / 2) + count * (space / 304.8), selectedCenter.Y, 0);

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
