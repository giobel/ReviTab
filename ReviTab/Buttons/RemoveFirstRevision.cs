#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class RemoveFirstRevision : IExternalCommand
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

            try
            {
                using (Transaction t = new Transaction(doc, "Remove first revision"))
                {

                    t.Start();


                    List<string> parameters = new List<string>() { "Rev.", "Date", "Modeled By", "Approv.", "Checked", "Description" };

                    for (int i = 2; i < 11; i++)
                    {

                        foreach (string paramName in parameters)
                        {

                            Parameter pNew = vs.LookupParameter($"{i} - {paramName}");

                            Parameter pOld = vs.LookupParameter($"{i - 1} - {paramName}");

                            pOld.Set(pNew.AsString());
                        }

                    }
                    t.Commit();
                }


                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

        }

    }

}


