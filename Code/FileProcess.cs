using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Entities;

namespace TN.SendSMS.Code
{
    class FileProcess
    {
        //private static string lastIdFilePath = "..\\..\\LastData\\LastIdEmail.txt";
        public void AddLastId(int id, string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            string appendText = $"{id}{Environment.NewLine}";
            File.AppendAllText(path, appendText, Encoding.UTF8);
        }

        public bool CheckLastId(int id, string path)
        {
            var list = File.ReadAllLines(path).ToList();
            if (!list.Contains($"{id}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReSend( string phone, string message, TNGSMsms tngsmsms)
        {
            if (tngsmsms.IsConnected)
            {
                var response = tngsmsms.Send(phone, message);
                Console.WriteLine($"{response.Status} - {response.Message}");
                if (response.Status != 200)
                {
                    ReSend(phone, message, tngsmsms);
                }
                tngsmsms.Disconnect();
            }
            else
            {
                tngsmsms.Connect();
                ReSend(phone, message, tngsmsms);
            }
        }
    }
}
