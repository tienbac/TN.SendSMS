using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.SendSMS.Entities;

namespace TN.SendSMS.Code
{
    class TNGSMsms
    {
        private SerialPort gsmPort = null;
        private bool IsDeviceFound { get; set; } = false;
        public bool IsConnected { get; set; } = false;
        public TNGSMsms()
        {
            gsmPort = new SerialPort();
        }

        public TNGSMcom[] List()
        {
            List<TNGSMcom> gsmCom = new List<TNGSMcom>();
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.EnablePrivileges = true;
            string connString = $@"\\{Environment.MachineName}\root\cimv2";
            ManagementScope scope = new ManagementScope(connString, options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_POTSModem");
            ManagementObjectSearcher search = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection collection = search.Get();

            foreach (ManagementObject obj in collection)
            {
                string portName = obj["AttachedTo"].ToString();
                string portDescription = obj["Description"].ToString();

                if (portName != "")
                {
                    TNGSMcom com = new TNGSMcom();
                    com.Name = portName;
                    com.Description = portDescription;
                    gsmCom.Add(com);
                }
            }


            return gsmCom.ToArray();
        }
        public TNGSMcom Search()
        {

            //foreach (GSMcom com in List())
            //{
            //    Console.WriteLine(com.Description + " " + com.Name);
            //}
            // or better way using enumerator

            IEnumerator enumerator = List().GetEnumerator();
            TNGSMcom com = enumerator.MoveNext() ? (TNGSMcom)enumerator.Current : null;

            if (com == null)
            {
                IsDeviceFound = false;
                Console.WriteLine("No GSM device found!");
                //Disconnect();
            }
            else
            {
                IsDeviceFound = true;
                Console.WriteLine(com.ToString());
                //Connect();
            }

            return com;
        }

        public bool Connect()
        {
            if (gsmPort == null || !IsConnected || !gsmPort.IsOpen)
            {
                TNGSMcom com = Search();
                if (com != null)
                {
                    try
                    {
                        gsmPort.PortName = com.Name;
                        gsmPort.BaudRate = 9600;
                        gsmPort.Parity = Parity.None;
                        gsmPort.DataBits = 8;
                        gsmPort.StopBits = StopBits.One;
                        gsmPort.Handshake = Handshake.RequestToSend;
                        gsmPort.DtrEnable = true;    // Data-terminal-ready
                        gsmPort.RtsEnable = true;    // Request-to-send
                        gsmPort.NewLine = Environment.NewLine;
                        gsmPort.Open();
                        IsConnected = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        IsConnected = false;
                    }
                }
                else
                {
                    IsConnected = false;
                }

            }

            return IsConnected;
        }

        public void Disconnect()
        {
            if (gsmPort != null || IsConnected || gsmPort.IsOpen)
            {
                gsmPort.Close();
                gsmPort.Dispose();
                IsConnected = false;
            }
        }

        public void Read()
        {
            Console.WriteLine("Reading..");

            gsmPort.WriteLine("AT+CMGF=1"); // Set mode to Text(1) or PDU(0)
            Thread.Sleep(1000); // Give a second to write
            gsmPort.WriteLine("AT+CPMS=\"SM\""); // Set storage to SIM(SM)
            Thread.Sleep(1000);
            gsmPort.WriteLine("AT+CMGL=\"ALL\""); // What category to read ALL, REC READ, or REC UNREAD
            Thread.Sleep(1000);
            gsmPort.Write("\r");
            Thread.Sleep(1000);

            string response = gsmPort.ReadExisting();

            if (response.EndsWith("\r\nOK\r\n"))
            {
                Console.WriteLine(response);
                // add more code here to manipulate reponse string.
            }
            else
            {
                // add more code here to handle error.
                Console.WriteLine(response);
            }

        }

        public ResponseSend Send(string toAdress, string message)
        {
            Console.WriteLine("Sending..");
            ResponseSend responseSendSms;
            gsmPort.WriteLine("AT+CMGF=1");
            Thread.Sleep(1000);
            gsmPort.WriteLine($"AT+CMGS=\"{toAdress}\"");
            Thread.Sleep(1000);
            gsmPort.WriteLine(message + char.ConvertFromUtf32(26));
            Thread.Sleep(5000);

            string response = gsmPort.ReadExisting();


            if (response.EndsWith("\r\nOK\r\n") && response.Contains("+CMGS:"))
            {
                Console.WriteLine(response);
                responseSendSms = new ResponseSend(200, response);
                Utilities.WriteOperationLog($"[SEND SMS ({responseSendSms.Status}) ] : ", $"[SEND SMS TO {toAdress} SUCCESSFUL ! ] [ MESSAGE : {message} ]");
            }
            else
            {
                Console.WriteLine(response);
                responseSendSms = new ResponseSend(404, response);
                Utilities.WriteErrorLog($"[SEND SMS ({responseSendSms.Status}) ] : ", $"[SEND SMS TO {toAdress} FAIL. TRY AGAIN ! ] [ MESSAGE : {message} ]");
            }

            return responseSendSms;
        }
    }
}
