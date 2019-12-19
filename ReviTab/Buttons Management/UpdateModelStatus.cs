using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class UpdateModelStatus : IExternalCommand
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

            try
            {

                FilteredElementCollector fecElements = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                FilteredElementCollector fecTypes = new FilteredElementCollector(doc).WhereElementIsElementType();

                FilteredElementCollector fecSheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();
                FilteredElementCollector fecViews = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType();
                FilteredElementCollector fecViewPorts = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Viewports).WhereElementIsNotElementType();

                ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(doc.PathName);

                Dictionary<string, int> linksElements = ListLinks(modelPath);

                Dictionary<string, int> importElements = listImports(doc);

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

                int countWarnings = doc.GetWarnings().Count;

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


                IEnumerable<Element> fecDashboardFamilies = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .Where(x => x.Name == "Single Value");

                IEnumerable<Element> fecDashboardDate = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .Where(x => x.Name == "Dashboard Date");


                using (Transaction t = new Transaction(doc, "Update Dashboard"))
                {

                    t.Start();

                    foreach (var dict in dashboardDictionary)
                    {
                        Element e = fecDashboardFamilies.Where(x => x.LookupParameter("Name").AsString() == dict.Key).First();
                        e.LookupParameter("Content").Set("N/A");

                        e.LookupParameter("Old Value").Set(e.LookupParameter("Current Value").AsInteger());

                        e.LookupParameter("Current Value").Set(dict.Value.Value);
                        e.LookupParameter("Content").Set(dict.Value.Content);
                    }


                    Element dateFamily = fecDashboardDate.First();

                    dateFamily.LookupParameter("Old Value").Set(dateFamily.LookupParameter("Current").AsString());
                    dateFamily.LookupParameter("Current").Set(DateTime.Now.ToShortDateString());


                    t.Commit();
                }

                TaskDialog.Show("Model Updated", "Done");

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);

                return Result.Failed;
            }
        }

        private struct CardContent
        {
            public int Value;
            public string Content;

        }

        private static List<CardContent> CountViewsNotOnSheet(FilteredElementCollector allViews)
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

        private static string importCategoryNameToFileName(string catName)
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

        private static Dictionary<string, int> listImports(Document doc)
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
                                listOfViewSpecificImports.Add(new KeyValuePair<string, string>(viewName, importCategoryNameToFileName(e.Category.Name)));
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
                            listOfModelImports.Add(new KeyValuePair<string, string>(e.Name, importCategoryNameToFileName(e.Category.Name)));
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

        private Dictionary<string, int> ListLinks(ModelPath location)
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


    }

}