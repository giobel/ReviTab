using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Autodesk.Revit.UI.Selection;

namespace ReviTab
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ButtonPickPoint : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;

            XYZ selectedCenter = uiapp.ActiveUIDocument.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Pick the first view center point");

            Commands.GlobalObjects.Point = selectedCenter;
        }

        public string GetName()
        {
            return "External Event Example";
        }
        
    }
}
