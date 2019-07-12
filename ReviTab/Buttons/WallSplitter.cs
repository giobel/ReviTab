#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winforms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class WallSplitter : IExternalCommand
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
                
                Reference refWall = uidoc.Selection.PickObject(ObjectType.Element, "Select a wall");

                Element selectedWall = doc.GetElement(refWall);

                //top and bottom elevation
                ElementId wallTopLevel = selectedWall.LookupParameter("Top Constraint").AsElementId();

                double wallTopElevation = doc.GetElement(wallTopLevel).LookupParameter("Elevation").AsDouble();

                ElementId wallBottomLevel = selectedWall.LookupParameter("Base Constraint").AsElementId();

                double wallBottomElevation = doc.GetElement(wallBottomLevel).LookupParameter("Elevation").AsDouble();

                string wallMark = selectedWall.LookupParameter("Mark").AsString();
                //

                List<ElementId> allLevelsList = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().ToElementIds().ToList();

                IOrderedEnumerable<ElementId> levelByZ = from ElementId l in allLevelsList orderby doc.GetElement(l).LookupParameter("Elevation").AsDouble() ascending select l;

                IEnumerable<ElementId> topConstraintList = levelByZ.Where(eid => doc.GetElement(eid).LookupParameter("Elevation").AsDouble() <= wallTopElevation &&

                                                                          doc.GetElement(eid).LookupParameter("Elevation").AsDouble() > wallBottomElevation

                                                                         ).ToList();

                IEnumerable<ElementId> bottomConstraintList = levelByZ.Where(eid => doc.GetElement(eid).LookupParameter("Elevation").AsDouble() >= wallBottomElevation &&

                                                              doc.GetElement(eid).LookupParameter("Elevation").AsDouble() < wallTopElevation

                                                             ).ToList();

                using (var t = new Transaction(doc, "Split Wall"))
                {

                    t.Start();

                    FailureHandlingOptions failOpt = t.GetFailureHandlingOptions();

                    failOpt.SetFailuresPreprocessor(new DuplicateMarksSwallower());

                    t.SetFailureHandlingOptions(failOpt);

                    for (int i = 0; i < topConstraintList.Count(); i++)
                    {

                        ICollection<ElementId> newWallId = ElementTransformUtils.CopyElement(doc, selectedWall.Id, new XYZ(0, 0, 0));

                        Element newWall = doc.GetElement(newWallId.First());

                        ElementId topLevelId = topConstraintList.ElementAt(i);
                        ElementId bottomLevelId = bottomConstraintList.ElementAt(i);

                        newWall.LookupParameter("Top Constraint").Set(topLevelId);

                        newWall.LookupParameter("Base Constraint").Set(bottomLevelId);

                        newWall.LookupParameter("Mark").Set(wallMark);
                    }



                    doc.Delete(selectedWall.Id);

                    t.Commit();

                }


                string splittingLevels = "";

                foreach (ElementId eid in topConstraintList)
                {

                    splittingLevels += doc.GetElement(eid).Name + "\n";
                }

                TaskDialog.Show("Result", String.Format("The wall has been splitted in {0} parts at: \n{1}", topConstraintList.Count(), splittingLevels));

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }


        }


        public class DuplicateMarksSwallower : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor a)
            {
                // inside event handler, get all warnings

                IList<FailureMessageAccessor> failures
                  = a.GetFailureMessages();

                foreach (FailureMessageAccessor f in failures)
                {
                    // check failure definition ids
                    // against ones to dismiss:

                    FailureDefinitionId id
                      = f.GetFailureDefinitionId();

                    a.DeleteAllWarnings();

                    //if (BuiltInFailures.CreationFailures.CannotCreateCornice
                    //  == id)
                    //{
                    //    a.DeleteWarning(f);
                    //}
                }
                return FailureProcessingResult.Continue;
            }
        }

        #region General Warning Swallower
        FailureProcessingResult PreprocessFailures(FailuresAccessor a)
        {
            IList<FailureMessageAccessor> failures = a.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureSeverity fseverity = a.GetSeverity();

                if (fseverity == FailureSeverity.Warning)
                {
                    a.DeleteWarning(f);
                }
                else
                {
                    a.ResolveFailure(f);
                    return FailureProcessingResult.ProceedWithCommit;
                }
            }
            return FailureProcessingResult.Continue;
        }
        #endregion // General Warning Swallower

    }//close external command

}//close namespace