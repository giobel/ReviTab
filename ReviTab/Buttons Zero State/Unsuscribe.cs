
using System;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Media;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using UIFramework;

using Xceed.Wpf.AvalonDock.Controls;
using Autodesk.Revit.Attributes;
using System.Diagnostics;

namespace ReviTab
{
    /// <summary>
    /// All credits to pyrevit https://github.com/eirannejad/pyRevit/blob/4afd56ccb4d77e4e0228b8e64d80d1f541bc791e/pyrevitlib/pyrevit/runtime/EventHandling.cs
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Unsuscribe : IExternalCommand
    {
        //private int tabCount = 0;
        private List<string> tabProjectNames = new List<string>();
        private List<SolidColorBrush> tabProjectColors = new List<SolidColorBrush> { Brushes.Coral, Brushes.RoyalBlue, Brushes.DeepPink, Brushes.SeaGreen, Brushes.Yellow, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Red, Brushes.Violet };


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
                uiapp.ViewActivated += new EventHandler<ViewActivatedEventArgs>(My_Application_ViewActivated);
                return Result.Succeeded;

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

        }

        private void Doc_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            Debug.WriteLine("Document closing");

            UIApplication uiapp = sender as UIApplication;

            uiapp.ViewActivated -= My_Application_ViewActivated;

            Debug.WriteLine("Unsuscribed");
        }

        public void My_Application_ViewActivated(object sender, ViewActivatedEventArgs args)
        {
            Debug.WriteLine("Test View Activated");
        }


    }
}
