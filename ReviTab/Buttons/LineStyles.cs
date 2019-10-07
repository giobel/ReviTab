using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class LineStyles : IExternalCommand
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

                double width = 0.2; //feet 

                Category c = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                CategoryNameMap subcats = c.SubCategories;

                double offset = 0;

                TextNoteOptions options = new TextNoteOptions();
                options.HorizontalAlignment = HorizontalTextAlignment.Left;
                options.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                var dict = new SortedDictionary<string, GraphicsStyle>();

                foreach (Category lineStyle in subcats)
                {

                    GraphicsStyle gs = lineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);

                    dict.Add(gs.Name, gs);
                }


                var output = dict.OrderBy(e => e.Key).Select(e => new { graphicStyle = e.Value, linestyleName = e.Key }).ToList();


                using (Transaction t = new Transaction(doc, "Place Lines"))
                {

                    t.Start();

                    //foreach( Line item in ordered) {
                    foreach (var item in output)
                    {

                        //					GraphicsStyle gs = lineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);

                        XYZ newOrigin = new XYZ(origin.X, origin.Y + offset, 0);
                        XYZ offsetPoint = new XYZ(origin.X + width, origin.Y + offset, 0);

                        Line L1 = Line.CreateBound(newOrigin, offsetPoint);

                        try
                        {

                            TextNote note = TextNote.Create(doc, doc.ActiveView.Id, new XYZ(origin.X - 0.2, origin.Y + offset + 0.01, 0), 0.2, item.linestyleName, options);

                            DetailCurve e = doc.Create.NewDetailCurve(doc.ActiveView, L1);

                            Parameter p = e.LookupParameter("Line Style");

                            p.Set(item.graphicStyle.Id);

                        }
                        catch
                        {

                        }
                        offset -= 0.03;
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
