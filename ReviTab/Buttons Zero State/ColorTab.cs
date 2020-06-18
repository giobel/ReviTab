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

namespace ReviTab
{
    /// <summary>
    /// All credits to pyrevit https://github.com/eirannejad/pyRevit/blob/4afd56ccb4d77e4e0228b8e64d80d1f541bc791e/pyrevitlib/pyrevit/runtime/EventHandling.cs
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class ColorTab : IExternalCommand
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
                
                uiapp.ViewActivated += new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);

                uiapp.ApplicationClosing += new EventHandler<ApplicationClosingEventArgs>(Doc_DocumentClosing);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            
        }

        private void Doc_DocumentClosing(object sender, ApplicationClosingEventArgs e)
        {
            UIApplication uiapp = sender as UIApplication;

            uiapp.ViewActivated -= new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);

            TaskDialog.Show("R", "Unsuscribing");
        }

        public void Application_ViewActivated(object sender, ViewActivatedEventArgs args)
        {
 
            UIApplication uiapp = sender as UIApplication;

            var docTabGroup = GetDocumentTabGroup(uiapp);

            if (docTabGroup != null)
            {
                var docTabs = GetDocumentTabs(docTabGroup);

                foreach (TabItem tab in docTabs)
                {
                    //tab.BorderBrush = Brushes.Red;
                    //tab.BorderThickness = new System.Windows.Thickness(2);

                    SolidColorBrush mySolidColorBrush = new SolidColorBrush(Colors.Aqua);
                    mySolidColorBrush.Opacity = 0.25;

                    if (tab.ToolTip.ToString().Contains("Plan"))
                        tab.Background = mySolidColorBrush;

                }

            }
        }

        //public static Xceed.Wpf.AvalonDock.DockingManager GetDockingManager(UIApplication uiapp)
        //{
        //    //var wndRoot = (MainWindow)GetWindowRoot(uiapp);
        //    var wndRoot = (MainWindow)GetWindowRoot(uiapp);
        //    if (wndRoot != null)
        //    {
        //        return MainWindow.FindFirstChild<Xceed.Wpf.AvalonDock.DockingManager>(wndRoot);
        //    }
        //    return null;
        //}

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
            var wndRoot = GetWindowRoot(uiapp);

            if (wndRoot != null)
            {
                return MainWindow.FindFirstChild<LayoutDocumentPaneGroupControl>((MainWindow)wndRoot);
                
            }
            return null;
        }

        public static Visual GetWindowRoot(UIApplication uiapp)
        {
            IntPtr wndHndle = IntPtr.Zero;
            try
            {

                wndHndle = uiapp.MainWindowHandle;
            }
            catch { }

            if (wndHndle != IntPtr.Zero)
            {
                var wndSource = HwndSource.FromHwnd(wndHndle);
                return wndSource.RootVisual;
            }
            return null;
        }
    }
}
