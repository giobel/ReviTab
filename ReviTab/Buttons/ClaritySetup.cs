using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForms = System.Windows.Forms;
using System.Linq;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ClaritySetup : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;

            int check = 0;

            Document openDoc = null;

            using (var formOpen = new FormOpenFile())
            {

                formOpen.ShowDialog();

                string fileName = formOpen.filePath;

                ModelPath modelP = ModelPathUtils.ConvertUserVisiblePathToModelPath(fileName);


                if (formOpen.DialogResult == winForms.DialogResult.OK)
                {
                    OpenOptions optionDetach = new OpenOptions();

                    optionDetach.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;

                    openDoc = app.OpenDocumentFile(modelP, optionDetach);

                    IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(openDoc).OfClass(typeof(ViewFamilyType))
                                                                  let type = elem as ViewFamilyType
                                                                  where type.ViewFamily == ViewFamily.ThreeDimensional
                                                                  select type;

                    View3D view3d = null;

                    using (Transaction tran = new Transaction(openDoc, "NewView3D"))
                    {
                        tran.Start();

                        view3d = View3D.CreateIsometric(openDoc, viewFamilyTypes.First().Id);
                        view3d.Name = "Clarity_IFC _3D";

                        tran.Commit();
                    }

                    SaveAsOptions saveOpt = new SaveAsOptions();
                    WorksharingSaveAsOptions wos = new WorksharingSaveAsOptions();
                    wos.SaveAsCentral = true;
                    saveOpt.SetWorksharingOptions(wos);
                    saveOpt.OverwriteExistingFile = true;

                    openDoc.SaveAs(modelP, saveOpt);


                    check += 1;

                }
                else
                {
                    TaskDialog.Show("Result", "Command aborted.");
                }
                    
                if (check > 0)
                {
                    TaskDialog.Show("Result", "Clarity_IFC _3D view created.");
                }
            }//close using

            return Result.Succeeded;

        }
    }//close class
}//close namespace
