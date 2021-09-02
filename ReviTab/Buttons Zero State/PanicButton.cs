#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PanicButton : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			string date = DateTime.Today.ToShortDateString();

            TaskDialog.Show("R", "Doine");

			return Result.Succeeded;
        }
    }
}

