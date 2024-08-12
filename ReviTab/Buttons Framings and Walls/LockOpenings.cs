using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;

namespace ReviTab
{

    [Transaction(TransactionMode.Manual)]
    public class LockOpenings : IExternalCommand
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
            View activeView = doc.ActiveView;

            ISelectionFilter openingFilter = new CategorySelectionFilter("Structural Connections");
            ISelectionFilter refPlanesFilter = new CategorySelectionFilter("Reference Planes");

            IList<Reference> openings = uidoc.Selection.PickObjects(ObjectType.Element, openingFilter, "Select openings");

            List<FamilyInstance> fa = new List<FamilyInstance>();
            
            foreach (Reference refe in openings)
            {
                FamilyInstance fi = doc.GetElement(refe) as FamilyInstance;
                fa.Add(fi);
            }

            var grouped = fa.GroupBy(x => x.Host.Id);

            double offset = 315 / 304.8;

            //Reference refPlaneLine = uidoc.Selection.PickObjects(ObjectType.Element,refPlanesFilter, "Select a ref plane").First();
            IList<Reference> refPlaneLine = uidoc.Selection.PickObjects(ObjectType.Element, refPlanesFilter, "Select a ref plane");

            var enu = grouped.GetEnumerator();

            using (Transaction t = new Transaction(doc, "Lock dim"))
            {
                t.Start();

                while (enu.MoveNext())
                {
                    foreach (FamilyInstance item in enu.Current)
                    {
                        offset += 315 / 304.8;

                        foreach (Reference refPlaneReference in refPlaneLine)
                        {
                            ReferencePlane refP = doc.GetElement(refPlaneReference) as ReferencePlane;

#if REVIT2019
                            if (VoidByLineHelpers.IsParallel(item, refP))
                            {
                                VoidByLineHelpers.DrawDimension(doc, refPlaneReference, item, offset);
                            }


#elif REVIT2017

#endif

                        }
                    }
                    offset = 0;
                }
                
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
