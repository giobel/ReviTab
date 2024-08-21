using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PushToDB : IExternalCommand
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

                using (var form = new FormAddActiveView("Enter database table name"))
                {
                    form.ShowDialog();

                    if (form.DialogResult == winForm.DialogResult.Cancel)
                    {
                        return Result.Cancelled;
                    }
                    
                    string tableName = form.TextString;

                    string filePath = doc.PathName;

                    var fileInfo = new FileInfo(filePath);

                    long fileSize = fileInfo.Length;

                    FilteredElementCollector fecElements = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                    FilteredElementCollector fecTypes = new FilteredElementCollector(doc).WhereElementIsElementType();

                    FilteredElementCollector fecSheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();
                    FilteredElementCollector fecViews = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();
                    FilteredElementCollector fecViewPorts = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewports).WhereElementIsNotElementType();


#if REVIT2017
                    int countWarnings = 0;
#else
                    int countWarnings = doc.GetWarnings().Count;
#endif
                    int countElements = fecElements.Count();
                    int countTypes = fecTypes.Count();

                    int countSheets = fecSheets.Count();
                    int countViews = fecViews.Count();
                    int countViewPorts = fecViewPorts.Count();

                    int viewsNotOnSheet = Helpers.CountViewsNotOnSheet(fecViews).Count;

                    DateTime dateo = DateTime.Now;
                    string time = $"{dateo.Hour}h{dateo.Minute}m{dateo.Second}s";

                    string formatDate = $"{dateo.Year}{dateo.Month}{dateo.Day.ToString().PadLeft(2, '0')}_{time}";


                    string outputFile = $"{doc.ProjectInformation.BuildingName}\\{Environment.UserName}_{formatDate}.csv";
                    StringBuilder sb = new StringBuilder();
                    
                    if (Helpers.InsertData(tableName, DateTime.Now, Environment.UserName, 
                                           fileSize, countElements, countTypes, countSheets, 
                                           countViews, countViewPorts, countWarnings,Helpers.CountPurgeableElements(doc),viewsNotOnSheet))
                    {
                        //File.WriteAllText(outputFile, "Date," +
                        //        "Username," +
                        //        "Total Warnings, " +
                        //        "File Size, " +
                        //        "Purgeable Elements, " +
                        //        "Total Elements\n");

                        //sb.AppendLine($"{DateTime.Now},{Environment.UserName},{countWarnings},{fileSize},{Helpers.CountPurgeableElements(doc)},{countElements}");

                        //File.AppendAllText(outputFile, sb.ToString());

                        TaskDialog.Show("result", $"File size: {(fileSize/1000000).ToString("#.##")}Mb\nWarnings: {countWarnings}");
                    }

                    return Result.Succeeded;
                }

            }

            

            catch(Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}
