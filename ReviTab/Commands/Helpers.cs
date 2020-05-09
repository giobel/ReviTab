using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirtableApiClient;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MySql.Data.MySqlClient;

namespace ReviTab
{
    public class Helpers
    {

        #region RANDOM

        /// <summary>
        /// Hello World
        /// </summary>
        public static void leannSays()
        {
            string[] leanneDictionary = { "Ciao a tutti!", "You are Evil", "i am angel", "I smash you", "You I know can you guys delete it after finish ready. Thanks", "book my set tomorrow. donot forget thanks", "i am coming back on thuesday. \nthis is the day i will in office", "i try to found you last Friday you not in office", "coffee i thing" };

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

            string viewSetName = "remote printer";
            try
            {
                viewSetName = split[1];
            }
            catch
            {
                // leave the view set name = remote printer
            }


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
        public static ViewSection CreateSectionParallel(Document doc, UIDocument uidoc, Element ele, double sectionPosition, double farClipOffset, double bottomLevel, double topLevel)
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

            return vs;
        }

        public static void CreateColumnSection(Document doc, Element columnElement, double offsetFromAlignment, double farClip, double bottomZ, double topZ, bool flipDirection, string columnParameter)
        {
            
            FamilyInstance fi = columnElement as FamilyInstance;

            LocationPoint lp = columnElement.Location as LocationPoint;
            //PlotPoint(lp.Point);
            //x-Vector
            XYZ xDir = fi.HandOrientation.Normalize();
            //PlotPoint(xDir);
            //y-Vector
            XYZ yDir = xDir.CrossProduct(XYZ.BasisZ).Normalize();
            //PlotPoint(yDir);


            //Start Point
            XYZ p = lp.Point + xDir * 2;
            //End Point
            XYZ q = lp.Point - xDir * 2;

            XYZ v = q - p;

            XYZ perp = null;

            //Rightward
            XYZ gX = new XYZ(1, 0, 0);
            //Downward
            XYZ gY = new XYZ(0, -1, 0);

            if (yDir.IsAlmostEqualTo(gX) || yDir.IsAlmostEqualTo(gY))
            {
                perp = lp.Point - yDir * 2;
                xDir = -xDir;
            }
            else
            {
                perp = lp.Point + yDir * 2;
            }

            //section alignment
            //Line crv = Line.CreateBound(p, q);

            //section direction
            //Line dir = Line.CreateBound(lp.Point, perp);

            //section alignment, section height, section depth on plan (far clip offset)
            XYZ min = new XYZ(-v.GetLength() / 2 - 1, bottomZ, offsetFromAlignment);
            XYZ max = new XYZ(v.GetLength() / 2 + 1, topZ, farClip);

            XYZ midpoint = lp.Point;
            XYZ sectionAlignment = xDir;

            if (flipDirection)
            {
                sectionAlignment = -xDir;
            }

            XYZ up = XYZ.BasisZ;
            XYZ viewdir = sectionAlignment.CrossProduct(up);

            Transform t = Transform.Identity;
            t.Origin = midpoint;
            t.BasisX = sectionAlignment;
            t.BasisY = up;
            t.BasisZ = viewdir;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = t;
            sectionBox.Min = min;
            sectionBox.Max = max;

            //doc.Create.NewDetailCurve(doc.ActiveView, crv);
            //doc.Create.NewDetailCurve(doc.ActiveView, dir);
            ViewFamilyType vft = viewFamilyType(doc);

            ViewSection vs = ViewSection.CreateSection(doc, vft.Id, sectionBox);

            try
            {
                vs.Name = $"Section {columnElement.LookupParameter(columnParameter).AsString()}";
            }
            catch
            {

            }


            
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
        public static void SelectAllElementsInView(UIDocument uidoc, string message)
        {

            Document doc = uidoc.Document;


            string command = message.Split('*')[1];

            string param = "";
            string operatorValue = "";
            string valueToCheck = "";

            try
            {

                if (command.Contains('>'))
                {
                    string[] check = command.Split('+')[1].Split('>');
                    param = check[0];
                    operatorValue = "larger";
                    valueToCheck = check[1];
                }
                else if (command.Contains('<'))
                {
                    string[] check = command.Split('+')[1].Split('<');
                    param = check[0];
                    operatorValue = "smaller";
                    valueToCheck = check[1];
                }
                else if (command.Contains('='))
                {
                    string[] check = command.Split('+')[1].Split('=');
                    param = check[0];
                    operatorValue = "equal";
                    valueToCheck = check[1];
                }
                else if (command.Contains('!'))
                {
                    string[] check = command.Split('+')[1].Split('!');
                    param = check[0];
                    operatorValue = "different";
                    valueToCheck = check[1];
                }
            }

            catch
            {

                TaskDialog.Show("Warning", "Something went wrong");
            }



            Selection selElements = uidoc.Selection;

            ICollection<ElementId> idTxt = new FilteredElementCollector(doc, doc.ActiveView.Id).ToElementIds();

            List<ElementId> selectedElements = new List<ElementId>();

            foreach (ElementId eid in idTxt)
            {
                try
                {
                    Element ele = doc.GetElement(eid);
                    bool appendElementId = false;

                    if (ele.Category != null)
                    {
                        string name = ele.Category.Name;
                        if (command.Split('+')[0] == "All")
                        {
                            if (param != "")
                            {
                                if (FilterElementIds(ele, param, valueToCheck, operatorValue) != null)
                                    appendElementId = true;

                            }
                            else
                            {
                                appendElementId = true;
                            }

                        }
                        else if (name == command.Split('+')[0])
                        {
                            if (param != "")
                            {
                                if (FilterElementIds(ele, param, valueToCheck, operatorValue) != null)
                                    appendElementId = true;

                            }
                            else
                            {
                                appendElementId = true;
                            }
                        }
                    }

                    if (appendElementId == true)
                    {
                        selectedElements.Add(eid);
                    }

                }


                catch
                {

                }

            }

            selElements.SetElementIds(selectedElements);
            //TaskDialog.Show("Success", param);

        }//close method


        public static Dictionary<string, List<string>> GetSettings(string inputFile)
        {

            Dictionary<string, List<string>> settings = new Dictionary<string, List<string>>();

            using (var reader = new StreamReader(inputFile))
            {

                string header = reader.ReadLine();

                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();

                    var values = line.Split(',').ToList();

                    string category = values.First();

                    values.RemoveAt(0);

                    settings.Add(category, values);

                }
            }

            return settings;

        }

        public static ElementId FilterElementIds(Element _ele, string _param, string _valueToCheck, string _operatorValue)
        {
            ElementId selectedElement = null;

            double paramValue = 0;
            int boolIntValue = 0;
            string stringValue = "";
            string test = "";

            Parameter p = _ele.LookupParameter(_param);

            StorageType parameterType = p.StorageType;
            if (StorageType.Double == parameterType)
            {
                paramValue = UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
                test = "Double";
            }
            else if (StorageType.String == parameterType)
            {
                stringValue = p.AsString();
                test = "String";
            }
            else if (StorageType.Integer == parameterType)
            {
                boolIntValue = p.AsInteger();
                test = "Bool";
            }


            if (test == "Double")
            {
                if (DoubleParamCheck(paramValue, Int64.Parse(_valueToCheck), _operatorValue))
                {
                    selectedElement = _ele.Id;
                }
            }
            else if (test == "String")
            {
                if (StringParamCheck(stringValue, _valueToCheck, _operatorValue))
                {
                    selectedElement = _ele.Id;
                }
            }
            else if (test == "Bool")
            {
                if (BoolParamCheck(boolIntValue, Int64.Parse(_valueToCheck), _operatorValue))
                {
                    selectedElement = _ele.Id;
                }
            }


            return selectedElement;
        }


