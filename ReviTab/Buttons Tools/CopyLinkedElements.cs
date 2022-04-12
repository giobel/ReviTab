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

            //TaskDialog.Show("Selected Elements", )


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

                        Dictionary<string, int> elementsSummary = new Dictionary<string, int>();

                        foreach (Reference elementRef in linkedModelRef)
                        {


                            Element eLinked = linkRvtDoc.GetElement(elementRef.LinkedElementId);

                            //reference are duplicated. avoid copying the same object twice
                            if (!eleToCopy.Contains(eLinked.Id))
                            {
                                string eLinkedCategoryName = eLinked.Category.Name;

                                if (elementsSummary.ContainsKey(eLinkedCategoryName))
                                {
                                    elementsSummary[eLinkedCategoryName]++;
                                }
                                else
                                {
                                    elementsSummary.Add(eLinked.Category.Name, 1);
                                }
                                eleToCopy.Add(eLinked.Id);

                            }
                        }

                        string result = "Selected elements:\n\n";


                        List<string> catNames = new List<string>();

                        foreach (var item in elementsSummary)
                        {
                            catNames.Add(item.Key.ToString());
                            result += $"{item.Key} : {item.Value}\n";
                        }


                        var form = new Forms.FormCopyLinkedElements(catNames);

                        form.ShowDialog();




                        TaskDialog td = new TaskDialog("Selected Elements");
                        td.MainContent = result;
                        td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,
                                           "Copy Elements");
                        td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2,
                                            "Cancel");


                        switch (td.Show())
                        {
                            case TaskDialogResult.CommandLink1:
                                // do the simple stuff
                                break;

                            case TaskDialogResult.CommandLink2:
                                throw new Exception("Command cancelled by Owen");
                                break;

                            default:
                                // handle any other case.
                                break;
                        }


                        ElementTransformUtils.CopyElements(linkRvtDoc, eleToCopy, doc, transf, copyPasteOption);

                        TaskDialog.Show("elements to copy", $"{eleToCopy.Count} elements have been copied from model {revitLinkInst.Name}");
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Warning", ex.Message);
                }

                finally
                {
                    t.Commit();
                }
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
