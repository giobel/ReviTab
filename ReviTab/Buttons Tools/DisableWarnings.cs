using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class DisableWarnings : IExternalCommand
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
                TaskDialog.Show("r", "Enabled");
                uiapp.Application.FailuresProcessing -= Application_FailuresProcessing;

            }
            catch (Exception ex)
            {
                TaskDialog.Show("catch", ex.ToString());

            }
            finally
            {

            }

            return Result.Succeeded;
        }//close Execute

        void Application_FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor fa = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> failList = new List<FailureMessageAccessor>();
            failList = fa.GetFailureMessages(); // Inside event handler, get all warnings
            foreach (FailureMessageAccessor failure in failList)
            {

                // check FailureDefinitionIds against ones that you want to dismiss, FailureDefinitionId failID = failure.GetFailureDefinitionId();
                // prevent Revit from showing Unenclosed room warnings
                FailureDefinitionId failID = failure.GetFailureDefinitionId();

                TaskDialog.Show("r", failID.Guid.ToString());

                //if (failID == BuiltInFailures.WorksharingFailures.DuplicateNamesChanged)
                //{
                fa.DeleteWarning(failure);
                //}
            }
        }
    }
}
