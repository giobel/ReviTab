﻿using System;
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
    public class CreateViewSet : IExternalCommand
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

            using (var form = new FormCreateViewSet())
            {
                //use ShowDialog to show the form as a modal dialog box. 
                form.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (form.DialogResult == winForm.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                string text = "+viewset " + form.tBoxViewsetName + " " + form.tBoxSheetNumber;

                Helpers.CreateViewset(doc, text);

            }


            

            return Result.Succeeded;
        }
    }
}
