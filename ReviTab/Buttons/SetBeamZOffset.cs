#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class SetBeamZOffset : IExternalCommand
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
                ICollection<Reference> selectedBeamsId = uidoc.Selection.PickObjects(ObjectType.Element, "Select Beams");

                using (Transaction t = new Transaction(doc, "Change Z Offset"))
                {
                    t.Start();

                    foreach (Reference eid in selectedBeamsId)
                    {
                        Element beam = doc.GetElement(eid);

                        Parameter pStartOffset = beam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION);
                        Parameter pEndOffset = beam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION);
                        Parameter pZOffset = beam.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE);

                        pStartOffset.Set(pStartOffset.AsDouble() + pZOffset.AsDouble());
                        pEndOffset.Set(pEndOffset.AsDouble() + pZOffset.AsDouble());

                        pZOffset.Set(0);
                    }

                    t.Commit();
                }

                return Result.Succeeded;
            }
            catch(Exception ex) {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            }
    }
}
