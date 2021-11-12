using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using winForm = System.Windows.Forms;


namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class OverrideColors : IExternalCommand
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

			FilteredElementCollector elementsInView = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementsInView.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

			List<BuiltInCategory> builtInCats = new List<BuiltInCategory>();
			builtInCats.Add(BuiltInCategory.OST_StructuralFraming);
			builtInCats.Add(BuiltInCategory.OST_Walls);
			builtInCats.Add(BuiltInCategory.OST_Floors);
			builtInCats.Add(BuiltInCategory.OST_StructuralColumns);
			builtInCats.Add(BuiltInCategory.OST_StructuralFoundation);

			ElementMulticategoryFilter filter1 = new ElementMulticategoryFilter(builtInCats);

			IList<Element> allElementsInView = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filter1).WhereElementIsNotElementType().ToElements();

			var grouped = allElementsInView.GroupBy(x => x.GetTypeId());

			Random pRand = new Random();

            var words = ("She sells sea shells on the sea shore but the sea " +
             "shells she sells are sea shells no more.").Split(' ');
            var md5 = MD5.Create();

            //TaskDialog.Show("r", grouped.First().First().Name);
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
#if REVIT2020 || REVIT2021
				ogs.SetSurfaceForegroundPatternId(solidFillPattern.Id);            
#else
            ogs.SetProjectionFillPatternId(solidFillPattern.Id);
#endif

            using (Transaction t = new Transaction(doc, "Override Colors"))
			{
				t.Start();
				foreach (var element in grouped)
				{


                    var value = element.First() ;
                        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value.Name));
                    //var color = Color.FromArgb(hash[0], hash[1], hash[2]);

                    byte iR, iG, iB;
					iR = Convert.ToByte(pRand.Next(0, 255));
					iG = Convert.ToByte(pRand.Next(0, 255));
					iB = Convert.ToByte(pRand.Next(0, 255));
                    //Autodesk.Revit.DB.Color pcolor = new Autodesk.Revit.DB.Color(iR, iG, iB);

                    Autodesk.Revit.DB.Color pcolor = new Autodesk.Revit.DB.Color(hash[0], hash[1], hash[2]);

                    #if REVIT2020 || REVIT2021
                    ogs.SetSurfaceForegroundPatternColor(pcolor);
					#else
						ogs.SetProjectionFillColor(pcolor);
					#endif
					try
                    {
						foreach (FamilyInstance item in element)
						{
							doc.ActiveView.SetElementOverrides(item.Id, ogs);

						}

					}
                    catch
                    {

                    }
				}

				t.Commit();
			}



			return Result.Succeeded;
            
        }
    }
}
