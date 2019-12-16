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
    public class TextFonts : IExternalCommand
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
                XYZ origin = uidoc.Selection.PickPoint("Select insertion point");

                double Yoffset = origin.Y;

                double width = 0.5; //feet 

                TextNoteOptions options = new TextNoteOptions();
                options.HorizontalAlignment = HorizontalTextAlignment.Left;
                options.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                ICollection<Element> textNoteTypes = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).OrderBy(x => x.Name).ToList();


                using (Transaction t = new Transaction(doc, "Place text"))
                {
                    t.Start();

                    foreach (Element e in textNoteTypes)
                    {


                        TextNoteType textNoteElement = doc.GetElement(e.Id) as TextNoteType;

                        double fontSize = Convert.ToDouble(textNoteElement.LookupParameter("Text Size").AsValueString().Replace("mm", "")) / 304.8;

                        double borderOffset = textNoteElement.LookupParameter("Leader/Border Offset").AsDouble();

                        XYZ offsetPoint = new XYZ(origin.X, Yoffset, 0);

                        TextNote note = TextNote.Create(doc, doc.ActiveView.Id, offsetPoint, width, textNoteElement.Name, options);

                        note.ChangeTypeId(e.Id);

                        Yoffset -= (fontSize + borderOffset * 2 + 0.03);
                    }

                    t.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

        }
    }

}
