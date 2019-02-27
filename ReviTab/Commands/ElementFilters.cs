using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    public class BeamSelectionFilter : ISelectionFilter
    {

        public string catNameChosen { get; set; }

        public BeamSelectionFilter(string catName)
        {
            this.catNameChosen = catName;
        }

        public bool AllowElement(Element e)
        {

            //if (e.Category.Name == "Structural Framing")
            if (e.Category.Name == catNameChosen)
            {
                return true;
            }
            return false;
        }


        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }

    }//close class


    public class LineSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element e)
        {

            if (e.Category.Name == "Lines")
            {
                return true;
            }
            return false;
        }


        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }

    }//close class




}
