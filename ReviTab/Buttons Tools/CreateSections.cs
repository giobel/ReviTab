using System.Collections.Generic;
using System.Linq;
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

            string  s = "";

            foreach (var e in r)
            {
                myElements.Add(doc.GetElement(e));
            }

            ViewSection vs = null;

            ICollection<ViewFamilyType> allSectionTypes = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().Where<ViewFamilyType>(x => ViewFamily.Section == x.ViewFamily).ToList();

            using (var form = new FormCreateSections())
            {

                form.SectionTypes = allSectionTypes;

                form.ShowDialog();

                if (form.DialogResult == winForm.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                ViewFamilyType vft = Helpers.viewFamilyType(doc);

                if (form.chosenSection != null)
                {
                    vft = form.chosenSection as ViewFamilyType;
                }

                using (Transaction tx = new Transaction(doc))
                {
                    try
                    {
                        tx.Start("Create Section");

                        if (form.sectionOrientation == "Long")

                            foreach (Element e in myElements)
                            {
                                vs = Helpers.CreateSectionParallel(doc, uidoc, e, form.sectionPositionOffset, form.farClipOffset, form.bottomLevel, form.topLevel, form.columnParameter, form.flipDirection, form.prefixText, vft);
                                s += $"{vs.Name}\n";
                            }

                        else
                        {
                            foreach (Element e in myElements)
                            {
                                Helpers.CreateSectionPerpendicular(doc, uidoc, e, form.columnParameter, form.prefixText);
                                s += 1;
                            }
                        }

                        

                        tx.Commit();
                    }
                    catch (System.Exception ex)
                    {

                        TaskDialog.Show("Error", ex.Message);
                    }
                }


            }//close form

            if (null != vs)
            {
                uidoc.ActiveView = vs;
            }


            TaskDialog.Show("Result", $"{s} Created");

            return Result.Succeeded;
        }
    }
}
