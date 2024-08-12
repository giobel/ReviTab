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

            List<string> sheetNumbers = new List<string>();
            foreach (UIView uiview in uidoc.GetOpenUIViews())
            {
                var openView = doc.GetElement(uiview.ViewId);
                if (openView is ViewSheet)
                {
                    ViewSheet vs = openView as ViewSheet;
                    sheetNumbers.Add(vs.SheetNumber.ToString());
                }
            }


            var form = new Forms.FormInputCombobox();
            
                using (Transaction t = new Transaction(doc))
                {

                    form.ViewSheetList = new List<string>(sheetNumbers.ToList());
                    // form.ViewSheetList = new List<string>() { "S001", "S002" };
                    //form.ViewTemplateList = new List<View>(views.OrderBy(x => x.Name));

                    form.ShowDialog();

                        //if the user hits cancel just drop out of macro
                        if (form.DialogResult == false)
                        {
                            return Result.Cancelled;
                        }

                        //string sheetNumber = form.TextString.ToString();
                        string sheetNumber = form.SelectedViewSheet;

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
                            t.Commit();
                            if (null != viewSh)
                                uidoc.ActiveView = viewSh;
                        }
                        catch (Exception ex)
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
                                //TaskDialog.Show("Warning", "The view is already placed on another sheet");
                                TaskDialog.Show("Warning", ex.Message);
                                t.RollBack();
                                //                      form.ShowDialog();
                            }
                        }//close catch



                

            }

            return Result.Succeeded;
        }
    }
}
