using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Code
{
    class AppSettings
    {
        public static string ConnectionStringCitilog = ConfigurationManager.AppSettings["ConnectionStringCitilog"];
        public static string ConnectionStringTNAID = ConfigurationManager.AppSettings["ConnectionStringTNAID"];
        public static string ServiceName = ConfigurationManager.AppSettings["ServiceName"];
        public static string ServiceDisplayName = ConfigurationManager.AppSettings["ServiceDisplayName"];
        public static string ServiceDescription = ConfigurationManager.AppSettings["ServiceDescription"];
        public static string LastIdSMS = ConfigurationManager.AppSettings["LastIdSMS"];
        public static string LastIdEmail = ConfigurationManager.AppSettings["LastIdEmail"];
        public static string TimeGetSystemParameters = ConfigurationManager.AppSettings["TimeGetSystemParameters"];

        ////PHONE
        //public static string ListPhone = ConfigurationManager.AppSettings["ListPhone"];

        //// MESSAGE
        //public static string Message = ConfigurationManager.AppSettings["Message"];

        ////EMAIL
        //public static string ListEmail = ConfigurationManager.AppSettings["ListEmail"];
        //public static string Subject = ConfigurationManager.AppSettings["Subject"];
        //public static string STMPHost = ConfigurationManager.AppSettings["STMPHost"];
        //public static int STMPPort = Convert.ToInt32(ConfigurationManager.AppSettings["STMPPort"]);
        //public static string STMPEmailAddress = ConfigurationManager.AppSettings["STMPEmailAddress"];
        //public static string STMPDisplayName = ConfigurationManager.AppSettings["STMPDisplayName"];
        //public static string STMPUsername = ConfigurationManager.AppSettings["STMPUsername"];
        //public static string STMPPassword = ConfigurationManager.AppSettings["STMPPassword"];
        //public static string STMPDomain = ConfigurationManager.AppSettings["STMPDomain"];
    }
}
