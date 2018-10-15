#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
#endregion

namespace RevitSamples.Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class ComnmandHelloWorld : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                // Begin Code Here
                MessageBox.Show("Hello World");
                // Return Success
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // In Case of a Failure
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
