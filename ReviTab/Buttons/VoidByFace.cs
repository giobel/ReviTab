#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winforms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class VoidByFace : IExternalCommand
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


            IList<Reference> refs = uidoc.Selection.PickObjects(ObjectType.Element, "Select some beams");
            int count = 0;
            int errors = 0;

            using (var form = new FormAddOpening())
            {

                Dictionary<string, FamilySymbol> allCategories = Helpers.SelectFamilies(doc);



                foreach (string el in allCategories.Keys)
                {
                    form.familyName.Add(el);
                }


                form.ShowDialog();

                if (form.DialogResult == winforms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                string[] distance = form.distances.Split(' ');


                using (Transaction t = new Transaction(doc, "Place Opening"))
                {
                    t.Start();

                    foreach (string s in distance)
                    {
                        foreach (Reference r in refs)
                        {
                            try
                            {
                                Helpers.PlaceOpening(doc, r, Int16.Parse(s), form.choosenFamily, form.formPosition, form.formVoidWidth, form.formVoidHeight);
                                count += 1;
                            }
                            catch
                            {
                                errors += 1;
                                TaskDialog.Show("Error", "Uh-oh something went wrong");
                            }
                        }
                    }
                    t.Commit();

                }//close transaction


            }//close form
               
            return Result.Succeeded;
        }
    }
}


