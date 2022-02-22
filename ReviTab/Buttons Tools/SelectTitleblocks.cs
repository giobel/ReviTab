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
    public class SelectTitleblocks : IExternalCommand
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

            ICollection<ElementId> selectedSheetsId = uidoc.Selection.GetElementIds();

            ICollection<ElementId> tblocksIds = new List<ElementId>();

            foreach (ElementId sheetId in selectedSheetsId)
            {
                FamilyInstance titleblock = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                .OfCategory(BuiltInCategory.OST_TitleBlocks).Cast<FamilyInstance>()
                                .First(q => q.OwnerViewId == sheetId);
                tblocksIds.Add(titleblock.Id);
            }

            uidoc.Selection.SetElementIds(tblocksIds);

            return Result.Succeeded;
        }
    }
}
