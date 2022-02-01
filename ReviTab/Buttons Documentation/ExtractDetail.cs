using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class ExtractDetail : IExternalCommand
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

			ICollection<ElementId> detailToCopy = uidoc.Selection.GetElementIds();

			ViewFamilyType vd = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().FirstOrDefault(q => q.ViewFamily == ViewFamily.Drafting);
			ViewDrafting destinationView = null;

			using (Transaction t = new Transaction(doc, "Move Details"))
			{
				t.Start();

				destinationView = ViewDrafting.Create(doc, vd.Id);

				//destinationView.Name = "New Drafting View";

				destinationView.Scale = doc.ActiveView.Scale;

				ElementTransformUtils.CopyElements(doc.ActiveView, detailToCopy, destinationView, null, null);

				doc.Delete(detailToCopy);

				t.Commit();

			}

			if (destinationView != null)
			{
				uidoc.ActiveView = destinationView;
			}

			return Result.Succeeded;
        }
    }
}
