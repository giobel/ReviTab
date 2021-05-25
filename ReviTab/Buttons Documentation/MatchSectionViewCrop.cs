#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using forms = System.Windows.Forms;
#endregion

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class MatchSectionViewCrop : IExternalCommand
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

            ICollection<ElementId> selectedElementsId = uidoc.Selection.GetElementIds();

			List<View> selectedViews = new List<View>();

            foreach (ElementId eid in selectedElementsId)
            {
				View current = doc.GetElement(eid) as View;
				selectedViews.Add(current);
            }



			int count = 0;
			string error = "";

			var form = new Forms.FormPickFromDropDown();

			using (Transaction t = new Transaction(doc, "Resize viewport"))
			{
				if (selectedViews.Count >1)
                {
					form.ViewSectionList = new ObservableCollection<View>(selectedViews.OrderBy(x => x.Name));

					form.ShowDialog();
				}
                else
                {
					TaskDialog.Show("Error", "Please select some Section Views first.");
					return Result.Cancelled;
                }


				if (form.DialogResult == false)
				{
					return Result.Cancelled;
				}

				View sourceView = form.SelectedViewSection;

				BoundingBoxXYZ sourceViewBB = sourceView.CropBox;

				ViewCropRegionShapeManager vcrSource = sourceView.GetCropRegionShapeManager();
				CurveLoop sourceLoop = vcrSource.GetCropShape()[0];

				t.Start();
				foreach (ElementId eid in selectedElementsId)
				{
					View destinationView = doc.GetElement(eid) as View;
					try
					{
						//The origin of the cropbox (from Transform) is not the same for different Sections. Use CropRegionShapeManager instead

						//destinationView.CropBox.Transform.Origin = sourceViewBB.Transform.Origin;
						//destinationView.CropBox = sourceViewBB;		
						ViewCropRegionShapeManager destinationVcr = destinationView.GetCropRegionShapeManager();
						destinationVcr.SetCropShape(sourceLoop);
						//if the crop is rectangular
						if (sourceLoop.Count() == 4)
						{
							destinationVcr.RemoveCropRegionShape(); //Reset Crop after it has been set
						}
						count++;
					}
					catch
					{						
						error += $"Error processing view: {destinationView.Name}\n";
					}
				}

				t.Commit();

			}



			TaskDialog.Show("Result", $"{count}/{selectedElementsId.Count} viewport updated. \n{error}");
            
            return Result.Succeeded;
        }


    }
}