        /// <summary>
        /// Select all the elements by their category name
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="message"></param>
        public static void SelectAllElements(UIDocument uidoc, string message)
        {

            Document doc = uidoc.Document;


            string command = message.Split('/')[1];

            string param = "";
            string operatorValue = "";
            string valueToCheck = "";

            try
            {

                if (command.Contains('>'))
                {
                    string[] check = command.Split('+')[1].Split('>');
                    param = check[0];
                    operatorValue = "larger";
                    valueToCheck = check[1];
                }
                else if (command.Contains('<'))
                {
                    string[] check = command.Split('+')[1].Split('<');
                    param = check[0];
                    operatorValue = "smaller";
                    valueToCheck = check[1];
                }
                else if (command.Contains('='))
                {
                    string[] check = command.Split('+')[1].Split('=');
                    param = check[0];
                    operatorValue = "equal";
                    valueToCheck = check[1];
                }
                else if (command.Contains('!'))
                {
                    string[] check = command.Split('+')[1].Split('!');
                    param = check[0];
                    operatorValue = "different";
                    valueToCheck = check[1];
                }
            }

            catch
            {

                TaskDialog.Show("Warning", "Something went wrong");
            }



            Selection selElements = uidoc.Selection;


            ICollection<ElementId> idTxt = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElementIds();



            List<ElementId> selectedElements = new List<ElementId>();

            foreach (ElementId eid in idTxt)
            {
                try
                {
                    Element ele = doc.GetElement(eid);

                    string name = "";
                    if (ele.Category != null)
                    {
                        name = ele.Category.Name;
                    }

                    if (name == command.Split('+')[0])
                    {
                        if (param != "")
                        {
                            double paramValue = 0;
                            string stringValue = "";
                            string test = "";

                            Parameter p = ele.LookupParameter(param);

                            StorageType parameterType = p.StorageType;
                            if (StorageType.Double == parameterType)
                            {
                                paramValue = UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
                                test = "Double";
                            }
                            else if (StorageType.String == parameterType)
                            {
                                stringValue = p.AsString();
                                test = "String";
                            }



                            if (test == "Double")
                            {
                                if (DoubleParamCheck(paramValue, Int64.Parse(valueToCheck), operatorValue))
                                {
                                    selectedElements.Add(eid);
                                }
                            }
                            else if (test == "String")
                            {
                                if (StringParamCheck(stringValue, valueToCheck, operatorValue))
                                {
                                    selectedElements.Add(eid);
                                }
                            }

                        }
                        else
                        {
                            selectedElements.Add(eid);
                        }
                    }
                }


                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                }

            }

            selElements.SetElementIds(selectedElements);
            //TaskDialog.Show("Success", param);

        }//close method

        /// <summary>
        /// Highlights sheet by their number. Syntax "Sheet: A101 A120"
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="message"></param>
        public static void HighlightSelectSheets(UIDocument uidoc, string message)
        {

            Document doc = uidoc.Document;

            string command = message.Split(':')[1];

            List<string> selectedSheetsList = command.Split(' ').ToList();

            IEnumerable<ViewSheet> allSheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).ToElements().Cast<ViewSheet>();

            ICollection<ElementId> eid = new List<ElementId>();

            try
            {
                foreach (ViewSheet sheet in allSheets)
                {
                    if (selectedSheetsList.Contains("all"))
                    {
                        eid.Add(sheet.Id);
                    }
                    else if (selectedSheetsList.Contains(sheet.SheetNumber.ToString()))
                    {
                        eid.Add(sheet.Id);
                    }
                }

            }
            catch
            {
                TaskDialog.Show("Error", "Syntax not recognized." + Environment.NewLine + "Use Sheets: all or Sheets: A101 A201 A301");
            }





