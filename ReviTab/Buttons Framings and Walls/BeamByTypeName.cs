#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class BeamByTypeName : IExternalCommand
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

            using (var form = new FormAddActiveView("Enter Beam Identity Type Mark"))
            {
                using (Transaction t = new Transaction(doc))
                {

                    Reference selectedBeam = uidoc.Selection.PickObject(ObjectType.Element, "Select a beam");

                    FamilyInstance beam = doc.GetElement(selectedBeam) as FamilyInstance;

                    string interrupt = "False";

                    while (interrupt == "False")
                    {
                        //use ShowDialog to show the form as a modal dialog box. 



                        form.ShowDialog();

                        //if the user hits cancel just drop out of macro
                        if (form.DialogResult == forms.DialogResult.Cancel)
                        {
                            return Result.Cancelled;
                        }

                        string beamTypeName = form.TextString.ToUpper().ToString();

                        FilteredElementCollector beamTypesCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).WhereElementIsElementType();



                        var wte = beamTypesCollector.FirstOrDefault((e) =>
                        {
                            if (string.Equals(e.Name.ToUpper(), beamTypeName.ToUpper()))
                                return true;
                            else
                            {
                                return false;
                            }
                            
                        });


                        ElementType et = wte as ElementType;


                        t.Start("Add view to sheet");

                        try
                        {
                            beam.ChangeTypeId(et.Id);
                            interrupt = "True";
                            t.Commit();
                        }
                        catch(Exception ex)
                        {
                            if (beamTypeName == "")
                            {
                                TaskDialog.Show("Warning", "Please enter an identity type mark");
                                t.RollBack();
                                //                                form.ShowDialog();
                            }

                            else if (beamTypeName == null)
                            {
                                TaskDialog.Show("Warning", "This identity type mark not exist");
                                t.RollBack();
                                //                          form.ShowDialog();
                            }

                            else
                            {
                                TaskDialog.Show("Warning", ex.Message);
                                t.RollBack();
                                //                      form.ShowDialog();
                            }
                        }//close catch

                    }//close while

                }

            }

            return Result.Succeeded;
        }
    }
}
