using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class TagElementsInViewport : IExternalCommand
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

            IList<Reference> refe = uidoc.Selection.PickObjects(ObjectType.Element, "Select Viewports");

            IList<ElementId> viewportViewIds = new List<ElementId>();

            foreach (Reference r in refe)
            {
                Viewport vp = doc.GetElement(r) as Viewport;
                viewportViewIds.Add(vp.ViewId);
            }

            List<Element> toBeTagged = new List<Element>();

            using (Transaction t = new Transaction(doc, "Tag Elements in View"))
            {
                t.Start();

                foreach (ElementId viewId in viewportViewIds)
                {
                    FilteredElementCollector fec = new FilteredElementCollector(doc, viewId).WhereElementIsNotElementType();
                    foreach (Element ele in fec)
                    {
                        try
                        {
                            if (ele.Category.Name == "Structural Columns")
                            {       
                                FamilyInstance fa = ele as FamilyInstance;
                                CreateIndependentTagColumn(doc, fa, viewId);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                t.Commit();
            }

            string s = "";

            foreach (Element e in toBeTagged)
            {

                s += e.Category.Name;
            }

            TaskDialog.Show("result", s);

            return Result.Succeeded;
        }



        /// <summary>
        /// https://forums.autodesk.com/t5/revit-api-forum/independenttag-how-do-i-call-this-in-revit/td-p/7733731
        /// </summary>
        /// <param name="document"></param>
        /// <param name="column"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        private IndependentTag CreateIndependentTagColumn(Document document, FamilyInstance column, ElementId viewId)
        {
            View view = document.GetElement(viewId) as View;

            // define tag mode and tag orientation for new tag
            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorn = TagOrientation.Horizontal;

            // Add the tag to the middle of the colunm
            Reference columnRef = new Reference(column);

            BoundingBoxXYZ bbox = column.get_BoundingBox(view);

            XYZ centroid = new XYZ((bbox.Max.X + bbox.Min.X) / 2, (bbox.Max.Y + bbox.Min.Y) / 2, (bbox.Max.Z + bbox.Min.Z) / 2);

            XYZ position = centroid + new XYZ(4, 0, 0);

            IndependentTag newTag = IndependentTag.Create(document, viewId, columnRef, false, tagMode, tagorn, position);

            if (null == newTag)
            {
                throw new Exception("Create IndependentTag Failed.");
            }

            return newTag;
        }

    }
}


    
