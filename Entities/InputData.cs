using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Code;

namespace TN.SendSMS.Entities
{
    class InputData
    {
        public EmailObject EmailObject { get; set; }
        public List<IncidentsV80R2E23Historic> Historics { get; set; }

    }
}
