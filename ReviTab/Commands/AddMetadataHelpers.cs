using Autodesk.Revit.DB;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviTab
{
    class AddMetadataHelpers
    {
        public static void AddMetadata(string pdfPath, Dictionary<string, string> propertyNameValue)
        {

            PdfDocument document = PdfReader.Open(pdfPath);

            var properties = document.Info.Elements;


            foreach (var element in propertyNameValue)
            {

                if (properties.ContainsKey("/" + element.Key))
                {
                    properties.SetValue("/" + element.Key, new PdfString(element.Value));
                }
                else
                {
                    properties.Add(new KeyValuePair<String, PdfItem>("/" + element.Key, new PdfString(element.Value)));
                }


            }


            document.Save(pdfPath);

            document.Close();
        }

        public static ViewSheet MatchSheet(FilteredElementCollector _allSheets, string _pdfSheetNumber)
        {

            foreach (ViewSheet sheet in _allSheets)
            {

                if (_pdfSheetNumber.Contains(sheet.SheetNumber))
                {
                    return sheet;
                }

            }

            return null;
        }


        public static FileInfo[] GetDirectoryContent(string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            return dinfo.GetFiles(FileType);
        }

    }
}

