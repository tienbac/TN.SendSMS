using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Code;
using TN.SendSMS.Entities;

namespace TN.SendSMS.DataHelper
{
    class CitilogModel
    {
        public static SqlConnection connectionCitiLog = null;
        public List<string> ListIncTypes = new List<string>(){ "StopF", "StopC", "SlowDown", "Debris" };

        public List<IncidentsV80R2E23Historic> GetAllIncidentsV80R2E23Historics()
        {
            try
            {
                List<IncidentsV80R2E23Historic> list = new List<IncidentsV80R2E23Historic>();

                if (connectionCitiLog == null)
                {
                    connectionCitiLog =
                        ConnectionHelper.GetConnection(AppSettings.ConnectionStringCitilog, connectionCitiLog);
                }
                SqlCommand cmd = connectionCitiLog.CreateCommand();
                cmd.CommandText = "SELECT Id, IncType, StartInc, CameraId FROM IncidentsV80R2E23Historic WHERE Id = (SELECT MAX(ID)  FROM IncidentsV80R2E23Historic)";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                IncidentsV80R2E23Historic incidents = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (ListIncTypes.Contains(reader.GetString("IncType")))
                        {
                            incidents = new IncidentsV80R2E23Historic(reader.GetInt32("Id"), reader.GetString("IncType"), reader.GetDateTime(reader.GetOrdinal("StartInc")), reader.GetInt32("CameraId"));
                            list.Add(incidents);
                        }
                    }
                }
                reader.Close();
                return list;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
