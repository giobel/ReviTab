using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;


namespace gHowl
{
    public class UdpSenderComponent
    {

        //The main disposable item
        public UdpClient _udpClient;

        private IPEndPoint _receiverIp;
        private byte[] _message;

        private string _serviceMessage = null;
        private Formatter _formatter = Formatter.Instance;
        private bool patternChanged = false;

        public string ipAddress { get; set; }

        int _prt = 0;


        public void SendMessage(List<string> _sMessage)
        {


            string addy = IPAddress.Loopback.ToString();

            int port = 8051;

            //GH_Integer pattern = patterns.get_FirstItem(false);
            _prt = port;
            if (port < 0)
                return;

            //string address = "172.22.133.49";
            string address = ipAddress;
            IPAddress parsedAdd;

            if (address == null)
            {
                parsedAdd = IPAddress.Loopback;
            }
            else if (!IPAddress.TryParse(address, out parsedAdd))
            {
                Console.WriteLine("This address is misspelled.");
                return;
            }

             //_pattern = SendPattern.Text;
            

            if (_receiverIp == null || port != _receiverIp.Port || parsedAdd != _receiverIp.Address || patternChanged)
            {
                _receiverIp = new IPEndPoint(parsedAdd, port); //destination
                patternChanged = false;
            }

            List<string> sMessage = _sMessage;

            _message = _formatter.AsciiBytes(sMessage);

            

            if (SendMessages(_message, _message.Length, _receiverIp))
            {
                _serviceMessage = string.Format("Sending successful - UDP does not guarantee any arrival");
            }

            
        }

        private bool SendMessages(byte[] _message, int length, IPEndPoint endpoint)
        {
            return length == _udpClient.Send(_message, length, endpoint);
        }

        private void StartSending()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
                _udpClient = new UdpClient();
        }

        private void StopSending()
        {
            _udpClient.Close();
            _udpClient = null;
        }


    }
}
