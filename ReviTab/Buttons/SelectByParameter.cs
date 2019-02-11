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
    public class SelectByParameter : IExternalCommand
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

            using (var form = new FormAddActiveView())
            {
                using (Transaction t = new Transaction(doc))
                {

                    string interrupt = "False";

                    while (interrupt == "False")
                    {
                        //use ShowDialog to show the form as a modal dialog box. 
                        form.ShowDialog();

                        //if the user hits cancel just drop out of macro
                        if (form.DialogResult == forms.DialogResult.Cancel)
                        {
                            return Result.Cancelled;
                        }

                        t.Start("Select By Parameter");

                        string userMessage = form.TextString.ToString();

                        Helpers.SelectAllTypes(uidoc, userMessage);


                        t.Commit();
                       


                    }//close while

                }

            }

            return Result.Succeeded;
        }
    }
}