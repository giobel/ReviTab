#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ReviTab.Forms;
using System;
using System.Collections.Generic;
using forms = System.Windows.Forms;
using System.Linq;
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

            List<string> packageValues = new List<string>();

            ICollection<Element> fecSheets = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElements();

            foreach (ViewSheet item in fecSheets)
            {
                packageValues.Add(item.Name);
                //string p = item.LookupParameter("Package").AsString();
                //if (null!=p && !packageValues.Contains(p))
                //{
                //    packageValues.Add(p);
                //}
            }

            //List<string> tblocksNames = new List<string>();

            ICollection<Element> fecTitleblocks = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType().ToElements();

            //foreach (Element item in fecTitleblocks)
            //{
            //    tblocksNames.Add(item.Name);
            //}


            using (var form = new FormCreateSheet())
            {
                form.Packages = packageValues;
                form.TitleblocksNames = fecTitleblocks;
                form.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                FilteredElementCollector collector = new FilteredElementCollector(document);
                collector.OfClass(typeof(FamilySymbol));
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);

                FamilySymbol fs = collector.Last() as FamilySymbol;

                if (form.ChosenTitleblock != null)
                {
                    fs = form.ChosenTitleblock as FamilySymbol;
                }


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
                        catch(Exception ex)
                        {
                                TaskDialog.Show("Error", ex.Message);
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
