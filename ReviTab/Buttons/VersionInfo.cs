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
    public class VersionInfo : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {

            string date = "09/05/2019";


            
            TaskDialog.Show("Version Info", "Version 1.0.1 \nCompiled on " + date);




            return Result.Succeeded;
        }
    }
}