            uidoc.Selection.SetElementIds(eid);

        }


        public static void HighlightSelectTitleBlocks(UIDocument uidoc, string message)
        {

            Document doc = uidoc.Document;

            string command = message.Split(':')[1];

            List<string> selectedTBlocks = command.Split(' ').ToList();

            IEnumerable<Element> allTBlocks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).ToElements();

            ICollection<ElementId> eid = new List<ElementId>();

            try
            {
                foreach (Element tblock in allTBlocks)
                {
                    if (selectedTBlocks.Contains("all"))
                    {
                        eid.Add(tblock.Id);
                    }
                    else if (selectedTBlocks.Contains(tblock.LookupParameter("Sheet Number").Definition.Name))
                    {
                        eid.Add(tblock.Id);
                    }
                }
            }
            catch
            {
                TaskDialog.Show("Error", "Syntax not recognized." + Environment.NewLine + "Use tblocks: all or tblocks: A101 A201 A301");
            }
            uidoc.Selection.SetElementIds(eid);
        }

        public static bool DoubleParamCheck(double param1, double param2, string operatorValue)
        {

            string operatorSwitch = operatorValue;
            bool resultValue = false;

            switch (operatorSwitch)
            {
                case "larger":
                    resultValue = Convert.ToInt64(param1) > Convert.ToInt64(param2);
                    break;
                case "equal":
                    resultValue = Convert.ToInt64(param1) == Convert.ToInt64(param2);
                    break;
                case "smaller":
                    resultValue = Convert.ToInt64(param1) < Convert.ToInt64(param2);
                    break;
                case "different":
                    resultValue = Convert.ToInt64(param1) != Convert.ToInt64(param2);
                    break;
            }
            return resultValue;
        }

        public static bool StringParamCheck(string param1, string param2, string operatorValue)
        {
            string operatorSwitch = operatorValue;
            bool resultValue = false;

            switch (operatorSwitch)
            {
                case "equal":
                    resultValue = param1 == param2;
                    break;
                case "different":
                    resultValue = param1 != param2;
                    break;
            }
            return resultValue;
        }

        public static bool BoolParamCheck(int param1, double param2, string operatorValue)
        {

            string operatorSwitch = operatorValue;
            bool resultValue = false;

            switch (operatorSwitch)
            {

                case "equal":
                    resultValue = param1 == param2;
                    break;

                case "different":
                    resultValue = param1 != param2;
                    break;
            }
            return resultValue;
        }

        public static Dictionary<string, FamilySymbol> SelectFamilies(Document doc, string catName)
        {
            ICollection<Element> eleFamily = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).ToElements();

            Dictionary<string, FamilySymbol> categoryList = new Dictionary<string, FamilySymbol>();

            foreach (FamilySymbol e in eleFamily)
            {
                try
                {
                    if (e.Category.Name == catName)
                    {
                        categoryList.Add(e.Name, e);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return categoryList;
        }
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
                return '[' + rev.AsString() + ']';
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Create a string with SheetId, Number, Revision Cloud Description, Border Issue, Titleblock Latest Revision
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cloudId"></param>
        /// <returns></returns>
        public static string RevCloudSheet(Document doc, ElementId cloudId)
        {

            try
            {
                RevisionCloud cloud = doc.GetElement(cloudId) as RevisionCloud;
                View view = doc.GetElement(cloud.OwnerViewId) as View;

                if (view is ViewSheet)
                {
                    ViewSheet vs = view as ViewSheet;

                    string lastRevisionWithoutDate = "";
                    string lastRevisionWithDate = "";
                    string lastRevisionWithDateDate = "";

                    int i = 0;

                    while (i < 10)
                    {
                        try
                        {
                            //revision can be P01, T03, 11, A
                            Parameter p = vs.LookupParameter(String.Format("{0} - Rev.", i));
                            Parameter dateParam = vs.LookupParameter(String.Format("{0} - Date", i));
                            string date = dateParam.AsString();

                            if ( date == null || date.Length < 3 )
                            {
                                lastRevisionWithoutDate = date + " " + p.AsString();
                                lastRevisionWithDate = vs.LookupParameter(String.Format("{0} - Rev.", i-1)).AsString();
                                lastRevisionWithDate = vs.LookupParameter(String.Format("{0} - Date", i - 1)).AsString();
                                break;
                            }

                        }
                        catch { }

                        finally { i++; }
                    }
                        RevisionObj ro = FindLatestRevision(vs);
                    return String.Format($"{vs.Id}," +
                        $"{vs.SheetNumber}," +
                        $"{cloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString()}," +
                        $"{vs.LookupParameter("ARUP_BDR_ISSUE").AsString()}," +
                        $"{lastRevisionWithoutDate}," +
                        $"{lastRevisionWithDateDate}," +
                        $"{lastRevisionWithDate}");
                }
                else
                {
                    return view.Name;
                }
            }
            catch (Exception ex)
            {
                return String.Format("{0},{1}", cloudId.ToString(), ex.Message);

            }
        }

        public struct RevisionObj
        {

            public int TempRevision { get; private set; }
            public int RevisionIndex { get; private set; }
            public string Letter { get; private set; }
            public string Revision { get; private set; }
            public string Date { get; private set; }
            public string NewRevision { get; private set; }
            public string DrawnBy { get; set; }
            public string Checker { get; set; }
            public string Approver { get; set; }
            public string Description { get; set; }

            public RevisionObj(int TempRevision, int RevisionIndex, string Letter, string Revision, string Date, string NewRevision, string DrawnBy = "GB", string Checker = "CM", string Approver = "FXG", string Description = "Coordination updates")
            {
                this.TempRevision = TempRevision;
                this.RevisionIndex = RevisionIndex;
                this.Letter = Letter;
                this.Revision = Revision;
                this.Date = Date;
                this.NewRevision = NewRevision;
                this.DrawnBy = DrawnBy;
                this.Checker = Checker;
                this.Approver = Approver;
                this.Description = Description;

            }

        }

        public static bool UpRevSheet(Document doc, ViewSheet vs, RevisionObj revisionObj)
        {
            try
            {
                using (Transaction t = new Transaction(doc, "Uprev sheet"))
                {
                    t.Start();

                    Parameter revision = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Rev."); 
                    
                    if (null == revision)
                    {
                        revision = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Revision");
                    }

                    revision.Set(revisionObj.NewRevision);

                    Parameter sheetRevision = vs.LookupParameter("ARUP_BDR_ISSUE");
                    sheetRevision.Set(revisionObj.NewRevision);

                    Parameter description = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Description");
                    description.Set(revisionObj.Description);

                    Parameter modeledBy = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Modeled By");

                    if (null == modeledBy)
                    {
                        modeledBy = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Drawn By");
                    }
 
                    modeledBy.Set(revisionObj.DrawnBy);

                    Parameter checkedBy = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Checked");
                    checkedBy.Set(revisionObj.Checker);

                    Parameter approved = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Approv.");

                    if (null == approved)
                    {
                        approved = vs.LookupParameter($"{revisionObj.RevisionIndex + 1} - Approved");
                    }
                    
                    approved.Set(revisionObj.Approver);

                    t.Commit();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Increment a revision when it is not formed by 1 letter only (i.e. P01 -> P02, T02->T03, 4->5). Does not work with letters (i.e. A->B).
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static RevisionObj IncrementNonLetterRevision(ViewSheet vs, int i)
        {
            Parameter p = vs.LookupParameter(String.Format("{0} - Rev.", i));
            
            if (null == p)
            {
                p = vs.LookupParameter(String.Format("{0} - Revision", i));
            }


            //numeric digit 
            string revision = new String(p.AsString().Where(Char.IsDigit).ToArray());

            int tempRev = Convert.ToInt16(revision);
            //letter
            string letter = new string(p.AsString().Where(Char.IsLetter).ToArray());

            string newRevision = letter + (Convert.ToInt16(revision) + 1).ToString().PadLeft(2, '0');

            if (letter == "P")
                tempRev += 100;

            if (letter == "T")
                tempRev += 200;

            if (letter == "C")
                tempRev += 300;

            if (letter == "")
            {
                tempRev += 400;
                //if revision is in format 01, 02...
                if (p.AsString().Length == 2)
                {
                    newRevision = (Convert.ToInt16(revision) + 1).ToString().PadLeft(2, '0');
                }
                else
                {
                    newRevision = (Convert.ToInt16(revision) + 1).ToString();
                }

            }

            Parameter dateParam = vs.LookupParameter(String.Format("{0} - Date", i));

            string date = dateParam.AsString();

            // ignore revisions without date (either empties or drawing has been already uprev)
            if (date != "")
            {
                return new RevisionObj(tempRev, i, letter, revision, date, newRevision);
            }

            return new RevisionObj();
        }

        /// <summary>
        /// Finds the latest revision and its date on a titleblock. 
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static RevisionObj FindLatestRevision(ViewSheet vs)
        {

            List<RevisionObj> lastRevisions = new List<RevisionObj>();

            # region Old method. Does not work with letters.
            /*for (int i = 0; i < 10; i++)
            {
                try
                {
                    //revision can be P01, T03, 11
                    Parameter p = vs.LookupParameter(String.Format("{0} - Rev.", i));
                    //numeric digit 
                    string revision = new String(p.AsString().Where(Char.IsDigit).ToArray());

                    int tempRev = Convert.ToInt16(revision);
                    //letter
                    string letter = new string(p.AsString().Where(Char.IsLetter).ToArray());

                    string newRevision = letter + (Convert.ToInt16(revision) + 1).ToString().PadLeft(2, '0');

                    if (letter == "P")
                        tempRev += 100;

                    if (letter == "T")
                        tempRev += 200;

                    if (letter == "C")
                        tempRev += 300;

                    if (letter == "")
                    {
                        tempRev += 400;
                        newRevision = (Convert.ToInt16(revision) + 1).ToString();
                    }

                    Parameter dateParam = vs.LookupParameter(String.Format("{0} - Date", i));

                    string date = dateParam.AsString();

                    // ignore revisions without date (either empties or drawing has been already uprev)
                    if (date != "")
                    {
                        lastRevisions.Add(new RevisionObj(tempRev, i, letter, revision, date, newRevision));
                    }
                }
                catch
                {
                    //lastRevisions.Add(p.AsString(), 0);
                }


            }*/
            #endregion

            //new method. Works with letters
            //for (int i = 0; i < 10; i++)
            int i = 1;
            while (i < 10) 
            {
                try
                {
                    //revision can be P01, T03, 11, A
                    Parameter p = vs.LookupParameter(String.Format("{0} - Rev.", i));
                    
                    if (null == p)
                    {
                        p = vs.LookupParameter(String.Format("{0} - Revision", i));
                    }

                    Parameter dateParam = vs.LookupParameter(String.Format("{0} - Date", i));
                    string date = dateParam.AsString();

                    if (p.AsString().Length == 1 && Char.IsLetter(p.AsString().ToCharArray().First()))
                    {
                        Char letter = p.AsString().ToCharArray().First();

                        Char newRevision = (Char)(Convert.ToUInt16(letter) + 1);

                        lastRevisions.Add(new RevisionObj(500, i, letter.ToString(), letter.ToString(), date, newRevision.ToString()));

                    }
                    else if (date.Length > 2)
                    {

                        lastRevisions.Add(IncrementNonLetterRevision(vs, i));

                    }

                }
                catch
                {
                    //lastRevisions.Add(p.AsString(), 0);
                }
                finally
                {
                    i++;
                }


            }


            // WHY WAS THIS REQUIRED? IT DOES NOT WORK WHEN REVISION IS SINGLE LETTER C
            //lastRevisions.Sort((x, y) => y.TempRevision.CompareTo(x.TempRevision));

            //RevisionObj lastRev = lastRevisions.First();

            RevisionObj lastRev = lastRevisions.Last();

            Parameter drawnBy = vs.LookupParameter($"{lastRev.RevisionIndex} - Modeled By");
            
            if (null == drawnBy)
            {
                drawnBy = vs.LookupParameter($"{lastRev.RevisionIndex} - Drawn By");
            }

            lastRev.DrawnBy = drawnBy.AsString();
            
            lastRev.Checker = vs.LookupParameter($"{lastRev.RevisionIndex} - Checked").AsString();
            
            Parameter approver = vs.LookupParameter($"{lastRev.RevisionIndex} - Approv.");

            if (null == approver)
            {
                approver = vs.LookupParameter($"{lastRev.RevisionIndex} - Approved");
            }
            
            lastRev.Approver = approver.AsString();

            lastRev.Description = vs.LookupParameter($"{lastRev.RevisionIndex} - Description").AsString();

            return lastRev;

        }

        public static List<CardContent> CountViewsNotOnSheet(FilteredElementCollector allViews)
        {
            List<CardContent> viewsNotOnSheet = new List<CardContent>();

            foreach (Autodesk.Revit.DB.View view in allViews)
            {
                try
                {
                    if (view.LookupParameter("Sheet Number").AsString() == "---" && !view.LookupParameter("View Purpose").AsString().Contains("Library"))
                    {

                        viewsNotOnSheet.Add(new CardContent() { Value = 1, Content = view.Name + " - " + view.LookupParameter("View Purpose").AsString() });
                    }

                }
                catch { }
            }
            return viewsNotOnSheet;
        }

        #endregion

        #region STRUCTURAL FRAMINGS

        private static Options pickOptions(Document doc)
        {
            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;
            geomOptions.View = doc.ActiveView;

            return geomOptions;
        }

        public static void GetSymbolGeometry(GeometryObject obj, Dictionary<int, Face> areas, out Transform instanceTransform)
        {
            GeometryInstance instance = obj as GeometryInstance;

            instanceTransform = instance.Transform;


            if (null != instance)
            {
                GeometryElement symbolGeometryElement = instance.GetSymbolGeometry();

                foreach (GeometryObject instanceObj in symbolGeometryElement)
                {
                    Solid instanceGeomSolid = instanceObj as Solid;

                    if (null != instanceGeomSolid)
                    {
                        foreach (Face geomFace in instanceGeomSolid.Faces)
                        {
                            if (!areas.ContainsKey((int)geomFace.Area))
                            {
                                areas.Add((int)geomFace.Area, geomFace);
                            }    
                        }
                    }

                }

            }//close object array

        }//close method

        private static void GetInstanceGeometry(GeometryObject obj, Dictionary<int, Face> areas)
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
                                //							TaskDialog.Show("Result", "Solid geometry not found");
                            }
                        }
                    }

                }

            }//close object array

        }//close method

        public static FamilyInstance PlaceOpening(Document doc, Reference selectedElement, int distanceFromStart, string FamilyName, string position, int width, int height)
        {

            double newDistance = distanceFromStart / 304.8;

            //Reference selectedElement = uidoc.Selection.PickObject(ObjectType.Element, "Select a beam");

            Element ele = doc.GetElement(selectedElement.ElementId);

            Face webFace = null;

            Options geomOptions = pickOptions(doc);

            GeometryElement beamGeom = ele.get_Geometry(geomOptions);

            Transform instTransform = null;

            Dictionary<int, Face> areas = new Dictionary<int, Face>();

            foreach (GeometryObject obj in beamGeom)
            {
                Solid geomSolid = obj as Solid;
                if (null != geomSolid)
                {
                    foreach (Face geomFace in geomSolid.Faces)
                    {
                        if (!areas.ContainsKey((int)geomFace.Area))
                        {
                            areas.Add((int)geomFace.Area, geomFace);
                        }
                    }
                }
                else
                {
                    GetSymbolGeometry(obj, areas, out instTransform);
                }
            }

            int total = areas.Keys.Count;

            int maxArea = areas.Keys.Max();
            webFace = areas[maxArea];

            // Find beam width
            int minArea = areas.Keys.Min();
            Face verticalFace = areas[minArea];
            BoundingBoxUV faceBB = verticalFace.GetBoundingBox();
            double beamWidth = faceBB.Max.V - faceBB.Min.V;

            XYZ location = null;
            XYZ beamDirection = null;

            BeamStartUVPoint(ele, webFace, out location, out beamDirection);

            BoundingBoxUV bboxUV = webFace.GetBoundingBox();
            UV center = (bboxUV.Max + bboxUV.Min) * 0.5;


            /*
            BoundingBoxUV bboxUV = webFace.GetBoundingBox();

            UV start = bboxUV.Min;
            UV end = bboxUV.Max;
            UV center = (bboxUV.Max + bboxUV.Min) * 0.5;
            double length = Math.Abs(bboxUV.Min.U - bboxUV.Max.U);

            double midV = Math.Abs(start.V - end.V) / 2;

            UV startUmidV = new UV(start.U, midV);

            XYZ location = webFace.Evaluate(startUmidV);

            XYZ normal = webFace.ComputeNormal(center);
            XYZ refDir = normal.CrossProduct(XYZ.BasisZ);


            UV endFace = new UV(bboxUV.Max.U, 0);
       

            switch (position)
            {
                case "start":
                    location = webFace.Evaluate(startUmidV);
                    break;
                case "end":
                    location = webFace.Evaluate(startUmidV);
                    newDistance = (webFace.Evaluate(endFace) - webFace.Evaluate(start)).GetLength() - distanceFromStart / 304.8 - (width / 304.8) * 0.5;
                    break;
                case "mid":
                    location = webFace.Evaluate(center);
                    newDistance = 0;
                    break;
            }
                 */


            switch (position)
            {
                case "start":
                    //location = webFace.Evaluate(startUmidV);
                    break;
                case "end":
                    //location = webFace.Evaluate(startUmidV);
                    BeamEndUVPoint(ele, webFace, out location, out beamDirection);
                    //newDistance = (webFace.Evaluate(endFace) - webFace.Evaluate(start)).GetLength() - distanceFromStart/304.8 - (width/304.8)*0.5;
                    break;
                case "mid":
                    location = webFace.Evaluate(center);
                    newDistance = 0;
                    break;
            }



            //FilteredElementCollector ope = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructConnections).WhereElementIsElementType();

            FilteredElementCollector ope = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).WhereElementIsElementType();

            //LocationCurve beamLine = ele.Location as LocationCurve;

            // XYZ beamDirection = beamLine.Curve.GetEndPoint(0) - beamLine.Curve.GetEndPoint(1);

            //XYZ location = beamLine.Curve.Evaluate(0.5,true);

            FamilySymbol fs = null;

            foreach (FamilySymbol f in ope)
            {
                if (f.FamilyName == FamilyName)
                    fs = f as FamilySymbol;
            }


            if (!fs.IsActive)
            { fs.Activate(); doc.Regenerate(); }

            FamilyInstance instance = null;

            // the element does not have available solid geometries. Need to use its geometry instance first and transform the point to the project coordinates.
            if (beamGeom.Count() == 1)
            {

                XYZ transformedPoint = instTransform.OfPoint(location);
                //LocationCurve lc = ele.Location as LocationCurve;
                //XYZ direction = lc.Curve.GetEndPoint(0) - lc.Curve.GetEndPoint(1);

                instance = doc.Create.NewFamilyInstance(webFace, transformedPoint, beamDirection, fs);
                SetRectVoidParamters(instance, newDistance, width, height, beamWidth);
            }

            else
            {

                instance = doc.Create.NewFamilyInstance(webFace, location, beamDirection, fs);
                SetRectVoidParamters(instance, newDistance, width, height, beamWidth);
            }

            return instance;
            //TaskDialog.Show("Position", location.X + "-" + location.Y + "-" + location.Z);

        }//close method

        public static void BeamStartUVPoint(Element beam, Face myFace, out XYZ choosenLocation, out XYZ beamDirection)
        {

            BoundingBoxUV bbox = myFace.GetBoundingBox();
            double halfDepthU = (bbox.Min.U + bbox.Max.U) * 0.5;
            double halfDepthV = (bbox.Min.V + bbox.Max.V) * 0.5;

            //faceLength = Math.Abs(bbox.Max - bbox.Min);

            UV start = bbox.Min;
            UV end = bbox.Max;

            //XYZ beamDirection = myFace.IsInside()

            LocationCurve beamLine = beam.Location as LocationCurve;

            XYZ stPt = beamLine.Curve.GetEndPoint(0);
            XYZ endPt = beamLine.Curve.GetEndPoint(1);

            List<UV> UVpoints = new List<UV>();

            UVpoints.Add(new UV(bbox.Max.U, halfDepthV));
            UVpoints.Add(new UV(bbox.Min.U, halfDepthV));

            UVpoints.Add(new UV(halfDepthU, bbox.Max.V));
            UVpoints.Add(new UV(halfDepthU, bbox.Min.V));

            Dictionary<double, XYZ> ptOnSurfaces = new Dictionary<double, XYZ>();

            foreach (UV pt in UVpoints)
            {
                XYZ point = myFace.Evaluate(pt);
                ptOnSurfaces.Add(stPt.DistanceTo(point), point);
            }

            double closestDistanceToStart = ptOnSurfaces.Keys.Min();

            choosenLocation = ptOnSurfaces[closestDistanceToStart];

            beamDirection = beamLine.Curve.GetEndPoint(0) - beamLine.Curve.GetEndPoint(1); //invert if beam End Point is chosen

        }//close method

        public static void BeamEndUVPoint(Element beam, Face myFace, out XYZ choosenLocation, out XYZ beamDirection)
        {

            BoundingBoxUV bbox = myFace.GetBoundingBox();
            double halfDepthU = (bbox.Min.U + bbox.Max.U) * 0.5;
            double halfDepthV = (bbox.Min.V + bbox.Max.V) * 0.5;

            //faceLength = Math.Abs(bbox.Max - bbox.Min);

            UV start = bbox.Min;
            UV end = bbox.Max;

            //XYZ beamDirection = myFace.IsInside()

            LocationCurve beamLine = beam.Location as LocationCurve;

            XYZ stPt = beamLine.Curve.GetEndPoint(0);
            XYZ endPt = beamLine.Curve.GetEndPoint(1);

            List<UV> UVpoints = new List<UV>();

            UVpoints.Add(new UV(bbox.Max.U, halfDepthV));
            UVpoints.Add(new UV(bbox.Min.U, halfDepthV));

            UVpoints.Add(new UV(halfDepthU, bbox.Max.V));
            UVpoints.Add(new UV(halfDepthU, bbox.Min.V));

            Dictionary<double, XYZ> ptOnSurfaces = new Dictionary<double, XYZ>();

            foreach (UV pt in UVpoints)
            {
                XYZ point = myFace.Evaluate(pt);
                ptOnSurfaces.Add(endPt.DistanceTo(point), point);
            }

            double closestDistanceToStart = ptOnSurfaces.Keys.Min();

            choosenLocation = ptOnSurfaces[closestDistanceToStart];

            beamDirection = beamLine.Curve.GetEndPoint(1) - beamLine.Curve.GetEndPoint(0); //invert if beam End Point is chosen

        }//close method

        public static FamilySymbol GetFamilySymbolByName(Document doc, string name)
        {
            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructConnections).WhereElementIsElementType();

            FamilySymbol fs = null;

            foreach (FamilySymbol f in fec)
            {
                if (f.FamilyName == name)
                    fs = f as FamilySymbol;

            }

            return fs;
        }

        public static int HasSymbolGeometry(Reference selectedElement, Document doc)
        {

            Element ele = doc.GetElement(selectedElement.ElementId);

            Options geomOptions = new Options();
            geomOptions.ComputeReferences = true;

            GeometryElement beamGeom = ele.get_Geometry(geomOptions);

            if (beamGeom.Count() == 1)
                return 1;
            else
                return 0;
        }

        private static void SetRectVoidParamters(FamilyInstance instance, double newDistance, int width, int height, double symbolH)
        {


            foreach (Parameter p in instance.Parameters)
            {
                if (p.Definition.Name == "P_Distance from Start")
                {
                    p.Set(newDistance);
                }
            }

            try
            {
                foreach (Parameter p in instance.Parameters)
                {
                    if (p.Definition.Name == "P_Void Width")
                    {
                        p.Set(width / 304.8);
                    }
                }
            }
            catch
            {
                //it must be a void, just set the height
            }


            foreach (Parameter p in instance.Parameters)
            {
                if (p.Definition.Name == "P_Void Depth")
                {
                    p.Set(height / 304.8);
                }
            }

            foreach (Parameter p in instance.Parameters)
            {
                if (p.Definition.Name == "P_Symbol Height")
                {
                    p.Set(symbolH);
                }
            }
        }//close macro

        public static void MoveEnd(Document doc, Reference beamToMove, Reference refSource)
        {

            Element eleToMove = doc.GetElement(beamToMove.ElementId);

            //Element location curve
            LocationCurve locCrv = eleToMove.Location as LocationCurve;

            Curve crv = locCrv.Curve;

            XYZ lineStart = crv.GetEndPoint(0);
            XYZ lineEnd = crv.GetEndPoint(1);

            Element eleSource = doc.GetElement(refSource.ElementId);

            LocationCurve sourceCurve = eleSource.Location as LocationCurve;

            Curve sourceCrv = sourceCurve.Curve;

            XYZ sourceStart = sourceCrv.GetEndPoint(0);
            XYZ sourceEnd = sourceCrv.GetEndPoint(1);

            double dist1 = lineStart.DistanceTo(sourceEnd);
            double dist2 = lineEnd.DistanceTo(sourceEnd);

            double dist3 = lineStart.DistanceTo(sourceStart);
            double dist4 = lineEnd.DistanceTo(sourceStart);

            Dictionary<string, double> dict = new Dictionary<string, double>();

            dict.Add("d1", dist1);
            dict.Add("d2", dist2);
            dict.Add("d3", dist3);
            dict.Add("d4", dist4);

            var val = dict.OrderBy(k => k.Value).FirstOrDefault();

            string keyR = val.Key;

            if (keyR == "d1")
            {
                Line newColumnLine = Line.CreateBound(sourceEnd, lineEnd);
                locCrv.Curve = newColumnLine;
            }
            else if (keyR == "d2")
            {
                Line newColumnLine = Line.CreateBound(lineStart, sourceEnd);
                locCrv.Curve = newColumnLine;
            }
            else if (keyR == "d3")
            {
                Line newColumnLine = Line.CreateBound(sourceStart, lineEnd);
                locCrv.Curve = newColumnLine;
            }
            else if (keyR == "d4")
            {
                Line newColumnLine = Line.CreateBound(lineStart, sourceStart);
                locCrv.Curve = newColumnLine;
            }
            else
            {
                TaskDialog.Show("Error", "Error");
            }


        }//close method

        #endregion

        #region GEOMETRY

        public void SetBeamLocationCurves(UIDocument uidoc)
        {


            Document doc = uidoc.Document;

            ICollection<Reference> selectedLines = uidoc.Selection.PickObjects(ObjectType.Element, "Select Lines");

            ICollection<Reference> selectedBeams = uidoc.Selection.PickObjects(ObjectType.Element, "Select Beams");

            using (Transaction t = new Transaction(doc, "Change beams"))
            {

                t.Start();

                for (int i = 0; i < selectedBeams.Count; i++)
                {

                    Element ele = doc.GetElement(selectedBeams.ElementAt(i).ElementId);
                    Element newLine = doc.GetElement(selectedLines.ElementAt(i).ElementId);
                    (ele.Location as LocationCurve).Curve = (newLine.Location as LocationCurve).Curve;
                }
                t.Commit();
            }//close using
        }//close method



        public void ProjectLines(UIDocument uidoc)
        {


            Document doc = uidoc.Document;

            //ICollection<Reference> selectedLines = uidoc.Selection.PickObjects(ObjectType.Element, "Select Lines");

            Reference refFace = uidoc.Selection.PickObject(ObjectType.Face, "Select Surface");
            Element selectedElement = doc.GetElement(refFace);
            GeometryObject selectedGeoObject = selectedElement.GetGeometryObjectFromReference(refFace);

            Face selectedFace = selectedGeoObject as Face;

            Reference refLine = uidoc.Selection.PickObject(ObjectType.Element, "Select Line");

            LocationCurve locCurve = doc.GetElement(refLine.ElementId).Location as LocationCurve;
            Line line = locCurve.Curve as Line;

            using (Transaction t = new Transaction(doc, "test"))
            {

                t.Start();


                //SketchPlane splane = SketchPlane.Create(doc, refFace);

                XYZ q = line.GetEndPoint(1);
                XYZ p = line.GetEndPoint(0);

                XYZ v = q - p;

                double dxy = Math.Abs(v.X) + Math.Abs(v.Y);

                XYZ w = XYZ.BasisZ;
                XYZ norm = v.CrossProduct(w).Normalize();


                XYZ rayDirection = new XYZ(0, 0, 1);

                XYZ normal = line.Direction.CrossProduct(rayDirection);

                Plane verticalPlane = Plane.CreateByNormalAndOrigin(normal, p);

                SketchPlane splane = SketchPlane.Create(doc, verticalPlane);

                doc.ActiveView.SketchPlane = splane;

                //        		XYZ newStPt = ProjectOnto(splane.GetPlane(), line.GetEndPoint(0));
                //        		XYZ newEndPt = ProjectOnto(splane.GetPlane(), line.GetEndPoint(1));

                //Line projectedLine = Autodesk.Revit.DB.Line.CreateBound(newStPt, newEndPt);

                //        		Line verticalLine = Autodesk.Revit.DB.Line.CreateBound(line.GetEndPoint(1), new XYZ (pt.X, pt.Y, pt.Z+1000));

                View3D active3D = doc.ActiveView as View3D;

                ReferenceIntersector refIntersector = new ReferenceIntersector(refFace.ElementId, FindReferenceTarget.Face, active3D);

                ReferenceWithContext referenceWithContext = refIntersector.FindNearest(q, rayDirection);

                Reference reference = referenceWithContext.GetReference();
                XYZ intersection = reference.GlobalPoint;

                Line verticalLine = Autodesk.Revit.DB.Line.CreateBound(p, intersection);

                //ModelLine mline = doc.Create.NewModelCurve(projectedLine , splane) as ModelLine;

                ModelLine verticalmLine = doc.Create.NewModelCurve(verticalLine, splane) as ModelLine;

                //selectedFace.Intersect(selectedNewFace, out CurveResult);
                //selectedNewFace.Intersect(selectedFace, out CurveResult);

                t.Commit();
            }



        }//close method
        #endregion

        #region PERFORMANCE ADVISER

        public static PerformanceAdviserRuleId GetPerformanceRuleByName(IList<PerformanceAdviserRuleId> listAllRuleIds, string purgeGuid)
        {
            PerformanceAdviserRuleId selectedRule = null;

            foreach (PerformanceAdviserRuleId ruleId in listAllRuleIds)
            {
                if (ruleId.Guid.ToString().Equals(purgeGuid))
                {
                    return ruleId;
                }
            }

            return selectedRule;
        }

        public static int CountPurgeableElements(Document doc)
        {
            //The guid of the 'Project contains unused families and types' PerformanceAdviserRuleId.
            string PurgeGuid = "e8c63650-70b7-435a-9010-ec97660c1bda";

            PerformanceAdviser adviser = PerformanceAdviser.GetPerformanceAdviser();

            IList<PerformanceAdviserRuleId> selectedRuleIds = new List<PerformanceAdviserRuleId>();

            ICollection<ElementId> purgeableElem = new List<ElementId>();

            IList<PerformanceAdviserRuleId> allRuleIds = adviser.GetAllRuleIds();

            selectedRuleIds.Add(GetPerformanceRuleByName(allRuleIds, PurgeGuid));

            IList<FailureMessage> failureMessages = PerformanceAdviser.GetPerformanceAdviser().ExecuteRules(doc, selectedRuleIds);

            if (failureMessages.Count > 0)
            {
                purgeableElem = failureMessages.ElementAt(0).GetFailingElements();
            }

            return purgeableElem.Count;
        }


        #endregion

        #region MANAGE MODEL

        public static bool InsertData(string tableName, DateTime dt, string user, long rvtFileSize, int elementsCount, int typesCount, int sheetsCount, int viewsCount, int viewportsCount, int countWarnings, int purgeableElements, int viewsNotOnSheet)
        {


            string server = "127.0.0.1";
            string database = "new_schema";
            string uid = "root";
            string password = "password";


            /*
            string server = "remotemysql.com";
            string database = "r7BFoOjCty";
            string uid = "r7BFoOjCty";
            string password = "1vU3s1bj6T";
    */
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";


            // string table = "filesize";
            string table = tableName;


            try
            {

                MySqlConnection connection = new MySqlConnection(connectionString);

                MySqlCommand cmdInsert = new MySqlCommand("", connection);
                cmdInsert.CommandText = "INSERT INTO " + table + " (date, user, rvtFileSize, elementsCount, typesCount, sheetsCount, viewsCount, viewportsCount, warningsCount, purgeableElements, viewsNotOnSheet) " +
                    "VALUES (?date, ?user, ?rvtFileSize, ?elementsCount, ?typesCount, ?sheetsCount, ?viewsCount, " +
                    "?viewportsCount, ?warningsCount, ?purgeableElements, ?viewsNotOnSheet)";

                cmdInsert.Parameters.Add("?date", MySqlDbType.DateTime).Value = dt;
                cmdInsert.Parameters.Add("?user", MySqlDbType.VarChar).Value = user;
                cmdInsert.Parameters.Add("?rvtFileSize", MySqlDbType.Int64).Value = rvtFileSize;
                cmdInsert.Parameters.Add("?elementsCount", MySqlDbType.Int32).Value = elementsCount;
                cmdInsert.Parameters.Add("?typesCount", MySqlDbType.Int32).Value = typesCount;

                cmdInsert.Parameters.Add("?sheetsCount", MySqlDbType.Int32).Value = sheetsCount;
                cmdInsert.Parameters.Add("?viewsCount", MySqlDbType.Int32).Value = viewsCount;
                cmdInsert.Parameters.Add("?viewportsCount", MySqlDbType.Int32).Value = viewportsCount;

                cmdInsert.Parameters.Add("?warningsCount", MySqlDbType.Int32).Value = countWarnings;

                cmdInsert.Parameters.Add("?purgeableElements", MySqlDbType.Int32).Value = purgeableElements;

                cmdInsert.Parameters.Add("?viewsNotOnSheet", MySqlDbType.Int32).Value = viewsNotOnSheet;


                connection.Open();

                cmdInsert.ExecuteNonQuery();

                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return false;
            }


        }//close method

        public struct CardContent
        {
            public int Value;
            public string Content;

        }

        public static Dictionary<string, CardContent> ModelStatus(Document doc)
        {

            FilteredElementCollector fecElements = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            FilteredElementCollector fecTypes = new FilteredElementCollector(doc).WhereElementIsElementType();

            FilteredElementCollector fecSheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();
            FilteredElementCollector fecViews = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();
            FilteredElementCollector fecViewPorts = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewports).WhereElementIsNotElementType();

            ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(doc.PathName);

            Dictionary<string, int> linksElements = ListLinks(modelPath);

            Dictionary<string, int> importElements = ListImports(doc);

            int countImports = 0;

            importElements.ToList().ForEach(i => countImports += i.Value);

            FilteredWorksetCollector coll = new FilteredWorksetCollector(doc);

            coll.OfKind(WorksetKind.UserWorkset);

            int worksetCount = 0;

            StringBuilder sbWorkset = new StringBuilder();

            foreach (Workset workset in coll)
            {
                sbWorkset.AppendLine(workset.Name);
                worksetCount += 1;
            }


