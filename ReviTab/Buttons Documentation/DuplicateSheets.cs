#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using forms = System.Windows.Forms;
using System.Linq;
using System.Collections.ObjectModel;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class DuplicateSheets : IExternalCommand
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

            var form = new Forms.FormDuplicateSheets();
            
                using (Transaction t = new Transaction(doc, "Duplicate Sheet"))
                {

                IEnumerable<ViewSheet> sheetsElement = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToElements().Cast<ViewSheet>();

                form.SheetsList = new ObservableCollection<ViewSheet>(sheetsElement.OrderBy(x=>x.SheetNumber));

                form.ShowDialog();
                               

                if (form.DialogResult == false)
                {
                    return Result.Cancelled;
                }

                t.Start();

                foreach (ViewSheet item in form.SelectedSheets)
                {
                    Helpers.DuplicateSheet(doc, item, form.textSuffix);

                    //TaskDialog.Show("r", item.Name+form.textSuffix);
                }

                t.Commit();

                }

           

            return Result.Succeeded;
        }
    }
}
