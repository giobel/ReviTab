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
    public class DeleteLineStyles : IExternalCommand
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
                IList<Reference> linesToDelete = uidoc.Selection.PickObjects(ObjectType.Element, "Select linestyles to delete");

                Category c = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                CategoryNameMap subcats = c.SubCategories;

                using (Transaction t = new Transaction(doc, "Place text"))
                {
                    t.Start();
           
                    foreach (Reference line in linesToDelete)
                    {
                        CurveElement curveEle = doc.GetElement(line) as CurveElement;

                        foreach (Category cat in subcats)
                        {
                            try
                            {

                            if (cat.Name == curveEle.LineStyle.Name)
                            {
                                doc.Delete(cat.Id);
                                break;
                            }

                            }
                            catch
                            {
                                //TaskDialog.Show("Error", "Error" + curveEle.LineStyle.Name);
                            }
                        }
                        //Category cat = subcats.Where( x => x.Name == curveEle.LineStyle.Name).First();

                        //doc.Delete(curveEle.LineStyle.Id);
                        //doc.Delete(line.ElementId);
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
