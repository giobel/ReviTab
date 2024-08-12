﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForms = System.Windows.Forms;
using System.Linq;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PrintInBackground : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;

            int check = 0;

            Document openDoc = null;

            using (var formOpen = new FormOpenFile())
            {

                formOpen.ShowDialog();

                string fileName = formOpen.filePath;

                ModelPath modelP = ModelPathUtils.ConvertUserVisiblePathToModelPath(fileName);

                OpenOptions optionDetach = new OpenOptions();

                optionDetach.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;

                openDoc = app.OpenDocumentFile(modelP, optionDetach);

                check += 1;

                if (formOpen.DialogResult == winForms.DialogResult.OK)
                {
                    
                    using (var form = new FormPickSheets())
                    {


                        List<ViewSheet> collector = Helpers.CollectViewSheet(openDoc);

                        foreach (ViewSheet vs in collector)
                        {
                            form.sheetNames.Add(vs.Name);
                            form.sheetNumbers.Add(vs.SheetNumber);
                        }

                        List<ViewSheetSet> allViewSheets = Helpers.CollectViewSheetSets(openDoc);

                        foreach (ViewSheetSet vs in allViewSheets)
                        {

                            form.dictSheetSetsNames.Add(vs.Name, Helpers.SheetInViewSheetsSets(vs));
                        }

                        Dictionary<string, PrintSetting> printSetting = Helpers.GetPrintersSettings(openDoc);

                        form.printSettings = printSetting.Keys.ToList();

                        form.ShowDialog();

                        if (form.DialogResult == winForms.DialogResult.Cancel)
                        {
                            return Result.Cancelled;
                        }

                        string destination = @"C:\Temp\";

                        //TaskDialog.Show("result", Helpers.CollectViewSheet(openDoc));

                        List<ViewSheet> sheetList = Helpers.FindViewSheetByNumber(openDoc, form.pickedNumbers);

                        PrintSetting chosenPrintSet = printSetting.Values.ElementAt(form.pickedPrintSet);

                        foreach (ViewSheet sheet in sheetList)
                        {
                            try
                            {
                                ViewSet vs = Helpers.CreateViewset(openDoc, sheet.SheetNumber);//create a viewset with each view to be printed (only way to be able to set the file names)
                            }
                            catch
                            {
                                TaskDialog.Show("Error", "Can't create ViewSet");
                            }
                            Helpers.PrintDrawingsFromList(openDoc, sheet, destination + form.prefix + sheet.SheetNumber + Helpers.SheetRevision(sheet) + ".pdf", chosenPrintSet);
                        }

                        openDoc.Close(false);
                        check += -1;

                        if (check == 0)
                            TaskDialog.Show("Result", "The pdfs will appear in the printer default folder (something like C:\\Users\\[your name]\\Documents)");
                        else
                            TaskDialog.Show("Result", "Something went wrong");
                    }
                }
            }


            return Result.Succeeded;
        }
    }
}
