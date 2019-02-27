using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class EditBeamJoin : IExternalCommand
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



            ISelectionFilter beamFilter = new BeamSelectionFilter("Structural Framing");
            IList<Reference> refsBeams = uidoc.Selection.PickObjects(ObjectType.Element, beamFilter, "Select Beams");

            using (var form = new FormJoin())
            {


                form.ShowDialog();

                if (form.DialogResult == winForm.DialogResult.Cancel)
                {

                    return Result.Cancelled;
                }


                using (Transaction t = new Transaction(doc, "Edit Beam Join"))
                {

                    t.Start();

                    foreach (Reference r in refsBeams)
                    {

                        FamilyInstance ele = doc.GetElement(r) as FamilyInstance;

                        if (form.allowStartValue)
                        {
                            StructuralFramingUtils.AllowJoinAtEnd(ele, 0);
                        }

                        if (form.allowEndValue)
                        {
                            StructuralFramingUtils.AllowJoinAtEnd(ele, 1);
                        }

                        if (form.disallowStartValue)
                        {
                            StructuralFramingUtils.DisallowJoinAtEnd(ele, 0);
                        }

                        if (form.disallowEndValue)
                        {
                            StructuralFramingUtils.DisallowJoinAtEnd(ele, 1);
                        }

                        if (form.miterStart)
                        {
                            ele.ExtensionUtility.set_Extended(0, true);
                        }

                        if (form.miterEnd)
                        {
                            ele.ExtensionUtility.set_Extended(1, true);
                        }

                    }

                    t.Commit();
                }//close using

                return Result.Succeeded;
            }//close form using


        

    }

    }
}
