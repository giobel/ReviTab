#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class UpRevSheet : IExternalCommand
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
            View activeView = doc.ActiveView;

            ViewSheet vs = doc.ActiveView as ViewSheet;

            string inputFile = @"C:\Temp\UpRevParameters.csv";
            List<string> parameters = new List<string>();

            using (var reader = new System.IO.StreamReader(inputFile))
            {
                parameters = reader.ReadLine().Split(',').ToList();
            }

            string borderRevision = parameters[0].Trim();
            string revName = parameters[1].Trim();
            string dateName = parameters[2].Trim();
            int counter = int.Parse(parameters[3].Trim());
            parameters.RemoveRange(0, 4);
            ExpandoObject eo = Helpers.FindLatestRevisioneExpando(vs, revName, dateName, counter, parameters);

            //Helpers.RevisionObj revObj = Helpers.FindLatestRevision(vs, "REV", "DATE", "DESC", "DES", "CHK", "APR", 4);
            //Helpers.RevisionObj revObj = Helpers.FindLatestRevision(vs, parameters[0].Trim(), parameters[1].Trim(), parameters[2].Trim(), parameters[3].Trim(), parameters[4].Trim(), parameters[5].Trim(), int.Parse(parameters[7].Trim()));

            Helpers.UpRevSheetExpando(doc, vs, eo, borderRevision, revName, parameters);


            return Result.Succeeded;
        }

    }

}