#if REVIT2017
            int countWarnings = 0;
#else
            int countWarnings = doc.GetWarnings().Count;
#endif
            int countElements = fecElements.Count();
            int countTypes = fecTypes.Count();

            int countSheets = fecSheets.Count();
            int countViews = fecViews.Count();
            int countViewPorts = fecViewPorts.Count();
            List<CardContent> viewNotOnSheet = CountViewsNotOnSheet(fecViews);

            List<string> viewNotOnSheetsNames = new List<string>();


            foreach (var card in viewNotOnSheet)
            {
                viewNotOnSheetsNames.Add(card.Content);
            }

            viewNotOnSheetsNames.Sort();

            StringBuilder sb = new StringBuilder();

            viewNotOnSheetsNames.ToList().ForEach(i => sb.AppendLine(i));

            //TaskDialog.Show("re", sb.ToString());

            Dictionary<string, CardContent> dashboardDictionary = new Dictionary<string, CardContent>();

            dashboardDictionary.Add("WARNINGS", new CardContent() { Value = countWarnings, Content = "N/A" });
            dashboardDictionary.Add("ELEMENTS", new CardContent() { Value = countElements, Content = "N/A" });
            dashboardDictionary.Add("ELEMENT TYPES", new CardContent() { Value = countTypes, Content = "N/A" });
            dashboardDictionary.Add("SHEETS", new CardContent() { Value = countSheets, Content = "N/A" });
            dashboardDictionary.Add("VIEWS", new CardContent() { Value = countViews, Content = "N/A" });
            dashboardDictionary.Add("VIEWPORTS", new CardContent() { Value = countViewPorts, Content = "N/A" });
            dashboardDictionary.Add("VIEWS NOT ON SHEETS", new CardContent() { Value = viewNotOnSheet.Count, Content = sb.ToString() });
            dashboardDictionary.Add("CAD IMPORTS", new CardContent() { Value = countImports, Content = "N/A" });
            dashboardDictionary.Add("REVIT LINKS", new CardContent() { Value = linksElements["Revit Link"], Content = "N/A" });
            dashboardDictionary.Add("CAD LINKS", new CardContent() { Value = linksElements["CAD Link"], Content = "N/A" });
            dashboardDictionary.Add("WORKSETS", new CardContent() { Value = worksetCount, Content = sbWorkset.ToString() });

            return dashboardDictionary;
        }

        private static string ImportCategoryNameToFileName(string catName)
        {
            string fileName = catName;
            fileName = fileName.Trim();

            if (fileName.EndsWith(")"))
            {
                int lastLeftBracket = fileName.LastIndexOf("(");

                if (-1 != lastLeftBracket)
                    fileName = fileName.Remove(lastLeftBracket); // remove left bracket
            }

            return fileName.Trim();
        }

        private static Dictionary<string, int> ListImports(Document doc)
        {
            List<KeyValuePair<string, string>> listOfViewSpecificImports = new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> listOfModelImports = new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> listOfUnidentifiedImports = new List<KeyValuePair<string, string>>();

            Dictionary<string, int> results = new Dictionary<string, int>();

            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));

            foreach (Element e in col)
            {
                if (e.Category != null)
                {
                    if (e.ViewSpecific)
                    {
                        string viewName = null;

                        try
                        {
                            Element viewElement = doc.GetElement(e.OwnerViewId);
                            viewName = viewElement.Name;
                        }
                        catch (Autodesk.Revit.Exceptions
                          .ArgumentNullException) // just in case
                        {
                            viewName = String.Concat(
                              "Invalid View ID: ",
                              e.OwnerViewId.ToString());
                        }


                        if (null != e.Category)
                        {
                            try
                            {
                                listOfViewSpecificImports.Add(new KeyValuePair<string, string>(viewName, ImportCategoryNameToFileName(e.Category.Name)));
                            }
                            catch { }
                        }


                        else
                        {
                            try
                            {
                                listOfUnidentifiedImports.Add(new KeyValuePair<string, string>(viewName, e.Id.ToString()));
                            }
                            catch { }

                        }

                    }

                    else
                    {
                        try
                        {
                            listOfModelImports.Add(new KeyValuePair<string, string>(e.Name, ImportCategoryNameToFileName(e.Category.Name)));
                        }
                        catch { }

                    }

                }
                else
                {
                    //TaskDialog.Show("result",e.Id.ToString());
                }

            }

            results.Add("View Specific", listOfViewSpecificImports.Count());
            results.Add("Unidentified Imports", listOfUnidentifiedImports.Count());
            results.Add("Model Imports", listOfModelImports.Count());

            return results;

        }

        private static Dictionary<string, int> ListLinks(ModelPath location)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();

            int countRevLinks = 0;
            int countCadLink = 0;

            string path = ModelPathUtils.ConvertModelPathToUserVisiblePath(location);

            // access transmission data in the given Revit file

            TransmissionData transData = TransmissionData.ReadTransmissionData(location);

            if (transData == null)
            {

            }
            else
            {
                // collect all (immediate) external references in the model

                ICollection<ElementId> externalReferences = transData.GetAllExternalFileReferenceIds();

                // find every reference that is a link

                foreach (ElementId refId in externalReferences)
                {
                    ExternalFileReference extRef = transData.GetLastSavedReferenceData(refId);

                    if (extRef.ExternalFileReferenceType == ExternalFileReferenceType.RevitLink)
                    {
                        countRevLinks += 1;
                    }
                    else if (extRef.ExternalFileReferenceType == ExternalFileReferenceType.CADLink)
                    {
                        countCadLink += 1;
                    }
                }
            }

            results.Add("Revit Link", countRevLinks);
            results.Add("CAD Link", countCadLink);

            return results;

        }

        public static async Task CreateRecord(AirtableBase airtableBase, string tableName, Fields fields)
        {
            //var attachmentList = new List<AirtableAttachment>();
            //attachmentList.Add(new AirtableAttachment { Url = "https://upload.wikimedia.org/wikipedia/en/d/d1/Picasso_three_musicians_moma_2006.jpg" });

            Task<AirtableCreateUpdateReplaceRecordResponse> task = airtableBase.CreateRecord(tableName, fields, true);

            //This never get a response back
            var response = await task;

            //if (!response.Success)
            //{
            //    TaskDialog.Show("Error", response.AirtableApiError.ErrorMessage);
            //    //Console.WriteLine(response.AirtableApiError.ErrorMessage);
            //}
            //else
            //{
            //    //Console.WriteLine("Record created");
            //}
        }

        public static async Task GetRecords(AirtableBase airtableBase, string tableName, List<AirtableRecord> records, string errorMessage)
        {
            //
            // Use 'offset' and 'pageSize' to specify the records that you want
            // to retrieve.
            // Only use a 'do while' loop if you want to get multiple pages
            // of records.
            //
            IEnumerable<string> fields = null;
            string filterByFormula = null;
            int? maxRecords = null;
            int? pageSize = null;
            IEnumerable<Sort> sort = null;
            string view = null;
            string offset = null;

            do
            {
                Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                       tableName,
                       offset,
                       fields,
                       filterByFormula,
                       maxRecords,
                       pageSize,
                       sort,
                       view);

                AirtableListRecordsResponse response = await task;

                if (response.Success)
                {
                    records.AddRange(response.Records.ToList());
                    offset = response.Offset;
                    foreach (var item in response.Records)
                    {
                        Console.WriteLine(item.Fields["Id"]);
                    }

                }
                else if (response.AirtableApiError is AirtableApiException)
                {
                    errorMessage = response.AirtableApiError.ErrorMessage;
                    Console.WriteLine(errorMessage);
                    break;
                }
                else
                {
                    errorMessage = "Unknown error";
                    Console.WriteLine(errorMessage);
                    break;
                }
            } while (offset != null);
        }
        
        public static List<string> GetAirtableKeys(string configPath)
        {
            List<string> listA = new List<string>();

            using (var reader = new StreamReader(configPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    listA.Add(values[1].Trim());
                }
            }

            string baseId = listA[0];
            string appKey = listA[1];

            return listA;
        }
        
