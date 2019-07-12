using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ReviTab
{

    [Transaction(TransactionMode.Manual)]
    public class UnjoinElements : IExternalCommand
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

            Reference firstElement = uidoc.Selection.PickObject(ObjectType.Element, "Select First Element");
            IList<Reference> selectedElements = uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements to be joined");

            int count = 0;

            using (Transaction t = new Transaction(doc, "Unjoin"))
            {

                t.Start();

                foreach (Reference eleRef in selectedElements)
                {

                    try
                    {
                        
                        JoinGeometryUtils.UnjoinGeometry(doc, doc.GetElement(firstElement), doc.GetElement(eleRef));
                        count += 1;
                    }
                    catch (Exception ex)
                    {
                        
                        TaskDialog.Show("Error", ex.Message);
                    }
                }


                t.Commit();
            }


            TaskDialog.Show("Result", String.Format("{0} have been unjoined", count));

            return Result.Succeeded;
        }
    }
}
