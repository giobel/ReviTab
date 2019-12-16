using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;
using winForms = System.Windows.Forms;
using System.Linq;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
        public class SuppressWarnings : IExternalCommand
    {
        #region General Warning Swallower
        FailureProcessingResult PreprocessFailures(
          FailuresAccessor a)
        {
            IList<FailureMessageAccessor> failures
            = a.GetFailureMessages();

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


        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;

            

            //app.FailuresProcessing += App_FailuresProcessing;

            try
            {
                uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(Uiapp_DialogBoxShowing);

                string[] filePath = System.IO.Directory.GetFiles(@"C:\Users\giovanni.brogiolo\Documents\Animation");

                ModelPath modelP = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath[0]);

                OpenOptions optionDetach = new OpenOptions();

                optionDetach.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;

                Document openDoc = app.OpenDocumentFile(modelP, optionDetach);
                
                FilteredElementCollector fec = new FilteredElementCollector(openDoc).OfClass(typeof(FamilyInstance));

                winForms.MessageBox.Show(fec.Count().ToString());

                openDoc.Close(false);

                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                TaskDialog.Show("catch", ex.ToString());
                return Result.Failed;
            }
            finally
            {
                // app.FailuresProcessing -= App_FailuresProcessing;
                uiapp.DialogBoxShowing -= Uiapp_DialogBoxShowing;
                TaskDialog.Show("Result", "Done");
            }



        }

        private void Uiapp_DialogBoxShowing(object sender, DialogBoxShowingEventArgs e)
        {
            TaskDialogShowingEventArgs e2 = e as TaskDialogShowingEventArgs;

            string s = string.Empty;

            bool isConfirm = false;
            int dialogResult = 0;

            if (e2.DialogId.Equals("TaskDialog_Missing_Third_Party_Updaters"))
            {
                isConfirm = true;
                dialogResult = (int)TaskDialogResult.CommandLink1;
            }

            if (isConfirm)
            {
                e2.OverrideResult(dialogResult);
                s += ", auto-confirmed.";
            }
            else
            {
                s = string.Format(
                ", dialog id {0}, message '{1}'",
                e2.DialogId, e2.Message);
                winForms.MessageBox.Show(s);
            }
        }

        private void App_FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor fa = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> failList = new List<FailureMessageAccessor>();
            failList = fa.GetFailureMessages(); // Inside event handler, get all warnings
            foreach (FailureMessageAccessor failure in failList)
            {
                fa.DeleteAllWarnings();
                    /*
                // check FailureDefinitionIds against ones that you want to dismiss, FailureDefinitionId failID = failure.GetFailureDefinitionId();
                // prevent Revit from showing Unenclosed room warnings
                FailureDefinitionId failID = failure.GetFailureDefinitionId();
                if (failID == BuiltInFailures.WorksharingFailures.DuplicateNamesChanged)
                {
                    fa.DeleteWarning(failure);
                }*/
            }
        }
    }
}
