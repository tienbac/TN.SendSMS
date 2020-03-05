using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Entities
{
    class IncidentsV80R2E23Historic
    {
        public int Id { get; set; }
        public string IncType { get; set; }
        public DateTime StartInc { get; set; }
        public int CameraId { get; set; }

        public IncidentsV80R2E23Historic(int id, string incType, DateTime startInc, int cameraId)
        {
            Id = id;
            IncType = incType;
            StartInc = startInc;
            CameraId = cameraId;
        }

        public IncidentsV80R2E23Historic()
        {

        }
    }
}
