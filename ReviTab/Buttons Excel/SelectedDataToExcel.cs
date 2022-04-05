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
        public class SelectedDataToExcel : IExternalCommand
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

            try {

                StringBuilder sb = new StringBuilder();


                string inputFile = @"C:\Temp\RevitSettings.csv";

                Dictionary<string, List<string>> settings = Helpers.GetSettings(inputFile);


                ICollection<ElementId> selectedElementsId = uidoc.Selection.GetElementIds();

                TaskDialog.Show("r", selectedElementsId.Count.ToString() + " selected");

                uidoc.ActiveView = uidoc.ActiveGraphicalView;

                //keep track of the parameters names
                List<string> allParameters = new List<string>();

                string headers = "ElementId,";

                foreach (ElementId eid in selectedElementsId)
                {

                    Element e = doc.GetElement(eid);

                    string csvLine = "";

                    List<string> parameters = new List<string>();

                    if (settings.TryGetValue(e.Category.Name, out parameters))
                    {
                        if (e.Category.Name == "Text Notes")
                        {
                            TextElement textNote = e as TextElement;
                            string s = textNote.Text.TrimEnd('\r','\n');
                            csvLine += s + ",";
                        }


                        foreach (string s in parameters)
                        {
                            if (!allParameters.Contains(s))
                            {
                                headers += s.Trim() + ",";
                                allParameters.Add(s);
                            }
                                

                            string paramValue = "";

                            try
                            {

                                Parameter p = e.LookupParameter(s.Trim());

                                StorageType parameterType = p.StorageType;

                                
                                if (StorageType.Double == parameterType)
                                {
#if !REVIT2022
                                    paramValue = UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_MILLIMETERS).ToString();
#else

                                    paramValue = UnitUtils.ConvertFromInternalUnits(p.AsDouble(), UnitTypeId.Millimeters).ToString();
#endif

                                }
                                else if (p.Definition.Name == "Workset"){
                                    paramValue = p.AsValueString();
                                }
                                else if (StorageType.String == parameterType)
                                {
                                    paramValue = p.AsString();
                                }
                                else if (StorageType.Integer == parameterType)
                                {
                                    paramValue = p.AsInteger().ToString();
                                }
                                else if (StorageType.ElementId == parameterType)
                                {
                                    paramValue = p.AsValueString();
                                }

                            }
                            catch
                            {
                                paramValue = s;
                            }

                            csvLine += paramValue + ",";

                        }

                        sb.AppendLine(String.Format("{0},{1}", e.Id, csvLine));
                    }
                }

                string outputFile = @"C:\Temp\ExportedData.csv";


                //			var sortedList = selectedSheets.OrderBy(pd => pd.SheetNumber);



                File.WriteAllText(outputFile, headers + "\n");


                File.AppendAllText(outputFile, sb.ToString());

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = outputFile;
                process.Start();

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
