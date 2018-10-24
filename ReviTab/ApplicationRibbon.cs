#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
#endregion

namespace ReviTab
{
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(
          UIApplication a,
          CategorySet b)
        {
            return true;
        }
    }

    [Regeneration(RegenerationOption.Manual)]
    class ApplicationRibbon : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication a)
        {

            try
            {
                               
                AddRibbonPanel(a,"SuperTab","Tools");

                RibbonPanel toolsPanel = GetSetRibbonPanel(a, "SuperTab", "Tools");

                RibbonPanel commandPanel = GetSetRibbonPanel(a, "SuperTab", "Command Line");

                RibbonPanel zeroState = GetSetRibbonPanel(a, "SuperTab", "Zero State");

                if (AddPushButton(toolsPanel, "btnSheetAddCurrentView", "Add View" + Environment.NewLine + "to Sheet", "", "pack://application:,,,/ReviTab;component/Resources/addView.png", "ReviTab.AddActiveViewToSheet", "Add the active view to a sheet") == false)
                {
                    MessageBox.Show("Failed to add button Add View to Sheet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnCreateSections", "Create Multiple" + Environment.NewLine + "Sections", "", "pack://application:,,,/ReviTab;component/Resources/multipleSections.png", "ReviTab.CreateSections", "Create multiple sections from selected elements (must have location curve i.e. beams, walls, lines...)") == false)
                {
                    MessageBox.Show("Failed to add button Create Multiple Sections", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnSelectText", "Select All" + Environment.NewLine + "Text", "", "pack://application:,,,/ReviTab;component/Resources/selectText.png", "ReviTab.SelectAllText", "Select all text notes in the project. Useful if you want to run the check the spelling.") == false)
                {
                    MessageBox.Show("Failed to add button Select all text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnCreateViewset", "Create" + Environment.NewLine + "Viewset", "", "pack://application:,,,/ReviTab;component/Resources/createViewSet.png", "ReviTab.CreateViewSet", "Create a Viewset from a list of Sheet Numbers ") == false)
                {
                    MessageBox.Show("Failed to add button Select all text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnSwapGrid", "Swap Grid" + Environment.NewLine + "Head", "", "pack://application:,,,/ReviTab;component/Resources/swapGrids.png", "ReviTab.SwapGridBubbles", "Swap the head of the selected grids") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnAddMultipleViews", "Add" + Environment.NewLine + "Multiple Views", "", "pack://application:,,,/ReviTab;component/Resources/createViewSet.png", "ReviTab.AddMultipleViewsToSheet", "Add multiple views to a sheet. Select the views in the project browser.") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

                if (AddZeroStatePushButton(zeroState, "btnPrintBackground","Background" + Environment.NewLine + "Print", "", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "ReviTab.PrintInBackground", "Open a model in background and print the selcted drawings","ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Print in Background", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                
                if (AddTextBox(commandPanel, "btnCommandLine","Type some text and hit Enter") == false)
                {
                    MessageBox.Show("Failed to add Text Box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // Return Success
                return Result.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }


        }

        public Result OnShutdown(UIControlledApplication a)
        {
            try
            {
                // Begin Code Here
                // Return Success
                return Result.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }
        }

        /// <summary>
        /// Create a Ribbon Tab and a Ribbon Panel
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="ribbonName"></param>
        static void AddRibbonPanel(UIControlledApplication application, string tabName, string ribbonName)

        {

            // Create a custom ribbon tab

            application.CreateRibbonTab(tabName);



            // Add a new ribbon panel

            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, ribbonName);

            /*

            // Get dll assembly path

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;



            // create push button for CurveTotalLength

            PushButtonData b1Data = new PushButtonData(

                "cmdCurveTotalLength",

                "Total" + System.Environment.NewLine + "  Length  ",

                thisAssemblyPath,

                "ReviTab.CommandHelloWorld");

            //typeof(CommandHelloWorld).FullName

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;

            pb1.ToolTip = "Select Multiple Lines to Obtain Total Length";

            //BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/GrimshawRibbon;component/Resources/totalLength.png"));

            //pb1.LargeImage = pb1Image;*/

        }

        /// <summary>
        /// Selects or Creates a Ribbon Panel in a specified Ribbon Tab
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="panelName"></param>
        /// <returns></returns>
        private RibbonPanel GetSetRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            List<RibbonPanel> tabList = new List<RibbonPanel>();

            tabList = application.GetRibbonPanels(tabName);

            RibbonPanel tab = null;

            foreach (RibbonPanel r in tabList)
            {
                if (r.Name.ToUpper() == panelName.ToUpper())
                {
                    tab = r;
                }
            }

            if (tab is null)
                tab = application.CreateRibbonPanel(tabName, panelName);

            return tab;
        }
        
        ///<summary>
        ///Add a PushButton to the Ribbon Panel
        ///</summary>
        private Boolean AddPushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);
                
                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a push button visible in Revit Zero State mode
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="ButtonName"></param>
        /// <param name="ButtonText"></param>
        /// <param name="ImagePath16"></param>
        /// <param name="ImagePath32"></param>
        /// <param name="dllClass"></param>
        /// <param name="Tooltip"></param>
        /// <returns></returns>
        private Boolean AddZeroStatePushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip, string Availability)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = Availability;

                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a Text Box to the Ribbon Panel
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="textboxName"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        private Boolean AddTextBox(RibbonPanel Panel, string textboxName, string tooltip)
        {
            try
            {
                TextBoxData tbData = new TextBoxData(textboxName);
                Autodesk.Revit.UI.TextBox textBox = Panel.AddItem(tbData) as Autodesk.Revit.UI.TextBox;

                textBox.PromptText = "Write something and hit Enter";
                textBox.ShowImageAsButton = true;
                textBox.ToolTip = tooltip;

                textBox.EnterPressed += new EventHandler<TextBoxEnterPressedEventArgs>(MyTextBoxEnter);
                     
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Text Box Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MyTextBoxEnter(object sender, TextBoxEnterPressedEventArgs args)
        {
            UIDocument uiDoc = args.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Autodesk.Revit.UI.TextBox textBox = sender as Autodesk.Revit.UI.TextBox;

            string message = textBox.Value.ToString();

            string caseSwitch = "default";

            if (message.Contains("babbo"))
                caseSwitch = "christmas";

            if (message.Contains("leann says"))
                caseSwitch = "leann";

            if (message.Contains("+"))
                caseSwitch = "sum";

            if (message.StartsWith("*"))
                caseSwitch = "select";

            if (message.StartsWith("-"))
                caseSwitch = "delete";

            if (message.StartsWith("+viewset"))
                caseSwitch = "createViewSet";

            switch (caseSwitch)
            {
                case "default":
                    MessageBox.Show(textBox.Value.ToString(), "Command Line");
                    break;
                case "christmas":
                    Helpers.Christams();
                    break;
                case "leann":
                    Helpers.leannSays();
                    break;
                case "sum":
                    Helpers.AddTwoIntegers(message);
                    break;
                case "select":
                    Helpers.SelectAllTypes(uiDoc, message);
                    break;
                case "createViewSet":
                    Helpers.CreateViewset(doc, message);
                    break;
                case "delete":
                    break;
            }

            
        }

    }
}
