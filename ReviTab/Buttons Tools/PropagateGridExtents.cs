﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using forms = System.Windows.Forms;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PropagateGridExtents : IExternalCommand
    {
        /// <summary>
        /// Update the selected grid extents to match the grid extents is the source view
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            //StringBuilder sb = new StringBuilder();

            //Select the grid to update
            //Reference re = uidoc.Selection.PickObject(ObjectType.Element, "Select Grid");

            GenericSelectionFilter gridFilter = new GenericSelectionFilter("Grids");

            IList<Reference> selectedGrids = uidoc.Selection.PickObjects(ObjectType.Element, gridFilter, "Select Grids");

            //Grid extent to copy from
            View source = null;
            using (var form = new FormAddActiveView("Enter Source View Name"))
            {
                form.ShowDialog();
                //if the user hits cancel just drop out of macro
                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                string viewName = form.TextString.ToString();

                source = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).ToElements()
                .Where(x => x.Name == viewName).First() as View;

            }


            using (Transaction t = new Transaction(doc, "set grid"))
            {
                t.Start();

                foreach (Reference re in selectedGrids)
                {
                    try
                    {
                        CopyGridExtent(doc, source, re);
                    }
                    catch (Exception ex)
                    {
                        Grid grid = doc.GetElement(re) as Grid;
                        TaskDialog.Show("Error", $"Error with {grid.Name} \n{ex.Message}");
                    }

                }

                t.Commit();
            }





            return Result.Succeeded;
        }


        public bool CopyGridExtent(Document doc, View sourceView, Reference selectedGrid)
        {

            DatumPlane selectedDatum = doc.GetElement(selectedGrid) as DatumPlane;
            Curve baseCurve = selectedDatum.GetCurvesInView(DatumExtentType.ViewSpecific, sourceView).ElementAt(0);
            XYZ basePoint0 = baseCurve.GetEndPoint(0);
            XYZ basePoint1 = baseCurve.GetEndPoint(1);
            Line baseLine = baseCurve as Line;

            Curve newCurve = selectedDatum.GetCurvesInView(DatumExtentType.ViewSpecific, doc.ActiveView).ElementAt(0);
            XYZ newCurvePt = newCurve.GetEndPoint(0);
            Line newLine = newCurve as Line;

            //sb.AppendLine("Source view end0 " + baseLine.GetEndPoint(0).X.ToString() + " - " + baseLine.GetEndPoint(0).Y.ToString() + " - " + baseLine.GetEndPoint(0).Z.ToString());
            //sb.AppendLine("Active view end0 " + newLine.GetEndPoint(0).X.ToString() + " - " + newLine.GetEndPoint(0).Y.ToString() + " - " + newLine.GetEndPoint(0).Z.ToString());

            //sb.AppendLine("Source view end1 " + baseLine.GetEndPoint(1).X.ToString() + " - " + baseLine.GetEndPoint(1).Y.ToString() + " - " + baseLine.GetEndPoint(1).Z.ToString());
            //sb.AppendLine("Active view end1 " + newLine.GetEndPoint(1).X.ToString() + " - " + newLine.GetEndPoint(1).Y.ToString() + " - " + newLine.GetEndPoint(1).Z.ToString());

            ISet<ElementId> par = new List<ElementId>() as ISet<ElementId>;

            View destination = doc.ActiveView;

            ViewPlan vp = destination as ViewPlan;

            PlanViewRange pvr = vp.GetViewRange();

            Level l = vp.GenLevel;

            double zLevel = pvr.GetOffset(PlanViewPlane.CutPlane) + l.Elevation; //Z point for Datum curve

            //Curve projectedCurve = Line.CreateBound(new XYZ(basePoint0.X, basePoint0.Y, newCurvePt.Z), new XYZ(basePoint1.X, basePoint1.Y, newCurvePt.Z));

            Curve projectedCurve = Line.CreateBound(new XYZ(basePoint0.X, basePoint0.Y, zLevel), new XYZ(basePoint1.X, basePoint1.Y, zLevel));

            //par.Add(destination);

            //TaskDialog.Show("r", par.Count.ToString());


                Grid g = doc.GetElement(selectedGrid) as Grid;

                //g.SetDatumExtentType(DatumEnds.End1, destination, DatumExtentType.ViewSpecific);

                g.SetCurveInView(g.GetDatumExtentTypeInView(DatumEnds.End1, sourceView), destination, projectedCurve);

            //TaskDialog.Show("r", sb.ToString());
            //g.PropagateToViews(source, par);

            return true;
        }
    }
}
