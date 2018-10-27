using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace ReviTab
{
    class Helpers
    {

        #region RANDOM

        /// <summary>
        /// Hello World
        /// </summary>
        public static void leannSays()
        {
            string[] leanneDictionary = { "Ciao a tutti!", "You are Evil", "I smash you", "You I know can you guys delete it after finish ready. Thanks", "book my set tomorrow. donot forget thanks", "i am coming back on thuesday. \nthis is the day i will in office",   };

            Random rand = new Random();

            int index = rand.Next(leanneDictionary.Length);

            // Begin Code Here
            TaskDialog.Show("Leanne says", leanneDictionary[index]);
        }

        /// <summary>
        /// Hello Christmas
        /// </summary>
        public static void Christams()
        {
            TaskDialog.Show("ohohoh", "Merry Christmas");
        }

        /// <summary>
        /// Calc the sum
        /// </summary>
        /// <param name="message"></param>
        public static void AddTwoIntegers(string message)
        {

            string[] sub = message.Split('+');

            int result = Int16.Parse(sub[0]) + Int16.Parse(sub[1]);

            TaskDialog.Show("Sum", result.ToString());

        }
        #endregion

        #region PRINT

        public static int PrintDrawingsFromList(Document doc, ViewSheet sheet, string destinationFile, PrintSetting printSetting)
        {
            int result = 0;

            using (Transaction transaction = new Transaction(doc, "Print ViewSheetSet"))
            {
                try
                {
                    transaction.Start();

                    PrintManager printMan = doc.PrintManager;

                    printMan.PrintRange = PrintRange.Select; //A range that represents a list of selected views and sheets. 

                    printMan.CombinedFile = true; //do not combine the pdfs
                    printMan.Apply();

                    printMan.PrintToFile = true;
                    printMan.Apply();

                    printMan.PrintToFileName = destinationFile;
                    printMan.Apply();

                    printMan.PrintSetup.CurrentPrintSetting = printSetting;

                    printMan.SubmitPrint();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("error", ex.Message);
                }

            }

            return result;

        }//close method

        public static List<ViewSheetSet> CollectViewSheetSets(Document doc)
        {

            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(ViewSheetSet));

            List<ViewSheetSet> allViewSets = new List<ViewSheetSet>();

            using (IEnumerator<Element> enumerator = fec.ToElements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ViewSheetSet vs = (ViewSheetSet)enumerator.Current;
                    allViewSets.Add(vs);

                }
            }

            return allViewSets;
        }
        
        /// <summary>
        /// Create a new ViewSet (if exists, delete it and create a new one) and add some sheet views to it.
        /// </summary>
        /// <param name="doc">The Revit project (current or opened in background)</param>
        /// <param name="message">String of sheet numbers to be added to the viewset (i.e. 101 102 103)</param>
        /// <returns></returns>
        public static ViewSet CreateViewset(Document doc, string message)
        {

            string sheetNumber = message;

            string[] split = sheetNumber.Split(' ');

            string viewSetName = split[1];

            ViewSet myViewSet = new ViewSet();

            FilteredElementIterator elemItr = new FilteredElementCollector(doc).OfClass(typeof(ViewSheetSet)).GetElementIterator();

            elemItr.Reset();

            Element existingViewSet = null;

            while (elemItr.MoveNext())
            {
                if (elemItr.Current.Name == viewSetName)
                    existingViewSet = elemItr.Current;
            }

            IEnumerable<ViewSheet> sheetItr = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToElements().Cast<ViewSheet>();

            foreach (ViewSheet e in sheetItr)
            {
                if (sheetNumber.Contains(e.SheetNumber))
                    myViewSet.Insert(e);
            }



            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create View Set");

                //If exists, delete existing viewset
                try
                {
                    doc.Delete(existingViewSet.Id);
                }
                catch
                {
                    //if the view set does not exists, don't crash
                }

                //Create the new viewset		
                PrintManager printMan = doc.PrintManager;
                printMan.PrintRange = PrintRange.Select;
                ViewSheetSetting viewSetting = printMan.ViewSheetSetting;
                viewSetting.CurrentViewSheetSet.Views = myViewSet;
                viewSetting.SaveAs(viewSetName);

                t.Commit();

            }

            return myViewSet;

        }//close macro

        public static string GetAvailablePrinterSetups(Document doc, string mustContain)
        {
            List<string> list = new List<string>();
            string names = "";
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(PrintSetting));
            using (IEnumerator<Element> enumerator = filteredElementCollector.ToElements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    PrintSetting printSetting = (PrintSetting)enumerator.Current;
                    //bool flag = mustContain.Length == 0 || printSetting.Name.Contains(mustContain);
                    bool flag = true;
                    if (flag)
                    {
                        list.Add(printSetting.Name);
                        names += printSetting.Name + "\n";
                    }
                }
            }
            return names;
        }

        public static string GetCurrentPrintSetup(Document doc)
        {
            PrintManager printManager = doc.PrintManager;
            bool flag = printManager != null && printManager.PrinterName != null;
            string result;
            if (flag)
            {
                result = printManager.PrinterName;
            }
            else
            {
                result = "";
            }
            return result;
        }
        #endregion

        #region SECTION

        /// <summary>
        /// Get the First or Default View Family Type of a Section
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static ViewFamilyType viewFamilyType(Document doc)
        {
            ViewFamilyType vft = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault<ViewFamilyType>(x => ViewFamily.Section == x.ViewFamily);

            return vft;
        }//close method


        /// <summary>
        /// Create Section Perpendicular to the selected elements (must have location curves)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="uidoc"></param>
        /// <param name="ele"></param>
        public static void CreateSectionPerpendicular(Document doc, UIDocument uidoc, Element ele)
        {
            // My library 

            Element wall = ele;

            LocationCurve lc = wall.Location as LocationCurve;
            Transform curveTransform = lc.Curve.ComputeDerivatives(0.5, true);

            // The transform contains the location curve
            // mid-point and tangent, and we can obtain
            // its normal in the XY plane:

            XYZ origin = curveTransform.Origin;
            XYZ viewdir = curveTransform.BasisX.Normalize();
            XYZ up = XYZ.BasisZ;
            XYZ right = up.CrossProduct(viewdir);

            // Set up view transform, assuming wall's "up" 
            // is vertical. For a non-vertical situation 
            // such as section through a sloped floor, the 
            // surface normal would be needed

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = right;
            transform.BasisY = up;
            transform.BasisZ = viewdir;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;

            // Min & Max X values define the section
            // line length on each side of the wall.
            // Max Y is the height of the section box.
            // Max Z (5) is the far clip offset.

            double d = 1;
            BoundingBoxXYZ bb = wall.get_BoundingBox(null);
            double minZ = bb.Min.Z;
            double maxZ = bb.Max.Z;
            double h = maxZ - minZ;
            // Z value offset from the current level in view. 
            Level level = doc.ActiveView.GenLevel;
            double top = 90 - level.Elevation;
            double bottom = -(level.Elevation + 25);

            sectionBox.Min = new XYZ(-2 * d, bottom, 0);
            sectionBox.Max = new XYZ(2 * d, top, 5);

            ViewFamilyType vft = viewFamilyType(doc);

            ViewSection vs = null;

            vs = ViewSection.CreateSection(doc, vft.Id, sectionBox);
        }

        /// <summary>
        /// Create Section Parallel to the selected elements (must have location curve)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="uidoc"></param>
        /// <param name="ele"></param>
        /// <param name="sectionPosition"></param>
        /// <param name="farClipOffset"></param>
        /// <param name="bottomLevel"></param>
        /// <param name="topLevel"></param>
        public static void CreateSectionParallel(Document doc, UIDocument uidoc, Element ele, double sectionPosition, double farClipOffset, double bottomLevel, double topLevel)
        {

            Element wall = ele;

            // Create a BoundingBoxXYZ instance centered on wall
            BoundingBoxXYZ bb = wall.get_BoundingBox(null);
            double minZ = bb.Min.Z;
            double maxZ = bb.Max.Z;
            double h = maxZ - minZ;
            Level level = doc.ActiveView.GenLevel;
            //double top = 90 - level.Elevation;
            //double bottom = -(level.Elevation + 25);

            LocationCurve lc = wall.Location as LocationCurve;
            Line line = lc.Curve as Line;

            XYZ p = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = p - q; // p point 0 - q point 1 - view direction up. 

            double halfLength = v.GetLength() / 2;
            //double offset = 0; // offset by 3 feet. 
            //double farClipOffset = 1;

            //Max/Min X = Section line Length, Max/Min Y is the height of the section box, Max/Min Z far clip
            XYZ min = new XYZ(-halfLength, bottomLevel, -sectionPosition);
            XYZ max = new XYZ(halfLength, topLevel, farClipOffset);

            XYZ midpoint = q + 0.5 * v; // q get lower midpoint. 
            XYZ walldir = v.Normalize();
            XYZ up = XYZ.BasisZ;
            XYZ viewdir = walldir.CrossProduct(up);

            Transform t = Transform.Identity;
            t.Origin = midpoint;
            t.BasisX = walldir;
            t.BasisY = up;
            t.BasisZ = viewdir;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = t;
            sectionBox.Min = min; // scope box start 
            sectionBox.Max = max; // scope box end

            ViewFamilyType vft = viewFamilyType(doc);
            ViewSection vs = null;

            vs = ViewSection.CreateSection(doc, vft.Id, sectionBox);

        }

        #endregion

        #region SELECTION

        /// <summary>
        /// Select All Text notes and runs the grammar check.
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="uiapp"></param>
        public static void SelectAllText(UIDocument uidoc, UIApplication uiapp)
        {

            Document doc = uidoc.Document;

            Selection selElements = uidoc.Selection;

            ICollection<ElementId> idTxt = new FilteredElementCollector(doc).OfClass(typeof(TextNote)).ToElementIds();

            selElements.SetElementIds(idTxt);

            //		    Helpers.Press.Keys("F7");
            RevitCommandId commandId = RevitCommandId.LookupPostableCommandId(PostableCommand.CheckSpelling);

            if (uiapp.CanPostCommand(commandId))
            {
                uiapp.PostCommand(commandId);
            }
        }//close method

        /// <summary>
        /// Select all the elements in the active view by their category name
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="message"></param>
        public static void SelectAllTypes(UIDocument uidoc, string message)
        {


            string eleType = message.Split('*')[1];

            Document doc = uidoc.Document;

            Selection selElements = uidoc.Selection;

            ICollection<ElementId> idTxt = new FilteredElementCollector(doc, doc.ActiveView.Id).ToElementIds();

            List<ElementId> selectedElements = new List<ElementId>();

            foreach (ElementId eid in idTxt)
            {
                Element ele = doc.GetElement(eid);
                try
                {
                    string name = ele.Category.Name;
                    if (name == eleType)
                        selectedElements.Add(eid);
                }
                catch
                {
                    continue;
                }
            }

            selElements.SetElementIds(selectedElements);
            //TaskDialog.Show("Success", eleType);

        }//close method}
        #endregion

        #region SHEET VIEWS

        public static ViewSheet FindViewSheetByName(Document doc, string ViewSheetName)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(ViewSheet));
            ViewSheet result;
            using (IEnumerator<Element> enumerator = filteredElementCollector.ToElements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ViewSheet viewSheet = (ViewSheet)enumerator.Current;
                    bool flag = viewSheet.Name == ViewSheetName;
                    if (flag)
                    {
                        result = viewSheet;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }

        public static List<ViewSheet> FindViewSheetByNumber(Document doc, string viewSheetNumbers)
        {

            List<ViewSheet> listSheets = new List<ViewSheet>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(ViewSheet));

            using (IEnumerator<Element> enumerator = filteredElementCollector.ToElements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ViewSheet viewSheet = (ViewSheet)enumerator.Current;
                    bool flag = viewSheetNumbers.Contains(viewSheet.SheetNumber);
                    if (flag)
                    {
                        listSheets.Add(viewSheet);

                    }
                }
            }

            return listSheets;
        }

        /// <summary>
        /// Collect all the Sheet views in the document. Return a List of ViewSheet sorted by their sheet numbers.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<ViewSheet> CollectViewSheet(Document doc)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(ViewSheet));
            List<ViewSheet> sheetInModel = new List<ViewSheet>();
            using (IEnumerator<Element> enumerator = filteredElementCollector.ToElements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ViewSheet viewSheet = (ViewSheet)enumerator.Current;
                    sheetInModel.Add(viewSheet);
                }
            }

            List<ViewSheet> sortedList = sheetInModel.OrderBy(o => o.SheetNumber).ToList();

            return sortedList;
        }

        public static string SheetInViewSheetsSets(ViewSheetSet pickedViewSheetSets)
        {

            string sheetsInViewSheetSetNumbers = "";

            ViewSet sheetsInViewSheetSet = pickedViewSheetSets.Views;


            foreach (View v in sheetsInViewSheetSet)
            {
                if (v.Category.Name == "Sheets")
                {
                    ViewSheet castView = v as ViewSheet;
                    sheetsInViewSheetSetNumbers += castView.SheetNumber + " ";
                }
            }
            return sheetsInViewSheetSetNumbers;
        }
        
        /// <summary>
        /// Get the sheet revision (must be the arup standard one)
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static string SheetRevision(ViewSheet vs)
        {
            try
            {
                Parameter rev = vs.GetParameters("ARUP_BDR_ISSUE")[0];
                return '['+rev.AsString()+']';
            }
            catch
            {
                return "";
            }
        }
        #endregion

        public static Dictionary<string, PrintSetting> GetPrintersSettings(Document doc)
        {

            Dictionary<string, PrintSetting> printSettingNames = new Dictionary<string, PrintSetting>();

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(PrintSetting));
            IEnumerator<Element> enumerator = filteredElementCollector.ToElements().GetEnumerator();

            while (enumerator.MoveNext())
            {
                PrintSetting printSetting = (PrintSetting)enumerator.Current;
                bool flag = true;
                if (flag)
                {
                    printSettingNames.Add(printSetting.Name, printSetting);

                }
            }
            return printSettingNames;
        }

        #region STRUCTURAL FRAMINGS

        public static void GetInstanceGeometry(GeometryObject obj, Dictionary<int, Face> areas)
        {

            GeometryInstance instance = obj as GeometryInstance;
            if (null != instance)
            {
                GeometryElement instanceGeometryElement = instance.GetInstanceGeometry();
                foreach (GeometryObject instanceObj in instanceGeometryElement)
                {
                    Solid instanceGeomSolid = instanceObj as Solid;
                    if (null != instanceGeomSolid)
                    {
                        foreach (Face geomFace in instanceGeomSolid.Faces)
                        {
                            try
                            {
                                areas.Add((int)geomFace.Area, geomFace);
                            }
                            catch
                            {
                            }
                        }
                    }

                }

            }//close object array

        }//close method

        public static void PlaceOpening(Document doc, Reference selectedElement, int distanceFromStart)
        {
            Element ele = doc.GetElement(selectedElement.ElementId);

            Face webFace = null;

            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.View = doc.ActiveView;
            GeometryElement beamGeom = ele.get_Geometry(geomOptions);

            Dictionary<int, Face> areas = new Dictionary<int, Face>();

            foreach (GeometryObject obj in beamGeom)
            {
                Solid geomSolid = obj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        try
                        {
                            areas.Add((int)geomFace.Area, geomFace);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    try
                    {
                        GetInstanceGeometry(obj, areas);
                    }
                    catch
                    {

                    }

                }
            }

            int total = areas.Keys.Count;

            int maxArea = areas.Keys.Max();
            webFace = areas[maxArea];

            BoundingBoxUV bboxUV = webFace.GetBoundingBox();

            UV center = (bboxUV.Max + bboxUV.Min) * 0.5;
            UV start = bboxUV.Min;
            XYZ location = webFace.Evaluate(start);

            XYZ normal = webFace.ComputeNormal(center);
            XYZ refDir = normal.CrossProduct(XYZ.BasisZ);

            FilteredElementCollector ope = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructConnections).WhereElementIsElementType();

            FamilySymbol fs = null;

            foreach (FamilySymbol f in ope)
            {
                if (f.FamilyName == "Web Penetration with Stiffeners")
                    fs = f as FamilySymbol;

            }

            FamilyInstance instance = doc.Create.NewFamilyInstance(webFace, location, refDir, fs);

            foreach (Parameter p in instance.Parameters)
            {
                if (p.Definition.Name == "Distance from Start")
                {
                    p.Set(distanceFromStart / 304.8);
                }
            }


        }//close method

        #endregion
    }
}
