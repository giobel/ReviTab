#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class AddLegendToSheets : IExternalCommand
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
            View activeView = doc.ActiveView;

            try
            {
                using (var form = new FormAddActiveView("Enter Sheet Number"))
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        Reference legendRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a Legend");

                        form.ShowDialog();

                        //if the user hits cancel just drop out of macro
                        if (form.DialogResult == forms.DialogResult.Cancel)
                        {
                            return Result.Cancelled;
                        }

                        string sheetNumber = form.TextString.ToString();

                        List<ElementId> sheetIds = new List<ElementId>();

                        IEnumerable<ViewSheet> sheetItr = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToElements().Cast<ViewSheet>();

                        foreach (ViewSheet e in sheetItr)
                        {
                            if (sheetNumber.Contains(e.SheetNumber))
                                sheetIds.Add(e.Id);
                        }

                        

                        Viewport legendVp = doc.GetElement(legendRef) as Viewport;

                        ElementId legendId = legendVp.ViewId;

                        XYZ center = legendVp.GetBoxCenter();


                        // start the transaction  
                        t.Start("Add Legend");

                        // loop through the list of sheet ids  
                        foreach (ElementId sheetid in sheetIds)
                        {
                            Viewport.Create(doc, sheetid, legendId, center);
                        }

                        // commit the changes  
                        t.Commit();
                    }

                }

                return Result.Succeeded;
            }//close try

            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

        }
    }
}
