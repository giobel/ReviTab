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
    public class ChangeBeamLocation : IExternalCommand
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

            ICollection<Reference> selectedLines = uidoc.Selection.PickObjects(ObjectType.Element, "Select Lines");

            ICollection<Reference> selectedBeams = uidoc.Selection.PickObjects(ObjectType.Element, "Select Beams");

            using (Transaction t = new Transaction(doc, "Change beams"))
            {

                t.Start();

                for (int i = 0; i < selectedBeams.Count; i++)
                {

                    Element ele = doc.GetElement(selectedBeams.ElementAt(i).ElementId);
                    Element newLine = doc.GetElement(selectedLines.ElementAt(i).ElementId);
                    (ele.Location as LocationCurve).Curve = (newLine.Location as LocationCurve).Curve;
                }
                t.Commit();
            }//close using

            return Result.Succeeded;
        }
    }
}
