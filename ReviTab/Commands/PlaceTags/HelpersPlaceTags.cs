using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;

namespace ReviTab
{
    class HelpersPlaceTags
    {
        public static List<ElementId> elementIds { get; set; }
        public static List<string> currentMarks { get; set; }

        public static Dictionary<ElementId, string> selectedBeamsOriginalMarks = new Dictionary<ElementId, string>();

        public static Dictionary<ElementId, string> selectedBeamsNewMarks = new Dictionary<ElementId, string>();

        
    }
}
