using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;
using PurgeUnused;
using System.IO;
using System.Linq;

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

            
            string filePath = doc.PathName;

            var fileInfo = new FileInfo(filePath);

            long fileSize = fileInfo.Length;

            FilteredElementCollector fecElements = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            FilteredElementCollector fecTypes = new FilteredElementCollector(doc).WhereElementIsElementType();

            FilteredElementCollector fecSheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();
            FilteredElementCollector fecViews = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();
            FilteredElementCollector fecViewPorts = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewports).WhereElementIsNotElementType();

            int countWarnings = doc.GetWarnings().Count;

            int countElements = fecElements.Count();
            int countTypes = fecTypes.Count();

            int countSheets = fecSheets.Count();
            int countViews = fecViews.Count();
            int countViewPorts = fecViewPorts.Count();


                if (Helpers.InsertData(DateTime.Now, Environment.UserName, fileSize, countElements, countTypes, countSheets, countViews, countViewPorts, countWarnings))
                {
                    TaskDialog.Show("result", fileSize.ToString() + "\n" + countWarnings.ToString());
                }
                
            return Result.Succeeded;
            }

            catch
            {
                return Result.Failed;
            }
        }
    }
}
