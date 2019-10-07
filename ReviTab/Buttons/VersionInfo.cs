#region Namespaces
using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class VersionInfo : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {

            //string date = "23/09/2019";

            string date = DateTime.Today.ToShortDateString();
            
            TaskDialog.Show("Version Info", "Version 1.0.6 \nCompiled on " + date);




            return Result.Succeeded;
        }
    }
}

