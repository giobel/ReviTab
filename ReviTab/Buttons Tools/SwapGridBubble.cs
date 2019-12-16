using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;



namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class SwapGridBubbles : IExternalCommand
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


            try
            {

                ISelectionFilter beamFilter = new GridSelectionFilter();

                IList<Reference> selectedGrids = uidoc.Selection.PickObjects(ObjectType.Element, beamFilter, "Select Grids");


            using (Transaction t = new Transaction(doc, "Swap grid bubble"))
            {

                t.Start();


                foreach (Reference r in selectedGrids)
                {

                    Grid g = doc.GetElement(r.ElementId) as Grid;

                        try
                        {
                            if (g.IsBubbleVisibleInView(DatumEnds.End0, doc.ActiveView))
                            {
                                g.HideBubbleInView(DatumEnds.End0, doc.ActiveView);
                                g.ShowBubbleInView(DatumEnds.End1, doc.ActiveView);
                            }
                            else
                            {
                                g.HideBubbleInView(DatumEnds.End1, doc.ActiveView);
                                g.ShowBubbleInView(DatumEnds.End0, doc.ActiveView);
                            }

                        }

                        catch
                        {
                            TaskDialog.Show("Result", String.Format("Can't swap grid {0} head. The tool does not support multi-segmented grids", g.Name));
                        }

                }

                t.Commit();


            }

            return Result.Succeeded;
            }
            catch
            {
                TaskDialog.Show("Result", "Result failed");

                return Result.Failed;
            }

        }
    }

    public class GridSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element e)
        {

            if (e.Category.Name == "Grids")
            {
                return true;
            }
            return false;
        }


        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }

    }//close class
}
