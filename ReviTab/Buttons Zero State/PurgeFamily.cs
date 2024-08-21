using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class PurgeFamily : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;

            


            using (var formOpen = new FormOpenFile())
            {
                formOpen.ShowDialog();

                string[] filePath = System.IO.Directory.GetFiles(formOpen.filePath);

                foreach (string file in filePath)
                {
                    Document open_file = app.OpenDocumentFile(file);


                    FamilyManager fm = open_file.FamilyManager;


                    using (Transaction t = new Transaction(open_file, "Remove types"))
                    {
                        t.Start();

                        FamilyTypeSet familyTypes = fm.Types;

                        int count = familyTypes.Size;

                        while (count > 1)
                        {
                            fm.DeleteCurrentType();
                            count -= 1;
                        }

                        try
                        {
                            fm.RenameCurrentType("Default");
                        }
                        catch
                        {
                            //Do nothing
                        }


                        ICollection<ElementId> purgeableElements = null;

                        //if (PurgeTool.GetPurgeableElements(open_file, ref purgeableElements) & purgeableElements.Count > 0)
                        //{
                        //    open_file.Delete(purgeableElements);
                        //}


                        t.Commit();


                        /*
                        Press.Keys("PU");
                        winForm.SendKeys.SendWait("{ENTER}");

                        String s_commandToDisable = "ID_PURGE_UNUSED";
                        RevitCommandId s_commandId = RevitCommandId.LookupCommandId(s_commandToDisable);
                        uiapp.PostCommand(s_commandId);*/
                    }

                    //open_file.Save();
                    open_file.Close();
                }

            }
            
            return Result.Succeeded;


        }
    }
}
