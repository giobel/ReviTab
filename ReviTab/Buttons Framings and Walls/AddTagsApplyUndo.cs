using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;


namespace ReviTab
{

    [Transaction(TransactionMode.Manual)]
    public class AddTagsApplyUndo : IExternalCommand
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
                var myForm = new FormPlaceTags();
                myForm.Show();

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }


        }


    }






}
