using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.SendSMS.Code;

namespace TN.SendSMS.Services
{
    class NewTNGSMsms
    {
        private SerialPort gsmPort = null;
        public bool IsConnected { get; set; } = false;

        TNGSMsms tngsMsms = new TNGSMsms();
        public bool Connect()
        {
            if (gsmPort == null || !IsConnected || !gsmPort.IsOpen)
            {
                var com = tngsMsms.Search();
                if (com!=null)
                {
                    try
                    {
                        gsmPort.PortName = com.Name;
                        gsmPort.BaudRate = 19200;
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
        public void SendSms(string destination, string text)
        {
            // Turn off echo, we don't need it for this
            gsmPort.WriteLine("ATE0");
            var response = gsmPort.ReadExisting();

            // Set text mode
            gsmPort.WriteLine("AT+CMGF=1");
            response = gsmPort.ReadExisting();

            // Send the SMS
            gsmPort.WriteLine(String.Format
                ("AT+CMGS=\"{0}\"", destination));
            response = gsmPort.ReadExisting();

            gsmPort.Write(text);
            gsmPort.Write(new byte[] { 26 }, 0, 1);
            Thread.Sleep(5000);

            response = gsmPort.ReadExisting();

            if (response.Contains("ERROR"))
            {
                Console.WriteLine("SMS Failed to send");
            }
            else
            {
                Console.WriteLine("SMS Sent");
                Console.WriteLine("Response: {0}", response.Length);
            }
        }
    }

    class SMSCOMMS
    {
        private SerialPort SMSPort;
        private Thread SMSThread;
        private Thread ReadThread;
        public static bool _Continue = false;
        public static bool _ContSMS = false;
        private bool _Wait = false;
        public static bool _ReadPort = false;
        public delegate void SendingEventHandler(bool Done);
        public event SendingEventHandler Sending;
        public delegate void DataReceivedEventHandler(string Message);
        public event DataReceivedEventHandler DataReceived;

        public SMSCOMMS(string COMMPORT)
        {
            SMSPort = new System.IO.Ports.SerialPort();
            SMSPort.PortName = COMMPORT;
            SMSPort.BaudRate = 9600;
            SMSPort.Parity = Parity.None;
            SMSPort.DataBits = 8;
            SMSPort.StopBits = StopBits.One;
            SMSPort.Handshake = Handshake.RequestToSend;
            SMSPort.DtrEnable = true;
            SMSPort.RtsEnable = true;
            SMSPort.NewLine = System.Environment.NewLine;
            ReadThread = new Thread(
                new System.Threading.ThreadStart(ReadPort));
        }

        public bool SendSMS(string CellNumber, string SMSMessage)
        {
            string MyMessage = null;
            //Check if Message Length <= 160
            if (SMSMessage.Length <= 160)
                MyMessage = SMSMessage;
            else
                MyMessage = SMSMessage.Substring(0, 160);
            if (SMSPort.IsOpen)
            {
                SMSPort.WriteLine("AT+CMGS=" + CellNumber + "r");
                _ContSMS = false;
                SMSPort.WriteLine(
                MyMessage + System.Environment.NewLine + (char)(26));
                _Continue = false;
                if (Sending != null)
                    Sending(true);
            }
            return false;
        }

        private void ReadPort()
        {
            string SerialIn = null;
            byte[] RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
            string SMSMessage = null;
            int Strpos = 0;
            string TmpStr = null;
            while (SMSPort.IsOpen == true)
            {
                if ((SMSPort.BytesToRead != 0) & (SMSPort.IsOpen == true))
                {
                    while (SMSPort.BytesToRead != 0)
                    {
                        SMSPort.Read(RXBuffer, 0, SMSPort.ReadBufferSize);
                        SerialIn =
                            SerialIn + System.Text.Encoding.ASCII.GetString(
                            RXBuffer);
                        if (SerialIn.Contains(">") == true)
                        {
                            _ContSMS = true;
                        }
                        if (SerialIn.Contains("+CMGS:") == true)
                        {
                            _Continue = true;
                            if (Sending != null)
                                Sending(true);
                            _Wait = false;
                            SerialIn = string.Empty;
                            RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                        }
                    }
                    if (DataReceived != null)
                        DataReceived(SerialIn);
                    SerialIn = string.Empty;
                    RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                }
            }
        }


        public void Open()
        {
            if (SMSPort.IsOpen == false)
            {
                SMSPort.Open();
                ReadThread.Start();
            }
        }

        public void Close()
        {
            if (SMSPort.IsOpen == true)
            {
                SMSPort.Close();
            }
        }
    }
}
