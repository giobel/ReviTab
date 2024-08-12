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
    public class VoidByRefPlane : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ISelectionFilter beamFilter = new CategorySelectionFilter("Structural Framing");
            ISelectionFilter refPlaneFilter = new RefPlaneFilter();

            IList<Reference> refsBeams = uidoc.Selection.PickObjects(ObjectType.Element, beamFilter, "Select Beams");

            IList<Reference> refsLines = uidoc.Selection.PickObjects(ObjectType.Element, refPlaneFilter, "Select Reference Planes");

            Debug.WriteLine($"{refsBeams.Count} beams selected");

            Debug.WriteLine("Reference plances selected");

            int countRectangular = 0;
            int countCircular = 0;
            string errors = "";

            //revit family name
            string webPenoRectangular = "Web_Peno_Rectangular";
            string webPenoCircular = "Web_Peno_Circular";

            List<Dimension> lockDimensions = new List<Dimension>();

            using (Transaction t = new Transaction(doc, "Place Dummy Opening"))
            {
                t.Start();
            
                foreach (Reference beamRef in refsBeams)
                {
                    FamilyInstance tempPeno = Helpers.PlaceOpening(doc, beamRef, 0, webPenoRectangular, "mid", 100, 100);
                    doc.Delete(tempPeno.Id);
                }

                t.Commit();
            }

                using (Transaction t = new Transaction(doc, "Place Opening"))
            {
                t.Start();

                foreach (Reference beamRef in refsBeams)
                {
                    //List<double> distances = PlaceOpeningIntersect.IntersectionPoint(doc, beamRef, refsLines);

                    // dictionary of distances from start, penos [width, depth]

                    Debug.WriteLine($"***Beam reference {beamRef.ElementId}***" );

                    List<Tuple<double, int[], ReferencePlane>> penoSizes = VoidByLineHelpers.IntersectionLinePlane(doc, beamRef, refsLines);

                        foreach (var item in penoSizes)
                        {
                            try
                            {
                                // remove beams without openings
                                if (item.Item1 > 0)
                                {
                                double d = item.Item1;

                                Debug.WriteLine($"Distance from start: {d.ToString()}");

                                Debug.WriteLine($"Peno sizes: {item.Item2[0]}x{item.Item2[1]}");

                                Debug.WriteLine($"Reference plane Id: {item.Item3.Id}");

                                FamilyInstance openingInstance = null;

                                        if (item.Item2[0] > 0)
                                        {
                                            openingInstance = Helpers.PlaceOpening(doc, beamRef, Convert.ToInt16(d), webPenoRectangular, "start", item.Item2[0], item.Item2[1]);
                                    
                                    Debug.WriteLine($"Opening Instance id: {openingInstance.Id}");
                                    
                                    countRectangular += 1;
                                        }
                                        else
                                        {
                                            openingInstance = Helpers.PlaceOpening(doc, beamRef, Convert.ToInt16(d), webPenoCircular, "start", item.Item2[0], item.Item2[1]);
                                            countCircular += 1;
                                        }

                                //if (VoidByLineHelpers.IsParallel(openingInstance, item.Item3))
                                //{
                                //  VoidByLineHelpers.DrawDimension(doc, item.Item3, openingInstance, 0);
                                //}
                                FamilyInstance fi = openingInstance;

                                ReferencePlane rp = item.Item3;

                                Element ln = doc.GetElement(beamRef);

                                LocationCurve lc = ln.Location as LocationCurve;

#if REVIT2017

#endif
#if REVIT2020
                                IList<Reference> fir1 = fi.GetReferences(FamilyInstanceReferenceType.WeakReference);

                                ReferenceArray rea = new ReferenceArray();

                                rea.Append(fir1.First());
                                rea.Append(rp.GetReference());

                                Line lnie = lc.Curve as Line;

                                //Dimension align = doc.Create.NewAlignment(doc.ActiveView, rp.GetReference(), fir1.First());
                                lockDimensions.Add( doc.Create.NewDimension(doc.ActiveView, lnie, rea) );
                                //dim.IsLocked = true;
#endif

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


                    

                }

                t.Commit();
            }//close transaction

            using (Transaction t = new Transaction(doc, "Lock dimensions"))
            {
                t.Start();

                if (lockDimensions.Count > 0)
                {
                    foreach (Dimension d in lockDimensions)
                    {
                        d.IsLocked = true;
                    }

                }

                t.Commit();
            }


            if (errors == "")
            {
                TaskDialog.Show("result", string.Format("{0} rectangular voids created \n {1} circular voids created", countRectangular, countCircular));
            }
            else
            {
                TaskDialog.Show("result", string.Format("{0} rectangular voids created \n{1} circular voids created \n" +
                            "Intersection not found between the lines and the beams Id:\n{2}" +
                            "Is the Subcategory parameter empty? Are they placed at the same level?", countRectangular, countCircular, errors));

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

