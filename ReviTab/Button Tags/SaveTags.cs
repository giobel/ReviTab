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
        public class SaveTags : IExternalCommand
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

                ElementId defaultTextTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

            FamilySymbol tagLocationFamily = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).WhereElementIsElementType().Cast<FamilySymbol>().Where(x=>x.Name == "Tag Location").First();

            if (!tagLocationFamily.IsActive) {
                tagLocationFamily.Activate(); doc.Regenerate();
            }


            ICollection<Element> fecTagsElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                                                                .Where(c => c.Category !=null)
                                                                .Where(x => x.Category.Name.Contains("Tags")).ToList();

                ICollection<ElementId> allCreatedTextNotes = new List<ElementId>();
            using (Transaction t = new Transaction(doc, "Save Tags"))
                {
                    t.Start();
                    foreach (Element tagElement in fecTagsElements)
                    {
                        IndependentTag it = tagElement as IndependentTag;
                        XYZ pos = it.TagHeadPosition;
                    string content = $"{Math.Round(pos.X, 3)}\r{Math.Round(pos.Y, 3)}\r{Math.Round(pos.Z, 3)}";
                    FamilyInstance instance = doc.Create.NewFamilyInstance(pos, tagLocationFamily, doc.ActiveView);
                    instance.LookupParameter("Text Content").Set(content);

                }
                    t.Commit();
                }


            uidoc.Selection.SetElementIds(allCreatedTextNotes);

            return Result.Succeeded;
            }
        public static double SignedDistanceTo(Plane plane, XYZ p) { XYZ v = p - plane.Origin; return plane.Normal.DotProduct(v); }

    }
    
}