#endregion

        private static void PaintFace()
        {

            /*			
			IList<Element> materials = new FilteredElementCollector(doc).OfClass(typeof(Material)).ToElements();

			Material matName = null;
			 	
			foreach (Material mat in materials) {
				if (mat.Name == "Glass")
				{
					matName = mat;
				}
			}
			
			doc.Paint(ele.Id, webFace, matName.Id);
			*/
        }

        public static string SetTemporaryMark(Document doc, ElementId elid)
        {
            string tag = MarkGenerator(doc, elid);
            Parameter p = doc.GetElement(elid).LookupParameter("Mark");
            //form.originalMarks.append(p.AsString())
            p.Set(string.Format("{0} (new: {1})", p.AsString(), tag));
            return tag;
        }

        public static string GetMark(Document doc, ElementId eid)
        {
            Parameter p = doc.GetElement(eid).LookupParameter("Mark");
            return p.AsString();
        }

        public static string MarkGenerator(Document doc, ElementId beamId)
        {
            Dictionary<string, string> usage = new Dictionary<string, string>(){
                {"Primary", "PB"},
                {"Secondary", "SB"},
                {"Vertical Bracing", "VB"},
                {"Diaphragm", "DB"}
            };

            Element beam = doc.GetElement(beamId);
            LocationCurve lc = beam.Location as LocationCurve;
            double xDist = Math.Abs((lc.Curve.Evaluate(0.5, true).X * 304.8 + 67286.598) / 1000);
            double yDist = Math.Abs((lc.Curve.Evaluate(0.5, true).Y * 304.8 - 559.835) / 1000);
            ElementId lvlId = beam.LookupParameter("Reference Level").AsElementId();
            string lvlName = doc.GetElement(lvlId).Name;
            string[] s = lvlName.Split(' ');
            if (s[1] == "Ground")
            {
                s[1] = "UG";
            }

            string usageValue = string.Empty;

            try
            {
                usageValue = usage[beam.LookupParameter("Str Framing Use").AsString()];
            }

            catch
            {
                usageValue = "Usage error";
            }

            string markValue = string.Empty;

            if (xDist < 9.5 && yDist < 10)
            {
                markValue = string.Format("{0}{1}0{2:0}0{3:0}", s[1], usageValue, xDist, yDist);
            }

            else if (xDist < 9.5)
            {
                markValue = string.Format("{0}{1}0{2:0}{3:0}", s[1], usageValue, xDist, yDist);
            }
            else if (yDist < 9.5)
            {
                markValue = string.Format("{0}{1}{2:0}0{3:0}", s[1], usageValue, xDist, yDist);
            }
            else
            {
                markValue = string.Format("{0}{1}{2:0}{3:0}", s[1], usageValue, xDist, yDist);

            }
            //TaskDialog.Show("result", usageValue);
            return markValue;

        }

        public static void assignMark(Document doc, ElementId eid, string markValue)
        {
            Parameter p = doc.GetElement(eid).LookupParameter("Mark");
            p.Set(markValue);
        }


        public static void OverrideColor(Document doc, ElementId eid, string oldMark, string newMark)
        {

            View view = doc.ActiveView;
            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();

            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement));

            ElementId solidPatternId = null;
            //ElementId solidPatternId = new ElementId(19);

            foreach (Element pattern in fec)
            {
                if (pattern.Name == "Solid fill")
                {
                    solidPatternId = pattern.Id;
                }
            }



