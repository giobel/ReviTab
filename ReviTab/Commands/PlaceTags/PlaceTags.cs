using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ReviTab
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ButtonPlaceTags : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;

            using (Transaction t = new Transaction(doc, "Place Tags"))
            {
                try
                {
                    t.Start();
                    ICollection<ElementId> eids = uiapp.ActiveUIDocument.Selection.GetElementIds();

                    if (eids.Count == 0)
                    {
                        TaskDialog.Show("Warning", "Select a beam or multiple beams");
                    }

                    string s = string.Empty;

                    foreach (ElementId e in eids)
                    {
                        string markValue = Helpers.GetMark(doc, e);
                        s += markValue;
                        string newMark = Helpers.SetTemporaryMark(doc, e);

                        try
                        {
                            Helpers.OverrideColor(doc, e, markValue, newMark);
                        }
                        catch
                        {

                        }
                        

                        HelpersPlaceTags.selectedBeamsOriginalMarks.Add(e, markValue);
                        HelpersPlaceTags.selectedBeamsNewMarks.Add(e, newMark);


                        //Helpers.elementIds.Add(e);
                        //Helpers.currentMarks.Add(markValue);
                    }


                    t.Commit();
                    //TaskDialog.Show("Result", string.Format("{0} items selected. {2} {1}", eids.Count.ToString(), s, Environment.NewLine));
                }

                #region catch and finally
                catch (Exception ex)
                {
                    TaskDialog.Show("Catch", "Failed due to:" + Environment.NewLine + ex.Message);
                }
                finally
                {

                }
                #endregion

            }//close using
        }

        public string GetName()
        {
            return "External Event Example";
        }




    }
}
