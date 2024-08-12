using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class MoveViewportToSheet : IExternalCommand
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

            
            ICollection<Reference> viewportsToMove = uidoc.Selection.PickObjects(ObjectType.Element, "Select viewports to move");


            FilteredElementCollector sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

            using (var form = new FormAddActiveView("Enter Sheet Number"))
            {
                form.ShowDialog();

                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                string sheetNumber = form.TextString.ToString();

                ViewSheet viewSh = null;

                foreach (ViewSheet sht in sheets)
                {
                    if (sht.SheetNumber == sheetNumber)
                    {
                        viewSh = sht;
                        break;
                    }
                }

                using (Transaction t = new Transaction(doc, "Move Viewports"))
                {
                    t.Start();

                    foreach (Reference selectedViewportId in viewportsToMove)
                    {
                        Viewport vp = doc.GetElement(selectedViewportId) as Viewport;
                        XYZ vpCenter = vp.GetBoxCenter();
                        ElementId vpId = vp.ViewId;
                        doc.Delete(vp.Id);
                        Viewport.Create(doc, viewSh.Id, vpId, vpCenter);
                    }

                    t.Commit();
                }

                if (viewSh != null)
                {
                    uidoc.ActiveView = viewSh;
                }
            }
            return Result.Succeeded;
        }
    }
}
