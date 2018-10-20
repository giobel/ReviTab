using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;


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

            string fileName = @"\\global.arup.com\australasia\MEL\Projects\261000\261184-00 Shipwreck Coast Master\Record Copy\Outgoing\2018-10-15 PCB Revit model moved\261184-ARP-M3-S_R18-PCB.rvt";

            //Document openDoc = Application.OpenDocumentFile(fileName);

            ModelPath modelP = ModelPathUtils.ConvertUserVisiblePathToModelPath(fileName);

            OpenOptions optionDetach = new OpenOptions();

            optionDetach.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;

            Document openDoc = app.OpenDocumentFile(modelP, optionDetach);
            check += 1;

            string destination = @"C:\Temp\temp.pdf";

            TaskDialog.Show("result", Helpers.CollectViewSheet(openDoc));

            ViewSet vs = Helpers.CreateViewset(openDoc, "360 100 230");

            Helpers.PrintDrawingsFromList(openDoc, vs, destination);


            openDoc.Close(false);
            check += -1;


            TaskDialog.Show("Result", check.ToString());


            return Result.Succeeded;
        }
    }
}
