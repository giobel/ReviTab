#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#endregion

namespace ReviTab
{
    class UpdateDataFromExcel
    {

        [Transaction(TransactionMode.Manual)]
        public class AlignViews : IExternalCommand
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

                try
                {
                    
                    string inputFile = @"C:\Temp\ExportedData.csv";

                    using (Transaction t = new Transaction(doc, "Update data"))
                    {

                        t.Start();

                        using (var reader = new StreamReader(inputFile))
                        {

                            string header = reader.ReadLine();

                            while (!reader.EndOfStream)
                            {

                                var line = reader.ReadLine();

                                var values = line.Split(',').ToList();

                                int id = Convert.ToInt32(values[0]);

                                ElementId currentId = new ElementId(id);

                                Viewport vport = doc.GetElement(currentId) as Viewport;
                                View view = doc.GetElement(vport.ViewId) as View;

                                view.LookupParameter("View Name").Set(values[7]);

                                ViewSheet vs = doc.GetElement(vport.SheetId) as ViewSheet;
                                vs.LookupParameter("Sheet Number").Set(values[1]);

                                vs.LookupParameter("Sheet Name").Set(values[2]);

                                vs.LookupParameter("ARUP_BDR_TITLE1").Set(values[3]);
                                vs.LookupParameter("ARUP_BDR_TITLE2").Set(values[4]);
                                vs.LookupParameter("ARUP_BDR_TITLE3").Set(values[5]);

                            }
                        }//close reader
                        t.Commit();
                    }//close transaction

                    TaskDialog.Show("l", "Done");


                    return Result.Succeeded;

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                    return Result.Failed;
                }
            }
        }

    }
}
