#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class VoidByLine : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ISelectionFilter beamFilter = new BeamSelectionFilter("Structural Framing");
            ISelectionFilter lineFilter = new LineSelectionFilter();

            IList<Reference> refsBeams = uidoc.Selection.PickObjects(ObjectType.Element, beamFilter, "Select Beams");

            IList<Reference> refsLines = uidoc.Selection.PickObjects(ObjectType.Element, lineFilter, "Select Lines");

            int countRectangular = 0;
            int countCircular = 0;
            string errors = "";




            using (Transaction t = new Transaction(doc, "Place Opening"))
            {
                t.Start();


                foreach (Reference beamRef in refsBeams)
                {
                    try
                    {
                        //								List<double> distances = PlaceOpeningIntersect.IntersectionPoint(doc, beamRef, refsLines);

                        // dictionary of distances from start, penos [width, depth]
                        Dictionary<double, int[]> penoSizes = VoidByLineHelpers.IntersectionPoint(doc, beamRef, refsLines);

                        // remove beams without openings
                        if (penoSizes.Keys.Count > 0)
                        {

                            foreach (double d in penoSizes.Keys)
                            {

                                if (penoSizes[d][0] > 0)
                                {
                                    Helpers.PlaceOpening(doc, beamRef, Convert.ToInt16(d), "Web_Peno_R", "start", penoSizes[d][0], penoSizes[d][1]);
                                    countRectangular += 1;
                                }
                                else
                                {
                                    Helpers.PlaceOpening(doc, beamRef, Convert.ToInt16(d), "Web_Peno_C", "start", penoSizes[d][0], penoSizes[d][1]);
                                    countCircular += 1;
                                }
                            }
                        }
                        else if (penoSizes.Count == 0)
                        {
                            errors += beamRef.ElementId + Environment.NewLine;
                        }

                    }
                    catch
                    {
                        
                        TaskDialog.Show("Error", "Uh-oh something went wrong");
                    }
                }

                t.Commit();
            }//close transaction

            if (errors == "")
            {
                TaskDialog.Show("result", string.Format("{0} rectangular voids created \n {1} circular voids created", countRectangular, countCircular));
            }
            else
            {
                TaskDialog.Show("result", string.Format("{0} rectangular voids created \n{1} circular voids created \n" +
                            "Intersection not found between the lines and the beams Id:\n{2}" +
                            "Are they placed at the same level?", countRectangular, countCircular, errors));

            }


            return Result.Succeeded;
        }



    }

    /*
    public class BeamSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element e)
        {

            if (e.Category.Name == "Structural Framing")
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
    */



        /*

    public class LineSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element e)
        {

            if (e.Category.Name == "Lines")
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
    */
}

