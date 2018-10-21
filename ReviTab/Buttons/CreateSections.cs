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
    public class CreateSections : IExternalCommand
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


            IList<Reference> r = uidoc.Selection.PickObjects(ObjectType.Element, "Select elements");

            List<Element> myElements = new List<Element>();

            int s = 0;

            foreach (var e in r)
            {
                myElements.Add(doc.GetElement(e));
            }

            
            using (var form = new  FormCreateSections())
            {

                form.ShowDialog();

                if (form.DialogResult == winForm.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                using (Transaction tx = new Transaction(doc))
                {
                    try
                    {
                        tx.Start("Create Section");

                        if (form.sectionOrientation == "Long")

                            foreach (Element e in myElements)
                            {
                                Helpers.CreateSectionParallel(doc, uidoc, e, form.sectionPositionOffset, form.farClipOffset, form.bottomLevel, form.topLevel);
                                s += 1;
                            }

                        else
                        {
                            foreach (Element e in myElements)
                            {
                                Helpers.CreateSectionPerpendicular(doc, uidoc, e);
                                s += 1;
                            }
                        }

                        TaskDialog.Show("Result", s.ToString());

                        tx.Commit();
                    }
                    catch
                    {

                        TaskDialog.Show("Result", "Something went wrong");
                    }
                }


            }//close form

            return Result.Succeeded;
        }
    }
}
