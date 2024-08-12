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

            string errorMessage = "";

            int counterTagged = 0;
            StringBuilder errorlog = new StringBuilder();

            double xOffset = 0;
            string category = "";

            using (var form = new FormTwoTextBoxes("Tag X-offset [mm] from centreline", "Category to Tag"))
            {
                form.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                try
                {
                    xOffset = Int16.Parse(form.TextString) / 304.8;
                    category = form.TextString2;
                }
                catch
                {
                    xOffset = 0;
                }

            }

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
                            if (ele.Category.Name == category)
                            {
                                FamilyInstance fa = ele as FamilyInstance;
                                var eleRef = new Reference(ele);

                                if (category == "Structural Columns")
                                {
#if !REVIT2022 && !REVIT2024
                                    CreateIndependentTagColumn(doc, fa, viewId, xOffset, category);
#else
                                    
                                    IndependentTag.Create(doc, fa.Id, viewId, eleRef, false, TagOrientation.AnyModelDirection, eleRef.GlobalPoint);
#endif
                                }
                                else if (category == "Walls")
                                {
                                    Wall wall = ele as Wall;
                                    //var wallref = new Reference(wall); 
#if !REVIT2022 && !REVIT2024
                                    CreateIndependentTagWall(doc, wall, viewId, xOffset, category);
#else
                                    IndependentTag.Create(doc, fa.Id, viewId, eleRef, false, TagOrientation.AnyModelDirection, eleRef.GlobalPoint);
#endif
                                }

                                    counterTagged++;

                            }
                        }
                        catch (Exception ex)
                        {
                            if (ele.Category != null && ele.Category.Name == "Structural Columns")
                            {
                                errorMessage = ex.Message;
                                //TaskDialog.Show("Error", ex.Message);
                                errorlog.AppendLine($"{ele.Id}");
                            }
                        }
                    }
                }
                t.Commit();
            }

            
            TaskDialog.Show("result", $"{counterTagged} elements tagged. \nElement Id errors: \n{errorlog.ToString()}");

            return Result.Succeeded;
        }


#if REVIT2019 || REVIT2018 || REVIT2020 || REVIT2021
        /// <summary>
        /// https://forums.autodesk.com/t5/revit-api-forum/independenttag-how-do-i-call-this-in-revit/td-p/7733731
        /// </summary>
        /// <param name="document"></param>
        /// <param name="column"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        private IndependentTag CreateIndependentTagColumn(Document document, FamilyInstance famInstance, ElementId viewId, double Xoffset, string ElementCategory)
        {
            View view = document.GetElement(viewId) as View;

            // define tag mode and tag orientation for new tag
            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorn = TagOrientation.Horizontal;

            Reference elementReference = null;

            if (ElementCategory == "Structural Columns")
            {                
                elementReference= new Reference(famInstance);
            }
            else
            {
                TaskDialog.Show("Error", "Works only with Columns and Walls");
            }

            // Add the tag to the middle of the colunm
            BoundingBoxXYZ bbox = famInstance.get_BoundingBox(view);

            XYZ centroid = new XYZ((bbox.Max.X + bbox.Min.X) / 2, (bbox.Max.Y + bbox.Min.Y) / 2, (bbox.Max.Z + bbox.Min.Z) / 2);

            XYZ position = centroid + new XYZ(Xoffset, 0, 0);

            IndependentTag newTag = IndependentTag.Create(document, viewId, elementReference, false, tagMode, tagorn, position);

            if (null == newTag)
            {
                throw new Exception("Create IndependentTag Failed.");
            }

            return newTag;
        }


        private IndependentTag CreateIndependentTagWall(Document document, Wall famInstance, ElementId viewId, double Xoffset, string ElementCategory)
        {
            View view = document.GetElement(viewId) as View;

            // define tag mode and tag orientation for new tag
            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorn = TagOrientation.Horizontal;

            Reference elementReference = null;

            if (ElementCategory == "Walls")
            {
                elementReference = new Reference(famInstance);
            }
            else
            {
                TaskDialog.Show("Error", "Works only with Columns and Walls");
            }

            // Add the tag to the middle of the colunm
            BoundingBoxXYZ bbox = famInstance.get_BoundingBox(view);

            XYZ centroid = new XYZ((bbox.Max.X + bbox.Min.X) / 2, (bbox.Max.Y + bbox.Min.Y) / 2, (bbox.Max.Z + bbox.Min.Z) / 2);

            XYZ position = centroid + new XYZ(Xoffset, 0, 0);

            IndependentTag newTag = IndependentTag.Create(document, viewId, elementReference, false, tagMode, tagorn, position);

            if (null == newTag)
            {
                throw new Exception("Create IndependentTag Failed.");
            }

            return newTag;
        }

#elif REVIT2017

        /// <summary>
        /// https://thebuildingcoder.typepad.com/blog/2010/06/set-tag-type.html
        /// </summary>
        /// <param name="document"></param>
        /// <param name="column"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        private IndependentTag CreateIndependentTagColumn(Document document, FamilyInstance column, ElementId viewId, double Xoffset)
        {
            View view = document.GetElement(viewId) as View;

            // define tag mode and tag orientation for new tag
            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagorientation = TagOrientation.Horizontal;

            // Add the tag to the middle of the colunm
            Reference columnRef = new Reference(column);

            BoundingBoxXYZ bbox = column.get_BoundingBox(view);

            XYZ centroid = new XYZ((bbox.Max.X + bbox.Min.X) / 2, (bbox.Max.Y + bbox.Min.Y) / 2, (bbox.Max.Z + bbox.Min.Z) / 2);

            XYZ position = centroid + new XYZ(Xoffset, 0, 0);

            IndependentTag newTag = document.Create.NewTag(view, column, false, tagMode, tagorientation, position);

            if (null == newTag)
            {
                throw new Exception("Create IndependentTag Failed.");
            }

            return newTag;
        }
#endif
    }

}


    
