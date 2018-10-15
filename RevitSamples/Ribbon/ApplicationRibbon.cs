#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
#endregion

namespace RevitSamples.Ribbon
{
    class ApplicationRibbon : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            try
            {
                string tabName = "Samples";

                try
                {
                    a.CreateRibbonTab(tabName);
                }
                catch
                {
                    //Might already exists
                }

                string ribbonPanelName = "Chapter2";

                RibbonPanel ribbonPanel = null;

                List<RibbonPanel> ribbonPanelList = new List<RibbonPanel>();
                ribbonPanelList = a.GetRibbonPanels(tabName);
                
                foreach(RibbonPanel r in ribbonPanelList)
                {
                    if (r.Name.ToUpper() == ribbonPanelName.ToUpper())
                    {
                        ribbonPanel = r;
                    }
                }

                //if (ribbonPanel is null)
                ribbonPanel = a.CreateRibbonPanel(tabName, ribbonPanelName);

                if (AddPushButton(ribbonPanel,"Ch2ButtonName","Hello","","","RevitSamples.dll","RevitSamples.CommandHelloWorld","Show a simple msg box Hello World") == false)
                {
                    MessageBox.Show("Failed to add PushButton", "caption",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

        ///<summary>
        ///Add a PushButton to the Ribbon
        ///</summary>
        private Boolean AddPushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllPath, string dllClass, string Tooltip)
        {
            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, dllPath, dllClass);
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

    }
}
