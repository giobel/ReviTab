#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using winforms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ColumnSplitter : IExternalCommand
    {
        #region General Warning Swallower
        public class DuplicateMarkSwallower : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor a)
            {
                var failures = a.GetFailureMessages();
                foreach (var f in failures)
                {
                    var id = f.GetFailureDefinitionId();
                    if (BuiltInFailures.GeneralFailures.DuplicateValue == id)
                    {
                        a.DeleteWarning(f);
                    }
                }
                return FailureProcessingResult.Continue;
            }
        }
        #endregion // DuplicateMarkSwallower

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
                IList<Reference> refColumn = uidoc.Selection.PickObjects(ObjectType.Element, "Select columns");

                int count = 0;

                foreach (Reference reference in refColumn)
                {

                    Element selectedColumn = doc.GetElement(reference);

                    //top and bottom elevation
                    ElementId columnTopLevel = selectedColumn.LookupParameter("Top Level").AsElementId();

                    double columnTopElevation = doc.GetElement(columnTopLevel).LookupParameter("Elevation").AsDouble();

                    ElementId columnBottomLevel = selectedColumn.LookupParameter("Base Level").AsElementId();

                    double columnBottomElevation = doc.GetElement(columnBottomLevel).LookupParameter("Elevation").AsDouble();

                    string markValue = selectedColumn.LookupParameter("Mark").AsString();

                    List<ElementId> allLevelsList = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().ToElementIds().ToList();

                    IOrderedEnumerable<ElementId> levelByZ = from ElementId l in allLevelsList orderby doc.GetElement(l).LookupParameter("Elevation").AsDouble() ascending select l;

                    IEnumerable<ElementId> topConstraintList = levelByZ.Where(eid => doc.GetElement(eid).LookupParameter("Elevation").AsDouble() <= columnTopElevation &&

                                                                              doc.GetElement(eid).LookupParameter("Elevation").AsDouble() > columnBottomElevation

                                                                             ).ToList();

                    IEnumerable<ElementId> bottomConstraintList = levelByZ.Where(eid => doc.GetElement(eid).LookupParameter("Elevation").AsDouble() >= columnBottomElevation &&

                                                                  doc.GetElement(eid).LookupParameter("Elevation").AsDouble() < columnTopElevation

                                                                 ).ToList();

                    using (var t = new Transaction(doc, "Split Column"))
                    {

                        t.Start();

                        // Set failure handler
                        var failureOptions = t.GetFailureHandlingOptions();
                        failureOptions.SetFailuresPreprocessor(new DuplicateMarkSwallower());
                        t.SetFailureHandlingOptions(failureOptions);

                        for (int i = 0; i < topConstraintList.Count(); i++)
                        {

                            ICollection<ElementId> newWallId = ElementTransformUtils.CopyElement(doc, selectedColumn.Id, new XYZ(0, 0, 0));

                            Element newColumn = doc.GetElement(newWallId.First());

                            ElementId topLevelId = topConstraintList.ElementAt(i);
                            ElementId bottomLevelId = bottomConstraintList.ElementAt(i);

                            newColumn.LookupParameter("Top Level").Set(topLevelId);

                            newColumn.LookupParameter("Base Level").Set(bottomLevelId);

                            newColumn.LookupParameter("Mark").Set(markValue);
                        }



                        doc.Delete(selectedColumn.Id);

                        t.Commit();

                    }


                    string splittingLevels = "";

                    foreach (ElementId eid in topConstraintList)
                    {

                        splittingLevels += doc.GetElement(eid).Name + "\n";
                    }

                    count += 1;
                }

                TaskDialog.Show("Result", String.Format("{0} columns have been splitted", count));

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }


        }
        
    }

}