using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReviTab
{

[Transaction(TransactionMode.Manual)]
    public class AddPDFcustomProperties : IExternalCommand
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

            int changeCounter = 0;

            try
            {
                using (var formOpen = new FormAddMetadata())
                {

                    formOpen.ShowDialog();

                    FilteredElementCollector allSheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

                    string folderPath = formOpen.filePath;

                    FileInfo[] allPdfs = AddMetadataHelpers.GetDirectoryContent(folderPath, "*.pdf");

                    

                    foreach (FileInfo pdfName in allPdfs)
                    {

                        ViewSheet currentSheet = AddMetadataHelpers.MatchSheet(allSheets, pdfName.FullName);
                        if (currentSheet != null)
                        {
                            Parameter paramRevision = currentSheet.LookupParameter("ARUP_BDR_ISSUE");
                            Parameter paramStatus = currentSheet.LookupParameter("ARUP_BDR_STATUS");
                            Parameter paramIssueDate = currentSheet.LookupParameter("Sheet Issue Date");

                            Dictionary<string, string> paramNameValue = new Dictionary<string, string>();

                            paramNameValue.Add("Sheet Name", currentSheet.Name);
                            paramNameValue.Add("Revision", paramRevision.AsString());
                            paramNameValue.Add("Status", paramStatus.AsString());
                            paramNameValue.Add("Issue Date", paramIssueDate.AsString());

                            AddMetadataHelpers.AddMetadata(pdfName.FullName, paramNameValue);

                            changeCounter += 1;
                        }

                    }


                    



                }

                TaskDialog.Show("Well Done", String.Format("{0} pdf(s) have been processed", changeCounter));
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }


        }
    }
}
