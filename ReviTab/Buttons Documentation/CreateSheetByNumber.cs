#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ReviTab.Forms;
using System;
using System.Collections.Generic;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class CreateSheetByNumber : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            
            Document document = uidoc.Document;
            

            using (var form = new FormCreateSheet())
            {

                form.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                FilteredElementCollector collector = new FilteredElementCollector(document);
                collector.OfClass(typeof(FamilySymbol));
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);

                FamilySymbol fs = collector.FirstElement() as FamilySymbol;
                if (fs != null)
                {
                    using (Transaction t = new Transaction(document, "Create a new ViewSheet"))
                    {
                        t.Start();

                        int sheetQuantity = form.Count;

                        for (int i = 0; i< sheetQuantity; i++)
                        {

                        
                        try
                        {
                            // Create a sheet view
                            ViewSheet viewSheet = ViewSheet.Create(document, fs.Id);

                                if (sheetQuantity == 1)
                                {
                                    viewSheet.SheetNumber = form.SheetNumber;
                                }
                                else
                                {
                                    viewSheet.SheetNumber = form.SheetNumber + i.ToString();
                                }


                            viewSheet.LookupParameter("Package").Set(form.PackageName);

                            if (null == viewSheet)
                            {
                                throw new Exception("Failed to create new ViewSheet.");
                            }
                        }
                        catch
                        {
                                TaskDialog.Show("Error")
                        }
                        }
                        t.Commit();
                    }
                }

            }

            return Result.Succeeded;
        }
    }
}
