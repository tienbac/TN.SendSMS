using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Entities
{
    class ResponseSend
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public ResponseSend()
        {
            
        }
        public ResponseSend(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
