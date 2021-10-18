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
    public class ColorTab : IExternalCommand
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
            int tabCount = 0;
            tabProjectNames.Clear();

            //sender is null and causes error
            //UIApplication uiapp = sender as UIApplication;

            uiapp.ViewActivated -= Application_ViewActivated;

            //TaskDialog.Show("R", "Unsuscribing");
        }

        private int tabCount = 0;
        private List<string> tabProjectNames = new List<string>();
        private List<SolidColorBrush> tabProjectColors = new List<SolidColorBrush> { Brushes.Coral, Brushes.RoyalBlue, Brushes.DeepPink, Brushes.SeaGreen, Brushes.Yellow, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Red, Brushes.Violet };

        public void Application_ViewActivated(object sender, ViewActivatedEventArgs args)
        {
            //Debug.WriteLine("View Activated");

            UIApplication uiapp = sender as UIApplication;

            //Debug.WriteLine(uiapp.ActiveUIDocument.Document.PathName);

            var docTabGroup = GetDocumentTabGroup(uiapp);

            //Debug.WriteLine("got docTabGroup");

            if (docTabGroup != null)
            {
                var docTabs = GetDocumentTabs(docTabGroup);

                int currentTabCount = docTabs.ToHashSet().Count;

                if (tabCount != currentTabCount)
                {                    
                    foreach (TabItem tab in docTabs)
                    {
                        string currentProjectName = tab.ToolTip.ToString().Split(' ')[0];

                        if (!tabProjectNames.Contains(currentProjectName))  //THE CURRENT TAB BELONGS TO A NEW PROJECT
                        {
                            tabProjectNames.Add(currentProjectName);
                            tab.BorderBrush = tabProjectColors[tabProjectNames.IndexOf(currentProjectName)];
                        }
                        else   //THE CURRENT TAB BELONGS TO A PROJECT THAT IS ALREADY OPENED
                        {
                            tab.BorderBrush = tabProjectColors[tabProjectNames.IndexOf(currentProjectName)];
                        }

                        tab.BorderThickness = new System.Windows.Thickness(0, 3, 0, 0);

                        SolidColorBrush planBrush = new SolidColorBrush(Colors.PaleVioletRed);
                        planBrush.Opacity = 0.75;

                        SolidColorBrush sectBrush = new SolidColorBrush(Colors.PaleGoldenrod);
                        planBrush.Opacity = 0.75;

                        SolidColorBrush threeDBrush = new SolidColorBrush(Colors.PaleTurquoise);
                        planBrush.Opacity = 0.75;

                        SolidColorBrush sheetBrush = new SolidColorBrush(Colors.PaleGreen);
                        planBrush.Opacity = 0.75;

                        if (tab.ToolTip.ToString().Contains("Plan:"))
                            tab.Background = planBrush;
                        else if (tab.ToolTip.ToString().Contains("Section:"))
                            tab.Background = sectBrush;
                        else if (tab.ToolTip.ToString().Contains("3D View:"))
                            tab.Background = threeDBrush;
                        else if (tab.ToolTip.ToString().Contains("Sheet:"))
                            tab.Background = sheetBrush;
                    }

                    tabCount = docTabs.ToHashSet().Count;
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
