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
        [Transaction(TransactionMode.Manual)]
        public class UpdateDataFromExcel : IExternalCommand
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

                using (Transaction t = new Transaction(doc, "Update Data from Excel"))
                {

                    t.Start();

                    using (var reader = new StreamReader(inputFile))
                    {

                        List<string> parameters = reader.ReadLine().Split(',').ToList();


                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();

                            var values = line.Split(',').ToList();

                            //TaskDialog.Show("R", values.Count.ToString());

                            int id = Convert.ToInt32(values[0]);

//                            TaskDialog.Show("R", id.ToString());

                            ElementId currentId = new ElementId(id);


                            //item 0 in elementId -> skip
                            for (int i = 1; i < parameters.Count; i++)
                            {


                                try
                                {

                                    Element e = doc.GetElement(currentId);

                                    Parameter p = e.LookupParameter(parameters[i].Trim());


                                    if (p.StorageType == StorageType.Integer)
                                    {
                                        p.Set(Convert.ToInt32(values[i]));
                                    }
                                    else
                                    {
                                        p.Set(values[i]);
                                    }
                                }
                                catch { 
                                    //TaskDialog.Show("Error", ex.Message); 
                                }

                            }

                        }
                    }//close reader
                    t.Commit();
                }//close transaction

                TaskDialog.Show("Result", "Done");

                return Result.Succeeded;
            }
            #region OLD Method
            //try
            //{

            //        string inputFile = @"C:\Temp\ExportedData.csv";

            //        using (Transaction t = new Transaction(doc, "Update data"))
            //        {

            //            t.Start();

            //            using (var reader = new StreamReader(inputFile))
            //            {

            //                string header = reader.ReadLine();

            //                while (!reader.EndOfStream)
            //                {

            //                    var line = reader.ReadLine();

            //                    var values = line.Split(',').ToList();

            //                    int id = Convert.ToInt32(values[0]);

            //                    ElementId currentId = new ElementId(id);

            //                    Viewport vport = doc.GetElement(currentId) as Viewport;
            //                    View view = doc.GetElement(vport.ViewId) as View;

            //                    view.LookupParameter("View Name").Set(values[7]);

            //                    ViewSheet vs = doc.GetElement(vport.SheetId) as ViewSheet;
            //                    vs.LookupParameter("Sheet Number").Set(values[1]);

            //                    vs.LookupParameter("Sheet Name").Set(values[2]);

            //                    vs.LookupParameter("ARUP_BDR_TITLE1").Set(values[3]);
            //                    vs.LookupParameter("ARUP_BDR_TITLE2").Set(values[4]);
            //                    vs.LookupParameter("ARUP_BDR_TITLE3").Set(values[5]);

            //                }
            //            }//close reader
            //            t.Commit();
            //        }//close transaction

            //        TaskDialog.Show("l", "Done");


            //        return Result.Succeeded;

            //    }
            #endregion
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
        

    }
}
