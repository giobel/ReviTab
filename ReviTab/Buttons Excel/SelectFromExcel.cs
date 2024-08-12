#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class SelectFromExcel : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {

                string content = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

                string[] contentSplit = content.Split('\n');

                ICollection<ElementId> eids = new List<ElementId>();

                int count = 0;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < contentSplit.Length - 1; i++)
                {
                    try
                    {
                        eids.Add(new ElementId(Int32.Parse(contentSplit[i])));
                        count++;
                    }
                    catch
                    {
                        sb.AppendLine($"ElementId {contentSplit[i]} has thrown an error.");
                    }

                }

                // Revit 2014

                //Autodesk.Revit.UI.Selection.SelElementSet selElements = uidoc.Selection.Elements;
                Selection selElements = uidoc.Selection;

                //foreach (ElementId eid in eids)
                //{
                //    selElements.Insert(doc.GetElement(eid));
                //    count += 1;
                //}

                TaskDialog.Show("Result", $"{count} element(s) have been selected. Errors:{sb.ToString()}");

                selElements.SetElementIds(eids);
                //uidoc.Selection.Elements = selElements;





            }
            else
            {
                TaskDialog.Show("Error", "THe Clipboard does not contain any valid ElementId");
            }

            return Result.Succeeded;

        }
    }
}