﻿#region Namespaces
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

                RibbonPanel beams = GetSetRibbonPanel(a, "SuperTab", "Structural Framings");

                RibbonPanel walls = GetSetRibbonPanel(a, "SuperTab", "Walls");

                RibbonPanel geometry = GetSetRibbonPanel(a, "SuperTab", "Geometry");

                RibbonPanel commandPanel = GetSetRibbonPanel(a, "SuperTab", "Command Line");

                RibbonPanel zeroState = GetSetRibbonPanel(a, "SuperTab", "Zero State");

                #region Doc Tools
                
                if (AddPushButton(toolsPanel, "btnSheetAddCurrentView", "Add View" + Environment.NewLine + "to Sheet", "", "pack://application:,,,/ReviTab;component/Resources/addView.png", "ReviTab.AddActiveViewToSheet", "Add the active view to a sheet") == false)
                {
                    MessageBox.Show("Failed to add button Add View to Sheet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnAddMultipleViews", "Add" + Environment.NewLine + "Multiple Views", "", "pack://application:,,,/ReviTab;component/Resources/addMultipleViews.png", "ReviTab.AddMultipleViewsToSheet", "Add multiple views to a sheet. Select the views in the project browser.") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnAddLegends", "Add Legend" + Environment.NewLine + "to Sheets", "", "pack://application:,,,/ReviTab;component/Resources/legend.png", "ReviTab.AddLegendToSheets", "Place a legend onto multiple sheets in the same place.") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (AddPushButton(toolsPanel, "btnOverrideDimensions", "Override \nDimension", "", "pack://application:,,,/ReviTab;component/Resources/dimensionOverride.png", "ReviTab.OverrideDimensions", "Override the text of a dimension") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnCopyLinkedElements", "Copy Linked \nElements", "", "pack://application:,,,/ReviTab;component/Resources/copyLinked.png", "ReviTab.CopyLinkedElements", "Copy elements from linked models") == false)
                {
                    MessageBox.Show("Failed to add button Copy Linked Elements", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                #endregion

                #region Structural Framing

                if (AddPushButton(beams, "btnPlaceVoidByFace", "Place Void" + Environment.NewLine + "By Face", "", "pack://application:,,,/ReviTab;component/Resources/addBeamOpening.png", "ReviTab.VoidByFace", "Place a void on a beam face") == false)
                {
                    MessageBox.Show("Failed to add button Void by face", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(beams, "btnPlaceVoidByLine", "Void By Line", "", "pack://application:,,,/ReviTab;component/Resources/line.png", "ReviTab.VoidByLine", "Place a void at line beam intersection. Contact: Ethan Gear.") == false)
                {
                    MessageBox.Show("Failed to add button Void By Line", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (AddPushButton(beams, "btnPlaceTags", "Place Tags", "", "pack://application:,,,/ReviTab;component/Resources/tag.png", "ReviTab.AddTagsApplyUndo", "Place a tag on multiple beams") == false)
                {
                    MessageBox.Show("Failed to add button Place Tags", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (AddPushButton(beams, "btnMoveBeamEnd", "Move Beam End", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.MoveBeamEnd", "Move a beam endpoint to match a selected beam closest point") == false)
                {
                    MessageBox.Show("Failed to add button Move Beam End", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(beams, "btnChangeBeamLocation", "Change Beam" + Environment.NewLine + "Location", "", "pack://application:,,,/ReviTab;component/Resources/moveBeam.png", "ReviTab.ChangeBeamLocation", "Move a beam to new location.") == false)
                {
                    MessageBox.Show("Failed to add button change beam location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(beams, "btnEditJoin", "Edit Beam" + Environment.NewLine + "End Join", "", "pack://application:,,,/ReviTab;component/Resources/joinEnd.png", "ReviTab.EditBeamJoin", "Allow/Disallow beam end join") == false)
                {
                    MessageBox.Show("Failed to add button Edit Beam", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                /*
                if (AddPushButton(beams, "btnSelectByParam", "Select By Parameter", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.SelectByParameter", "Select by parameter and operator") == false)
                {
                    MessageBox.Show("Failed to add button Select By Parameter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */

                #endregion

                #region Splitter

                if (AddPushButton(walls, "btnWallSplitter", "Split Wall" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/splitWalls.png", "ReviTab.WallSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Wall", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(walls, "btnColumnSplitter", "Split Column" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/wallSplit.png", "ReviTab.ColumnSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region Geometry

                if (AddPushButton(geometry, "btnSATtoDS", "Element to" + Environment.NewLine +"DirectShape", "", "pack://application:,,,/ReviTab;component/Resources/flatten.png", "ReviTab.SATtoDirectShape", "Convert an element into a DirectShape. Deletes the original element.") == false)
                {
                    MessageBox.Show("Failed to add button SAT to DS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(geometry, "btnProjectLines", "Project Lines"+ Environment.NewLine + "to Surface", "", "pack://application:,,,/ReviTab;component/Resources/projectLine.png", "ReviTab.ProjectLines", "Project some lines onto a surface.") == false)
                {
                    MessageBox.Show("Failed to add button project lines", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(geometry, "btnDrawAxis", "Draw Axis", "", "pack://application:,,,/ReviTab;component/Resources/axis.png", "ReviTab.DrawObjectAxis", "Draw local and global axis on a point on a surface.") == false)
                {
                    MessageBox.Show("Failed to add button draw axis", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                #endregion

                #region Zero State

                if (AddPushButton(zeroState, "btnPush", "Push to DB", "", "pack://application:,,,/ReviTab;component/Resources/arrowUp.png", "ReviTab.PushToDB", "Push date, user, rvtFileSize, elementsCount, typesCount, sheetsCount, viewsCount, viewportsCount, warningsCount to 127.0.0.1") == false)
                {
                    MessageBox.Show("Failed to add button Push to DB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddZeroStatePushButton(zeroState, "btnPrintBackground","Background" + Environment.NewLine + "Print", "", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "ReviTab.PrintInBackground", "Open a model in background and print the selcted drawings","ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Print in Background", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddZeroStatePushButton(zeroState, "btnClaritySetup", "Clarity Setup", "", "pack://application:,,,/ReviTab;component/Resources/claSetup.png", "ReviTab.ClaritySetup", "Open a model in background and create a 3d view for Clarity IFC export.", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Clarity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddZeroStatePushButton(zeroState, "btnPurgeFamilies", "Purge Families", "", "pack://application:,,,/ReviTab;component/Resources/wiping.png", "ReviTab.PurgeFamily", "Purge families and leave only a type called Default", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Purge Families", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                /*
                if (AddZeroStatePushButton(zeroState, "btnDiasbleWarning", "Open No Warnings", "", "pack://application:,,,/ReviTab;component/Resources/addMultiViews.png", "ReviTab.SuppressWarnings", "Suppress warnings when opening files", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Purge Families", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */
                if (AddZeroStatePushButton(zeroState, "btnHowl", "Howl", "", "pack://application:,,,/ReviTab;component/Resources/ghowlicon.png", "ReviTab.Howl", "Howl", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Purge Families", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddZeroStatePushButton(zeroState, "btnInfo", "Info", "", "pack://application:,,,/ReviTab;component/Resources/info.png", "ReviTab.VersionInfo", "Display Version Info Task Dialog.", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Version Info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                #endregion

                if (AddTextBox(commandPanel, "btnCommandLine", "*Structural Framing+Length>10000 \n *Walls+Mark!aa \n sheets: all \n sheets: A101 A103 A201\n tblocks: all") == false)
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

            if (message.StartsWith("/"))
                caseSwitch = "selectAll";

            if (message.StartsWith("sheets"))
                caseSwitch = "selectSheets";

            if (message.StartsWith("tblocks"))
                caseSwitch = "selectTBlocks";

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
                    Helpers.SelectAllElementsInView(uiDoc, message);
                    break;
                case "selectAll":
                    Helpers.SelectAllElements(uiDoc, message);
                    break;
                case "createViewSet":
                    Helpers.CreateViewset(doc, message);
                    break;
                case "delete":
                    break;
                case "selectSheets":
                    Helpers.HighlightSelectSheets(uiDoc, message);
                    break;
                case "selectTBlocks":
                    Helpers.HighlightSelectTitleBlocks(uiDoc, message);
                    break;
            }

            
        }

    }
}
