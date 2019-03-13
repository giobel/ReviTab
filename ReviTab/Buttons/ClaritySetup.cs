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
using PurgeUnused;

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
            int sheetsNumber = 0;
            int viewsNumber = 0;
            int schedulesNumber = 0;
            int furnitureElements = 0;

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

                    IEnumerable<ElementId> sheetIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Sheets) select elem.Id;

                    IEnumerable<ElementId> viewsIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Views) where elem.Name != "Clarity_IFC _3D" select elem.Id;

                    IEnumerable<ElementId> schedulesIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Schedules) select elem.Id;
                    

                    List<BuiltInCategory> builtInCats = new List<BuiltInCategory>();

                    builtInCats.Add(BuiltInCategory.OST_Furniture);
                    builtInCats.Add(BuiltInCategory.OST_Casework);
                    builtInCats.Add(BuiltInCategory.OST_Planting);
                    builtInCats.Add(BuiltInCategory.OST_SpecialityEquipment);
                    builtInCats.Add(BuiltInCategory.OST_Entourage);
                    builtInCats.Add(BuiltInCategory.OST_Railings);
                    builtInCats.Add(BuiltInCategory.OST_StairsRailing);
                    builtInCats.Add(BuiltInCategory.OST_MechanicalEquipment);

                    ElementMulticategoryFilter filter1 = new ElementMulticategoryFilter(builtInCats);


                    ICollection<ElementId> toDelete = new FilteredElementCollector(openDoc).WherePasses(filter1).ToElementIds();
             

                    View3D view3d = null;

                    using (Transaction tran = new Transaction(openDoc))
                    {
                        tran.Start("NewView3D");

                        try
                        {
                            view3d = View3D.CreateIsometric(openDoc, viewFamilyTypes.First().Id);
                            
                            view3d.Name = "Clarity_IFC _3D";

                            //uiapp.ActiveUIDocument.ActiveView = view3d;
                        }
                        catch(Exception ex)
                        {
                            
                            TaskDialog.Show("Error", "Name already taken" + "\n" + ex.Message);
                        }

                        tran.Commit();

                        tran.Start("Delete elements");
                        try
                        {
                            sheetsNumber += sheetIds.Count();

                            if (sheetIds.Count() > 0)
                            {
                                openDoc.Delete(sheetIds.ToList());
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Sheets Error", ex.Message);
                        }
                        try
                        {
                            viewsNumber += viewsIds.Count();

                            if (viewsIds.Count() > 0)
                            {
                                openDoc.Delete(viewsIds.ToList());
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Views Error", ex.Message);
                        }

                        try
                        {
                            if (schedulesIds.Count() > 0)
                            {
                                openDoc.Delete(schedulesIds.ToList());
                                schedulesNumber += schedulesIds.Count();
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Schedule Error", ex.Message);
                        }

                        int furnitureError = 0;

                        if (toDelete.Count() > 0)
                        {
                            foreach (ElementId id in toDelete)
                            {
                                try
                                {
                                    openDoc.Delete(id);
                                    furnitureElements += 1;
                                }
                                catch
                                {
                                    furnitureError += 1;
                                }
                            }
                            TaskDialog.Show("Error", String.Format("Cannot delete {0} furnitures", furnitureError));
                        }
                        

                        /*
                        ICollection<ElementId> purgeableElements = null;

                        if (PurgeTool.GetPurgeableElements(openDoc, ref purgeableElements) & purgeableElements.Count > 0)
                        {
                            openDoc.Delete(purgeableElements);
                        }*/

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
                    ICollection<ElementId> viewsId = new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Views).ToElementIds();

                    string viewsNames = "";

                    foreach (ElementId eid in viewsId)
                    {
                        viewsNames += openDoc.GetElement(eid).Name + Environment.NewLine;
                    }

                    TaskDialog.Show("Result", String.Format("Sheets deleted {0} \nViews deleted {1} \nSchedules deleted {2} \nViews in the model {3}",
                        
                        sheetsNumber, viewsNumber, furnitureElements, viewsNames));
                }
            }//close using

            return Result.Succeeded;

        }
    }//close class
}//close namespace
