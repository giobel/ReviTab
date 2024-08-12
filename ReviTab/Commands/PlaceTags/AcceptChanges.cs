using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ReviTab
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ButtonAcceptChanges : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;

            using (Transaction t = new Transaction(doc, "Undo changes"))
            {
                t.Start();

                try
                {
                    if (HelpersPlaceTags.selectedBeamsNewMarks.Count > 0)
                    {
                        foreach (ElementId eid in HelpersPlaceTags.selectedBeamsNewMarks.Keys)
                        {
                            Helpers.assignMark(doc, eid, HelpersPlaceTags.selectedBeamsNewMarks[eid]);

                            Helpers.ResetOverrideColor(eid, doc);
                        }

                        HelpersPlaceTags.selectedBeamsNewMarks.Clear();
                        HelpersPlaceTags.selectedBeamsOriginalMarks.Clear();
                    }
                    else
                    {
                        TaskDialog.Show("Warning", "There are no changes to accept");
                    }

                }

                //Press.Keys("UU");

                //SendKeys.SendWait("UU");

                #region catch and finally
                catch (Exception ex)
                {
                    TaskDialog.Show("Catch", "Undo changes failed due to:" + Environment.NewLine + ex.Message);
                }
                finally
                {

                }
                #endregion

                t.Commit();

            }//close using
        }

        public string GetName()
        {
            return "External Event Example";
        }

    }
}
