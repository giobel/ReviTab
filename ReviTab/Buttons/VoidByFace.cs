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

                using (var form = new FormPlaceFamilyByFace())
                {

                    form.ShowDialog();

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
                                if (Helpers.HasSymbolGeometry(r, doc) == 1)
                                {
                                    Helpers.PlaceOpeningSymbolGeometry(doc, r, Int16.Parse(s));
                                    count += 1;

                                }
                                else
                                {
                                    Helpers.PlaceOpeningSolidGeometry(doc, r, Int16.Parse(s));
                                    count += 1;
                                }
                            }
                                catch
                                {
                                    errors += 1;
                                }
                            }
                        }
                        t.Commit();
                    }//close transaction

                    TaskDialog.Show("result", "Void created: " + count.ToString() + "\n" + "Errors: " + errors.ToString());

                }//close form

            return Result.Succeeded;
        }
    }
}


