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
    public class UpdateSuscribe : IExternalCommand
    {
        private static UIApplication uiapp;
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            bool registered = false;
            try
            {
                if (registered == false)
                {
                    uiapp.ViewActivated += new EventHandler<ViewActivatedEventArgs>(My_Application_ViewActivated);
                    registered = true;
                }

                //uiapp.ApplicationClosing += new EventHandler<ApplicationClosingEventArgs>(Doc_DocumentClosing);
                doc.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(Doc_DocumentClosing);
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
            //sender is null and causes error
            //UIApplication uiapp = sender as UIApplication;

            uiapp.ViewActivated -= My_Application_ViewActivated;
            
            Debug.WriteLine("Unsuscribed");
        }

        private int tabCount = 0;
        private List<string> tabProjectNames = new List<string>();
        private List<SolidColorBrush> tabProjectColors = new List<SolidColorBrush> { Brushes.Coral, Brushes.RoyalBlue, Brushes.DeepPink, Brushes.SeaGreen, Brushes.Yellow, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Red, Brushes.Violet };

        public void My_Application_ViewActivated(object sender, ViewActivatedEventArgs args)
        {
            Debug.WriteLine("Test View Activated");
            UIApplication uiapp = sender as UIApplication;

            Debug.WriteLine(uiapp.ActiveUIDocument.Document.PathName);

            var wndRoot = GetWindowRoot(uiapp);

            Debug.WriteLine("docTabGroup");

            

        }

        public static IEnumerable<TabItem> GetDocumentTabs(LayoutDocumentPaneGroupControl docPane)
        {
            if (docPane != null)
            {
                return docPane.FindVisualChildren<TabItem>();
            }
            return new List<TabItem>();
        }

        public static LayoutDocumentPaneGroupControl GetDocumentTabGroup(UIApplication uiapp)
        {
            Debug.Write("Get document tab group");
            var wndRoot = GetWindowRoot(uiapp);
            Debug.WriteLine(wndRoot);

            if (wndRoot != null)
            {
                return MainWindow.FindFirstChild<LayoutDocumentPaneGroupControl>((MainWindow)wndRoot);

            }
            return null;
        }

        public static Visual GetWindowRoot(UIApplication uiapp)
        {
            IntPtr wndHndle = IntPtr.Zero;

            wndHndle = uiapp.MainWindowHandle;
            
           

            if (wndHndle != IntPtr.Zero)
            {
                var wndSource = HwndSource.FromHwnd(wndHndle);
                return wndSource.RootVisual;
            }
            return null;
        }
    }
}