#if REVIT2019

            overrideSettings.SetSurfaceForegroundPatternId(solidPatternId);

            if (oldMark == newMark)
            {
                overrideSettings.SetProjectionLineColor(new Color(0, 255, 0));
            }
            else if (oldMark == "")
            {
                overrideSettings.SetProjectionLineColor(new Color(255, 255, 0));
            }
            else
            {
                overrideSettings.SetProjectionLineColor(new Color(255, 0, 0));
            }

#else
            overrideSettings.SetProjectionFillPatternId(solidPatternId);
            
            if (oldMark == newMark)
            {
                overrideSettings.SetProjectionFillColor(new Color(0, 255, 0));
            }
            else if (oldMark == "")
            {
                overrideSettings.SetProjectionFillColor(new Color(255, 255, 0));
            }
            else
            {
                overrideSettings.SetProjectionFillColor(new Color(255, 0, 0));
            }

#endif


            try
            {
                view.SetElementOverrides(eid, overrideSettings);
                //TaskDialog.Show("Result", string.Format("oldMark: {0}newMark: {1}. are they the same? {2}", oldMark, newMark, oldMark==newMark));
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Catch", "Undo changes failed due to:" + Environment.NewLine + ex.Message);
            }

        }

        public static void ResetOverrideColor(ElementId eid, Document doc)
        {
            View v = doc.ActiveView;
            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();
            v.SetElementOverrides(eid, overrideSettings);
        }


    }

    public class GenericSelectionFilter : ISelectionFilter
    {

        public string catNameChosen { get; set; }

        public GenericSelectionFilter(string catName)
        {
            this.catNameChosen = catName;
        }

        public bool AllowElement(Element e)
        {
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
}
