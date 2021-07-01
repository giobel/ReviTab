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
                .Where(v => v.IsTemplate && v.HasDisplayStyle() == true);
                


                var form = new Forms.FormCopyViewFilter(doc);

                using (Transaction t = new Transaction(doc, "Copy View Template Filter"))
                {
                    form.ViewTemplateList = new List<View>(views.OrderBy(x => x.Name));                    
                    form.TargetTemplate = new List<View>(views.OrderBy(x => x.Name));

                    form.ShowDialog();

                    if (form.DialogResult == false)
                    {
                        return Result.Cancelled;
                    }


                    View sourceView = form.SelectedViewSource;

                    t.Start();

                    foreach (View targetView in form.SelectedTargetTemplate)
                    {
                    foreach (FilterElement selectedFilter in form.SelectedFilterElement)
                    {
                        if (!targetView.IsFilterApplied(selectedFilter.Id))
                        {                                                       
                            targetView.AddFilter(selectedFilter.Id);
                        }
                        bool visibility = sourceView.GetFilterVisibility(selectedFilter.Id);
                        OverrideGraphicSettings ogs = sourceView.GetFilterOverrides(selectedFilter.Id);
                        targetView.SetFilterOverrides(selectedFilter.Id, ogs);
                        targetView.SetFilterVisibility(selectedFilter.Id, visibility);
                    }                        
                    }
                                       

                    t.Commit();
                }


                return Result.Succeeded;

        }//close Execute
    }


}
