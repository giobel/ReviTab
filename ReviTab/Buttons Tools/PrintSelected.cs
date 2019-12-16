#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using forms = System.Windows.Forms;
using System.Linq;

#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PrintSelected : IExternalCommand
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

            ICollection<ElementId> selectedSheetsId = uidoc.Selection.GetElementIds();
            
            string viewSetName = app.Username + " Temp";
            ViewSet myViewSet = new ViewSet();

            FilteredElementIterator elemItr = new FilteredElementCollector(doc).OfClass(typeof(ViewSheetSet)).GetElementIterator();

            elemItr.Reset();

            Element existingViewSet = null;

            while (elemItr.MoveNext())
            {
                if (elemItr.Current.Name == viewSetName)
                    existingViewSet = elemItr.Current;
            }

            IEnumerable<ViewSheet> sheetItr = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToElements().Cast<ViewSheet>();

            foreach (ViewSheet e in sheetItr)
            {
                if (selectedSheetsId.Contains(e.Id))
                    myViewSet.Insert(e);
            }


            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create View Set");

                //If exists, delete existing viewset
                try
                {
                    doc.Delete(existingViewSet.Id);
                }
                catch
                {
                    //if the view set does not exists, don't crash
                }

                //Create the new viewset		
                PrintManager printMan = doc.PrintManager;
                printMan.PrintRange = PrintRange.Select;
                ViewSheetSetting viewSetting = printMan.ViewSheetSetting;
                viewSetting.CurrentViewSheetSet.Views = myViewSet;
                viewSetting.SaveAs(viewSetName);

                t.Commit();

            }

            //RevitCommandId id = RevitCommandId.LookupPostableCommandId(PostableCommand.SheetIssuesOrRevisions);

            //Jrn.RibbonEvent "Execute external command:CustomCtrl_%CustomCtrl_%CADtools%Publish%Batch" & vbCr & "Publish:Arup.CADtools.Revit.Commands.RevitPublishCmd"
            //vbCr = "\r"
            string name = "CustomCtrl_%CustomCtrl_%CADtools%Publish%Batch\rPublish";

            RevitCommandId id = RevitCommandId.LookupCommandId(name);
            uidoc.Application.PostCommand(id);


            return Result.Succeeded;

        }
    }
}