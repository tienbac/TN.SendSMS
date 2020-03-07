using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.SendSMS.Code;
using TN.SendSMS.DataHelper;
using TN.SendSMS.Services;
using Topshelf;

namespace TN.SendSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TopshelfExitCode exitCode = HostFactory.Run(x =>
                {
                    x.Service<SyncService>(s =>
                    {
                        s.ConstructUsing(name => new SyncService());
                        s.WhenStarted(cb => cb.Start());
                        s.WhenStopped(cb => cb.Stop());
                        s.WhenShutdown(cb => cb.Stop());
                    });
                    x.RunAsLocalSystem();

                    x.SetServiceName(AppSettings.ServiceName);
                    x.SetDisplayName(AppSettings.ServiceDisplayName);
                    x.SetDescription(AppSettings.ServiceDescription);

                });
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("Program_Main", ex.ToString());
            }

            //TNGSMsms tngs = new TNGSMsms();
            //tngs.Connect();
            //if (tngs.IsConnected)
            //{
            //    tngs.Read();
            //    var response = tngs.Send("+84382824552", "Camera:30, SuCo: Tai Nan, ThoiGian: 2/27/2020 3:12:11 PM");
            //    Console.WriteLine($"{response.Status} -- {response.Message}");
            //    if (response.Status != 200)
            //    {
            //        var response2 = tngs.Send("+84975891464", "Camera:30, SuCo: Tai Nạn, ThoiGian: 2/27/2020 3:12:11 PM");
            //        Console.WriteLine($"{response2.Status} -- {response2.Message}");
            //    }
            //    //NewTNGSMsms.SendSMS("+84975891464", "Camera:30, SuCo: Tai Nạn, ThoiGian: 2/27/2020 3:12:11 PM");
            //    //SendSms("+84975891464", "Camera:30, SuCo: Tai Nạn, ThoiGian: 2/27/2020 3:12:11 PM");
            //}
            //Console.Read();


            //TNGSMsms tngsMsms = new TNGSMsms();

            ////var destinationNumber = args[0];
            //var destinationNumber = "+84986906656";
            ////var message = args[1];
            //var message = "Camera:30, SuCo: Tai Nan, ThoiGian: 2/27/2020 3:12:11 PM";

            //Console.WriteLine("About to send message to {0}",
            //    destinationNumber);

            //_modemConnection = new SerialPort(SERIAL_PORT_NAME)
            ////var com = tngsMsms.Search();
            ////_modemConnection = new SerialPort(com.Name)
            //{
            //    // 19200 baud, most modems will accept everything
            //    // from 9600 up to 115200
            //    BaudRate = 19200,
            //    // 99% of the time the port connection will be
            //    //8 Data bits
            //    DataBits = 8,
            //    // NO partiy
            //    Parity = Parity.None,
            //    // and 1 stop bit. Check your modem manual if
            //    // this doesn't work
            //    StopBits = StopBits.One,
            //    Handshake = Handshake.RequestToSend,
            //    NewLine = Environment.NewLine
            //};

            ////for (int i = 0; i < 5; i++)
            ////{
            ////    _modemConnection.Open();

            ////    SendSms(destinationNumber, message);

            ////    _modemConnection.Close();

            ////    Thread.Sleep(5000);
            ////}

            //_modemConnection.Open();

            //SendSms(destinationNumber, message);

            //_modemConnection.Close();

            //NewTNGSMsms.SendSMS(message, destinationNumber);

            Console.Read();
        }
    }
}
