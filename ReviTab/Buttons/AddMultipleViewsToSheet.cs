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

            using (var form = new FormAddActiveView())
            {
                form.ShowDialog();

                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                try
                {

                
            foreach (ViewSheet sht in sheets)
            {
                if (sht.SheetNumber == form.TextString)
                    viewSh = sht;
            }

            string output = "";

            using (Transaction t = new Transaction(doc, "Add views"))
            {

                t.Start();

                foreach (ElementId e in refe)
                {
                    output += e.ToString() + "\n";
                    Viewport newvp = Viewport.Create(doc, viewSh.Id, e, new XYZ(1.38, .974, 0));
                }


                t.Commit();
            }

            }
                catch
                {
                    TaskDialog.Show("Result", "Uh-oh something went wrong");
                }
                }
            
            return Result.Succeeded;

        }
    }
}
