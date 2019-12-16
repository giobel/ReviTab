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
        public class UpdateViewportsFromExcel : IExternalCommand
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

            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WhereElementIsElementType();

            Dictionary<string, ElementId> viewTypes = new Dictionary<string, ElementId>();

            foreach (ElementType e in fec)
            {
                if (e.FamilyName == "Viewport")
                {
                    try
                    {
                        viewTypes.Add(e.Name, e.Id);
                    }
                    catch { }
                }
            }

            try
            {
                string inputFile = @"C:\Temp\ExportedData.csv";

                using (Transaction t = new Transaction(doc, "Update Viewports from Excel"))
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

                            //TaskDialog.Show("R", id.ToString());

                            ElementId currentId = new ElementId(id);

                            Viewport vp = doc.GetElement(currentId) as Viewport;

                            ElementId selectedType = null;
                            viewTypes.TryGetValue(values[3], out selectedType);

                            vp.LookupParameter("View Name").Set(values[2]); 
                            vp.ChangeTypeId(selectedType);

                            vp.SetBoxCenter(new XYZ(Convert.ToDouble(values[4]), Convert.ToDouble(values[5]), Convert.ToDouble(values[6])));


                        }

                    }//close reader
                    t.Commit();
                }//close transaction

                TaskDialog.Show("Result", "Done");

                return Result.Succeeded;
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

                #region Old Method
                //try
                //{
                //        ICollection<ElementId> selectedSheetsId = uidoc.Selection.GetElementIds();

                //        TaskDialog.Show("r", selectedSheetsId.Count.ToString() + " selected");

                //        ICollection<ViewSheet> selectedSheets = new List<ViewSheet>();

                //        foreach (var eid in selectedSheetsId)
                //        {
                //            selectedSheets.Add(doc.GetElement(eid) as ViewSheet);
                //        }

                //        uidoc.ActiveView = uidoc.ActiveGraphicalView;


                //        string outputFile = @"C:\Temp\ExportedData.csv";

                //        var sortedList = selectedSheets.OrderBy(pd => pd.SheetNumber);

                //        string headers = "ElementId, Sheet Number, Sheet Name, ARUP_BDR_TITLE1,ARUP_BDR_TITLE2,ARUP_BDR_TITLE3, View Type, View Name, View PosX, View PosY, View PosZ\n";

                //        StringBuilder sb = new StringBuilder();

                //        File.WriteAllText(outputFile, headers);


                //        foreach (ViewSheet vs in sortedList)
                //        {

                //            ICollection<ElementId> viewports = vs.GetAllViewports();
                //            foreach (var vpid in viewports)
                //            {
                //                Viewport vport = doc.GetElement(vpid) as Viewport;
                //                View view = doc.GetElement(vport.ViewId) as View;

                //                string viewId = vpid.ToString();
                //                string sheetNumber = vs.LookupParameter("Sheet Number").AsString();
                //                string sheetName = vs.LookupParameter("Sheet Name").AsString();
                //                string sheetTitle1 = vs.LookupParameter("ARUP_BDR_TITLE1").AsString();
                //                string sheetTitle2 = vs.LookupParameter("ARUP_BDR_TITLE2").AsString();
                //                string sheetTitle3 = vs.LookupParameter("ARUP_BDR_TITLE3").AsString();
                //                string viewType = view.ViewType.ToString();
                //                string viewName = view.Name;
                //                string viewPosition = vport.GetBoxCenter().ToString().Remove(0, 1).TrimEnd(')');
                //                sb.AppendLine(System.String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                //                                            viewId, sheetNumber, sheetName, sheetTitle1, sheetTitle2, sheetTitle3,
                //                                            viewType, viewName, viewPosition));

                //            }
                //        }
                //        File.AppendAllText(outputFile, sb.ToString());

                //        System.Diagnostics.Process process = new System.Diagnostics.Process();
                //        process.StartInfo.FileName = outputFile;
                //        process.Start();


                //        return Result.Succeeded;

                //    }
                //    catch (Exception ex)
                //    {
                //        TaskDialog.Show("Error", ex.Message);
                //        return Result.Failed;
                //    //}

                #endregion
            }
        }

    
}
