#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class IsolateCategories : IExternalCommand
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

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Select a detail Item");

            ICollection<ElementId> isolateElements = new List<ElementId>();

            List<string> categoriesToIsolate = new List<string>();

            foreach (Reference eleRef in selectedElementRefList)
            {
                string catName = doc.GetElement(eleRef).Category.Name;
                if (!categoriesToIsolate.Contains(catName))
                    categoriesToIsolate.Add(catName);
            }

            ICollection<Element> fec = new FilteredElementCollector(doc, doc.ActiveView.Id).WhereElementIsNotElementType().ToElements();

            foreach (Element element in fec)
            {
                if (element.Category != null && categoriesToIsolate.Contains(element.Category.Name))
                {
                    isolateElements.Add(element.Id);
                }
            }

            uidoc.Selection.SetElementIds(isolateElements);

            return Result.Succeeded;
        }
    }
}


 

