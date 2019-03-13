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
    public class AddActiveViewToSheet : IExternalCommand
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

            using (var form = new FormAddActiveView("Enter Sheet Number"))
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



                        string sheetNumber = form.TextString.ToString();

                        ViewSheet viewSh = null;

                        FilteredElementCollector sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

                        foreach (ViewSheet sht in sheets)
                        {
                            if (sht.SheetNumber == sheetNumber)
                                viewSh = sht;
                        }

                        t.Start("Add view to sheet");

                        try
                        {
                            Viewport newvp = Viewport.Create(doc, viewSh.Id, activeView.Id, new XYZ(1.38, .974, 0));
                            interrupt = "True";
                            t.Commit();
                        }
                        catch
                        {
                            if (sheetNumber == "")
                            {
                                TaskDialog.Show("Warning", "Please enter a sheet number");
                                t.RollBack();
                                //                                form.ShowDialog();
                            }

                            else if (viewSh == null)
                            {
                                TaskDialog.Show("Warning", "The sheet number does not exist");
                                t.RollBack();
                                //                          form.ShowDialog();
                            }

                            else
                            {
                                TaskDialog.Show("Warning", "The view is already placed on another sheet");
                                t.RollBack();
                                //                      form.ShowDialog();
                            }
                        }//close catch

                    }//close while

                }

            }

            return Result.Succeeded;
        }
    }
}
