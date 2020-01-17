using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AirtableApiClient;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PushToAirtable : IExternalCommand
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

            string configPath = @"C:\Temp\AirtableSettings.csv";

            try
            {
                Dictionary<string, Helpers.CardContent> dashboardDictionary = Helpers.ModelStatus(doc);

                List<string> apiAndKeys = Helpers.GetAirtableKeys(configPath);
                string baseId = apiAndKeys[0];
                string appKey = apiAndKeys[1];

                var fields = new Fields();

                foreach (var dict in dashboardDictionary)
                {
                    fields.AddField(dict.Key, dict.Value.Value);
                }

                AirtableBase airtableBase = new AirtableBase(appKey, baseId);
                
                    //var listRecords = GetRecords(airtableBase, "Model Status", records, errorMessage);
                    var create = Helpers.CreateRecord(airtableBase, "Model Status",fields);

                    Task.Run(() => create);
                    //Task.WaitAll(listRecords, create);
                    //Task.WaitAll(create);
                

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
