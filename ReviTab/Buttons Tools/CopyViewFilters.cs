using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class CopyViewFilters : IExternalCommand
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

            //List of View Templates to copy the filter to
            IEnumerable<View> views = new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate);


            if (doc.ActiveView.ViewTemplateId != null)
            {

                var form = new Forms.FormCopyViewFilter(doc);

                using (Transaction t = new Transaction(doc, "Copy View Template Filter"))
                {
                    form.ViewTemplateList = new ObservableCollection<View>(views.OrderBy(x => x.Name));                    
                    form.TargetTemplate = new ObservableCollection<View>(views.OrderBy(x => x.Name));

                    form.ShowDialog();

                    if (form.DialogResult == false)
                    {
                        return Result.Cancelled;
                    }


                    View sourceView = form.SelectedViewSource;
                    FilterElement selectedFilter = form.SelectedFilterElement;
                    View targetView = form.SelectedTargetTemplate;

                    OverrideGraphicSettings ogs = sourceView.GetFilterOverrides(selectedFilter.Id);
                    bool visibility = sourceView.GetFilterVisibility(selectedFilter.Id);
                    t.Start();

                    targetView.AddFilter(selectedFilter.Id);
                    targetView.SetFilterOverrides(selectedFilter.Id, ogs);
                    targetView.SetFilterVisibility(selectedFilter.Id, visibility);

                    t.Commit();

                }

            }

            else
            {
                TaskDialog.Show("Error", "The current view does not have a View Template applied");
            }


                return Result.Succeeded;

        }//close Execute
    }


}
