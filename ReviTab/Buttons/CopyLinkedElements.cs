using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class CopyLinkedElements : IExternalCommand
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
            
            IList<Reference> linkModelRefs = uidoc.Selection.PickObjects(ObjectType.LinkedElement, "Select Elements");

            //group selected elements by rvt link
            var refGroupByLinkModel = linkModelRefs.GroupBy(item => doc.GetElement(item).Id).Select(refs => refs.ToList());

            using (Transaction t = new Transaction(doc, "Copy Linked Elements"))
            {

                t.Start();

                try
                {

                    CopyPasteOptions copyPasteOption = new CopyPasteOptions();
                    copyPasteOption.SetDuplicateTypeNamesHandler(new CustomCopyHandler());

                    foreach (List<Reference> linkedModelRef in refGroupByLinkModel)
                    {


                        ICollection<ElementId> eleToCopy = new List<ElementId>();

                        Element e = doc.GetElement(linkedModelRef.First().ElementId);
                        RevitLinkInstance revitLinkInst = e as RevitLinkInstance;
                        Document linkRvtDoc = (e as RevitLinkInstance).GetLinkDocument();
                        Transform transf = revitLinkInst.GetTransform();

                        foreach (Reference elementRef in linkedModelRef)
                        {


                            Element eLinked = linkRvtDoc.GetElement(elementRef.LinkedElementId);
                            eleToCopy.Add(eLinked.Id);
                        }

                        ElementTransformUtils.CopyElements(linkRvtDoc, eleToCopy, doc, transf, copyPasteOption);

                        TaskDialog.Show("elements to copy", String.Format("{0} elements have been copied from model {1}", eleToCopy.Count, revitLinkInst.Name));
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("e", ex.Message);
                }

                t.Commit();
            }

            return Result.Succeeded;

        }//close Execute
    }

    internal class CustomCopyHandler : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}
