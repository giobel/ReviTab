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
    public class MoveBeamEnd : IExternalCommand
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

            Reference refSource = uidoc.Selection.PickObject(ObjectType.Element, "Select Source Element");

            IList<Reference> refsToMove = uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements to be moved");

            using (Transaction t = new Transaction(doc))
            {

                t.Start("Move end point");

                foreach (Reference r in refsToMove)
                {
                    Helpers.MoveEnd(doc, r, refSource);
                }

                t.Commit();
            }


            return Result.Succeeded;
            

        }
    }
}
