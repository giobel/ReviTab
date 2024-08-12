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
    public class SetTitleblockScale : IExternalCommand
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
                ViewSheet sheet = doc.ActiveView as ViewSheet;

                FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);

                Element sheetTitleblock = collector.ToElements().FirstOrDefault();

                //TaskDialog.Show("result", sheetTitleblock.Name);

                List<int> scaleValues = new List<int>();

                foreach (ElementId idVp in sheet.GetAllViewports())
                {
                    Viewport vp = doc.GetElement(idVp) as Viewport;

                    try
                    {
                        View view = doc.GetElement(vp.ViewId) as View;
                        if (view.ViewType != ViewType.Legend && view.ViewType != ViewType.ThreeD)
                        {
                            Parameter scale = vp.get_Parameter(BuiltInParameter.VIEW_SCALE);
                            scaleValues.Add(scale.AsInteger());
                        }
                    }
                    catch
                    {

                    }

                }

                //int scaleBarValue = scaleValues.GroupBy(x => x).First().First();
                var scaleBarValue = scaleValues.GroupBy(x => x)
                                      .Select(y => new { Element = y.Key, Counter = y.Count() })
                                      .ToList();

                int longestArray = scaleBarValue.OrderBy(x => x.Counter).Last().Element;

                using (Transaction t = new Transaction(doc, "Set scalebar"))
                {
                    t.Start();

                    //TaskDialog.Show("result", scaleBarValue.ToString());
                    Parameter tbScalebar = sheetTitleblock.LookupParameter("Scalebar scale");
                    tbScalebar.Set(longestArray);

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
