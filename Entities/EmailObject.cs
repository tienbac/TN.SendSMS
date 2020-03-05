using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Entities
{
    class EmailObject
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string EmailHost { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string SendTo { get; set; }
        public string Message { get; set; }

        public string SMSSendTo { get; set; }
        public string SMSTemplate { get; set; }

        public EmailObject(string host, int port, string emailHost, string password, string displayName, string title, string sendTo, string message, string smsSendTo, string smsTemplate)
        {
            Host = host;
            Port = port;
            EmailHost = emailHost;
            Password = password;
            DisplayName = displayName;
            Title = title;
            SendTo = sendTo;
            Message = message;
            SMSSendTo = smsSendTo;
            SMSTemplate = smsTemplate;
        }

        public EmailObject()
        {

        }

        public EmailObject(string host, int port, string emailHost, string password, string displayName, string title, string sendTo, string message)
        {
            Host = host;
            Port = port;
            EmailHost = emailHost;
            Password = password;
            DisplayName = displayName;
            Title = title;
            SendTo = sendTo;
            Message = message;
        }

        public EmailObject(string host, int port, string emailHost, string password, string displayName, string title, string message)
        {
            Host = host;
            Port = port;
            EmailHost = emailHost;
            Password = password;
            DisplayName = displayName;
            Title = title;
            Message = message;
        }
    }
}
