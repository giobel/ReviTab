#region Namespaces
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using forms = System.Windows.Forms;
#endregion

// credits: https://github.com/gHowl/gHowlComponents

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class Howl : IExternalCommand
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
            View activeView = doc.ActiveView;

            using (var form = new FormHowl("Enter a message"))
            {
                using (Transaction t = new Transaction(doc))
                {

                    string interrupt = "False";

                    while (interrupt == "False")
                    {
                        //use ShowDialog to show the form as a modal dialog box. 



                        form.ShowDialog();

                        //if the user hits cancel just drop out of macro
                        if (form.DialogResult == forms.DialogResult.Cancel)
                        {
                            return Result.Cancelled;
                        }

                        try
                        {   
                            gHowl.UdpSenderComponent gup = new gHowl.UdpSenderComponent();

                            gup.ipAddress = form.TextAddress;

                            if (NetworkInterface.GetIsNetworkAvailable())
                                gup._udpClient = new UdpClient();


                            List<string> rvtMessage = new List<string>();

                            rvtMessage.Add(form.TextString);

                            gup.SendMessage(rvtMessage);

                            gup._udpClient.Close();
                            gup._udpClient = null;

                        }
                        catch(Exception ex)
                        {
                            TaskDialog.Show("Error", ex.Message);
                        }

                       

                    }//close while

                }

            }

            return Result.Succeeded;
        }
    }
}
