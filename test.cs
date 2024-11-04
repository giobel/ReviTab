#region Namespaces
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Numerics;
#endregion

namespace RevitTest
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public static UIDocument _activeRevitUIDoc { get; private set; }

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

			FamilySymbol fs = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).First(q => q.Name.Equals("Family1")) as FamilySymbol;
            
			Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).ToElements().First() as Level;

            Options opt = new Options();

            IList<Reference> linkModelRefs = uidoc.Selection.PickObjects(ObjectType.LinkedElement, "Select Elements");
			
			using (Transaction t = new Transaction(doc, "Copy Linked Elements"))
            {

                t.Start();
                
                foreach (var linkModelRef in linkModelRefs) {

                	var e = doc.GetElement(linkModelRef.ElementId);
                	RevitLinkInstance revitLinkInst = e as RevitLinkInstance;
                	Document linkRvtDoc = (e as RevitLinkInstance).GetLinkDocument();
                    Transform transf = revitLinkInst.GetTransform();
                    
                    Element eLinked = linkRvtDoc.GetElement(linkModelRef.LinkedElementId);
                    
                    GeometryElement fiGeometry = eLinked.get_Geometry(opt);
                    
                    List<XYZ> points = new List<XYZ>();
                    
                    foreach (GeometryObject geoObj in fiGeometry)
		                {
                    	if (geoObj is Solid){
		                    	TaskDialog.Show("R", geoObj.ToString());  
		                    	
		                    	List<Face> allFaces = new List<Face>();
		                    	
								Solid solid = geoObj as Solid;		    
								foreach (Face face in solid.Faces)
			                        {
									allFaces.Add(face);
									
									foreach (var v in face.Triangulate().Vertices) {
										points.Add(v);
									}
									
			                        }								
                    	}
		                }
                    
                        XYZ centroid = XYZ.Zero;

						    foreach (XYZ point in points) {
						      centroid += point;
						    }
						
						    centroid /= points.Count;
						
						    XYZ direction = points[1] - points[0];

                            for(int i = 0; i < 100; i++){
						
						      XYZ nextDirection = XYZ.Zero;
						      
						      foreach (XYZ point in points) {
						      
						      	XYZ centeredPoint = point - centroid;
						      
						      	double dp = direction.DotProduct(centeredPoint);
						      
						      	nextDirection += dp * centeredPoint;
						      }
						      nextDirection.Normalize();
						      direction = nextDirection;
						    }
                                              
				    FamilyInstance dsOrigin = doc.Create.NewFamilyInstance(centroid, fs, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

				    FamilyInstance dsDirection = doc.Create.NewFamilyInstance(transf.OfPoint(direction), fs, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
				    
				    
				    TaskDialog.Show("R", string.Format("{0},{1},{2}",centroid.X, centroid.Y, centroid.Z));

                }
                t.Commit();
                
			}

            return Result.Succeeded;
        }


    


}//close class
}
