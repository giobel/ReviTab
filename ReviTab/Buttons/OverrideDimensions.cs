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
    public class OverrideDimensions : IExternalCommand
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

            ICollection<ElementId> r = uidoc.Selection.GetElementIds();

            Dimension dimension = null;


            foreach (var ele in r)
            {

                dimension = doc.GetElement(ele) as Dimension;

            }

            using (Transaction t = new Transaction(doc, "Override dimension"))
            {
                t.Start();
                try
                {
                    dimension.ValueOverride = " ";
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("error", ex.Message);
                }

                t.Commit();
            }


            return Result.Succeeded;
            
        }
    }
}
