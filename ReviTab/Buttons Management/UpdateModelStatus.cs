using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class UpdateModelStatus : IExternalCommand
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

            try
            {

                Dictionary<string, Helpers.CardContent> dashboardDictionary = Helpers.ModelStatus(doc);

                IEnumerable<Element> fecDashboardFamilies = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .Where(x => x.Name == "Single Value");

                IEnumerable<Element> fecDashboardDate = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .Where(x => x.Name == "Dashboard Date");


                using (Transaction t = new Transaction(doc, "Update Dashboard"))
                {

                    t.Start();

                    foreach (var dict in dashboardDictionary)
                    {
                        Element e = fecDashboardFamilies.Where(x => x.LookupParameter("Name").AsString() == dict.Key).First();
                        e.LookupParameter("Content").Set("N/A");

                        e.LookupParameter("Old Value").Set(e.LookupParameter("Current Value").AsInteger());

                        e.LookupParameter("Current Value").Set(dict.Value.Value);
                        e.LookupParameter("Content").Set(dict.Value.Content);
                    }


                    Element dateFamily = fecDashboardDate.First();

                    dateFamily.LookupParameter("Old Value").Set(dateFamily.LookupParameter("Current").AsString());
                    dateFamily.LookupParameter("Current").Set(DateTime.Now.ToShortDateString());


                    t.Commit();
                }

                TaskDialog.Show("Model Updated", "Done");

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);

                return Result.Failed;
            }
        }



    }

}