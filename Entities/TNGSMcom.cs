using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Code
{
    class TNGSMcom
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public TNGSMcom()
        {

        }

        override
            public string ToString()
        {
            return $"{Description} {Name}";
        }
    }
}
