using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using winForm = System.Windows.Forms;

namespace ReviTab
{
	[Transaction(TransactionMode.Manual)]
	public class CreateSectionColumns : IExternalCommand
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

			ISelectionFilter columnFilter = new CategorySelectionFilter("Structural Columns");

			IList<Reference> r = uidoc.Selection.PickObjects(ObjectType.Element, columnFilter, "Select columns");

			List<Element> myElements = new List<Element>();

			int s = 0;

			foreach (var e in r)
			{
				myElements.Add(doc.GetElement(e));
			}

			using (var form = new FormCreateSections())
			{

				form.ShowDialog();

				if (form.DialogResult == winForm.DialogResult.Cancel)
				{
					return Result.Cancelled;
				}

				using (Transaction tx = new Transaction(doc))
				{

					try
					{
						tx.Start("Create Section");

						if (form.sectionOrientation == "Long")

							foreach (Element e in myElements)
							{
								Helpers.CreateColumnSection(doc, e, form.sectionPositionOffset, form.farClipOffset, form.bottomLevel, form.topLevel, form.flipDirection);
								s += 1;
							}

						else
						{
							TaskDialog.Show("Warning", "Not implemented yet");
						}

						TaskDialog.Show("Result", $"{s.ToString()} sections created");

						//tx.Commit();

						return Result.Succeeded;
					}
					catch (Exception ex)
					{
						TaskDialog.Show("r", ex.Message);
						return Result.Failed;
					}
					finally
					{
						tx.Commit();
					}

				}//close transaction

			}//close winform
		}//close execute
	}
}
