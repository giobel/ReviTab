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
//using PurgeUnused;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ClaritySetup : IExternalCommand
    {
        public static Document openDoc = null;
        public static int modifiedByDeleteMaterial = 0;
        public static bool checkForPurgeMaterials = false;
        public static string materialName = "";

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
            //int schedulesNumber = 0;
            int furnitureElements = 0;

            

            using (var formOpen = new FormOpenFile())
            {

                formOpen.ShowDialog();

                string fileName = formOpen.filePath;

                ModelPath modelP = ModelPathUtils.ConvertUserVisiblePathToModelPath(fileName);


                if (formOpen.DialogResult == winForms.DialogResult.OK)
                {
                    OpenOptions optionDetach = new OpenOptions();

                    optionDetach.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;

                    optionDetach.Audit = true;

                    openDoc = app.OpenDocumentFile(modelP, optionDetach);

                    IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(openDoc).OfClass(typeof(ViewFamilyType))
                                                                  let type = elem as ViewFamilyType
                                                                  where type.ViewFamily == ViewFamily.ThreeDimensional
                                                                  select type;

                    IEnumerable<ElementId> sheetIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Sheets) select elem.Id;

                    IEnumerable<ElementId> viewsIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Views) where elem.Name != "Clarity_IFC _3D" select elem.Id;

#if REVIT2019
                    IEnumerable<ElementId> schedulesIds = from elem in new FilteredElementCollector(openDoc).OfCategory(BuiltInCategory.OST_Schedules) select elem.Id;
#elif REVIT2017

#endif

                    List<BuiltInCategory> builtInCats = new List<BuiltInCategory>();

                    builtInCats.Add(BuiltInCategory.OST_Furniture);
                    builtInCats.Add(BuiltInCategory.OST_Casework);
                    builtInCats.Add(BuiltInCategory.OST_Planting);
                    builtInCats.Add(BuiltInCategory.OST_Entourage);
                    builtInCats.Add(BuiltInCategory.OST_Railings);
                    builtInCats.Add(BuiltInCategory.OST_StairsRailing);

                    ElementMulticategoryFilter filter1 = new ElementMulticategoryFilter(builtInCats);

                    
                    View3D view3d = null;

                    using (Transaction tran = new Transaction(openDoc))
                    {
                        tran.Start("Clarity Setup");
                        
                        FilteredWorksetCollector worksetCollector = new FilteredWorksetCollector(openDoc).OfKind(WorksetKind.UserWorkset);

                        try
                        {
                            view3d = View3D.CreateIsometric(openDoc, viewFamilyTypes.First().Id);
                            
                            view3d.Name = "Clarity_IFC _3D";

                            //uiapp.ActiveUIDocument.ActiveView = view3d;

                            foreach (Workset e in worksetCollector)
                            {

                                view3d.SetWorksetVisibility(e.Id, WorksetVisibility.Visible);

                            }
                        }
                        catch(Exception ex)
                        {
                            
                            TaskDialog.Show("Error", "Name already taken" + "\n" + ex.Message);
                        }


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
#if REVIT2019
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
        
#elif REVIT2017

#endif

                      if (formOpen.cleanArchModel)
                        {
                            int furnitureError = 0;

                            ICollection<ElementId> toDelete = new FilteredElementCollector(openDoc).WherePasses(filter1).ToElementIds();


                            if (toDelete.Count() > 0)
                            {
                                string lastEx = "";
                                foreach (ElementId id in toDelete)
                                {
                                    try
                                    {
                                        openDoc.Delete(id);
                                        furnitureElements += 1;
                                    }
                                    catch (Exception ex)
                                    {

                                        lastEx = $"{ex.Message}\n";
                                    }
                                }
                                //Debug.WriteLine(lastEx.Message);
                                TaskDialog.Show("Error", $"{furnitureElements} elements deleted. {furnitureError} cannot be deleted. Errors:\n{lastEx}");
                            }
                        }

                        if (formOpen.purgeModel)
                        {
                            ICollection<ElementId> purgeableElements = null;

                            PurgeTool.GetPurgeableElements(openDoc, ref purgeableElements);
                            try
                            {
                                while (purgeableElements.Count > 0)
                                {
                                    //TaskDialog.Show("Purge Count", purgeableElements.Count().ToString());
                                    PurgeTool.GetPurgeableElements(openDoc, ref purgeableElements);
                                    openDoc.Delete(purgeableElements);
                                }

                            }
                            catch (Exception ex)
                            {
                                TaskDialog.Show("Purge Error", ex.Message);
                            }
                        }

                        tran.Commit();
                    }

                    SaveAsOptions saveOpt = new SaveAsOptions();
                    WorksharingSaveAsOptions wos = new WorksharingSaveAsOptions();
                    wos.SaveAsCentral = true;
                    saveOpt.Compact = true;
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
                
                //PurgeMaterials(openDoc); too slooooooow

            }//close using

            return Result.Succeeded;

        }

        public void PurgeMaterials(Document doc)
        {
            Application app = doc.Application;
            app.DocumentChanged += documentChanged_PurgeMaterials;
            List<Element> materials = new FilteredElementCollector(doc).OfClass(typeof(Material)).ToList();
            string deletedMaterials = "";
            int unusedMaterialCount = 0;
            foreach (Element material in materials)
            {
                modifiedByDeleteMaterial = 0;
                materialName = material.Name + " (id " + material.Id + ")";
                using (TransactionGroup tg = new TransactionGroup(doc, "Delete Material: " + materialName))
                {
                    tg.Start();
                    using (Transaction t = new Transaction(doc, "delete material"))
                    {
                        t.Start();
                        checkForPurgeMaterials = true;
                        doc.Delete(material.Id);

                        // commit the transaction to trigger the DocumentChanged event
                        t.Commit();
                    }
                    checkForPurgeMaterials = false;

                    if (modifiedByDeleteMaterial == 1)
                    {
                        unusedMaterialCount++;
                        deletedMaterials += materialName + Environment.NewLine;
                        tg.Assimilate();
                    }
                    else // rollback the transaction group to undo the deletion
                        tg.RollBack();
                }
            }

            TaskDialog td = new TaskDialog("Info");
            td.MainInstruction = "Deleted " + unusedMaterialCount + " materials";
            td.MainContent = deletedMaterials;
            td.Show();

            app.DocumentChanged -= documentChanged_PurgeMaterials;
        }

        private static void documentChanged_PurgeMaterials(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            // do not check when rolling back the transaction group
            if (!checkForPurgeMaterials)
            {
                return;
            }

            List<ElementId> deleted = e.GetDeletedElementIds().ToList();
            List<ElementId> modified = e.GetModifiedElementIds().ToList();

            // for debugging
            string s = "";
            foreach (ElementId id in modified)
            {
                Element modifiedElement = openDoc.GetElement(id);
                s += modifiedElement.Category.Name + " " + modifiedElement.Name + " (" + id.IntegerValue + ")" + Environment.NewLine;
            }
            //TaskDialog.Show("d", materialName + Environment.NewLine + "Deleted = " + deleted.Count + ", Modified = " + modified.Count + Environment.NewLine + s);

            // how many elements were modified and deleted when this material was deleted?
            // if 1, then the material is unused and should be deleted
            modifiedByDeleteMaterial = deleted.Count + modified.Count;
        }
    }//close class
}//close namespace
