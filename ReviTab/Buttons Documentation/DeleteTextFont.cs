using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class DeleteTextFont : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                IList<Reference> textToDelete = uidoc.Selection.PickObjects(ObjectType.Element, "Select text to delete");

                ICollection<ElementId> textNoteTypes = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).ToElementIds();

                using (Transaction t = new Transaction(doc, "Place text"))
                {
                    t.Start();

                    foreach (Reference textReference in textToDelete)
                    {

                        TextNote textNoteElement = doc.GetElement(textReference) as TextNote;
                        ElementId eid = textNoteTypes.Where(x => x == textNoteElement.GetTypeId()).First();
                        doc.Delete(eid);
                    }
                    t.Commit();
                }

                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            
        }
    }

}
