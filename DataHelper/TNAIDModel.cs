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
    class TNAIDModel
    {
        public static SqlConnection connectionTNAID = null;
        public EmailObject GetSystemParameters()
        {
            //List<ParametersKeyValue> list = new List<ParametersKeyValue>();
            try
            {
                Dictionary<string, string> systemparameters = new Dictionary<string, string>();

                if (connectionTNAID == null)
                {
                    connectionTNAID = ConnectionHelper.GetConnection(AppSettings.ConnectionStringTNAID, connectionTNAID);
                }

                SqlCommand cmd = connectionTNAID.CreateCommand();
                cmd.CommandText = "SELECT * FROM SystemParameters";
                cmd.CommandType = CommandType.Text;
                //ParametersKeyValue parameters = null;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //parameters = new ParametersKeyValue(reader.GetInt32("Id"), reader.GetString("Key"), reader.GetString("Value"));
                        //list.Add(parameters);
                        systemparameters.Add(reader.GetString("Key"), reader.GetString("Value"));
                    }
                }
                reader.Close();
                EmailObject emailObject = null;
                if (systemparameters != null)
                {
                    emailObject = new EmailObject();
                    emailObject.Host = systemparameters["Host"];
                    emailObject.Port = Int32.Parse(systemparameters["Port"]);
                    emailObject.EmailHost = systemparameters["Email"];
                    emailObject.Password = systemparameters["Password"];
                    emailObject.DisplayName = systemparameters["From"];
                    emailObject.Title = systemparameters["EmailTitle"];
                    emailObject.SendTo = systemparameters["EmailSendTo"];
                    emailObject.Message = systemparameters["EmailTemplate"];
                    emailObject.SMSSendTo = systemparameters["SMSSendTo"];
                    emailObject.SMSTemplate = systemparameters["SMSTemplate"];
                }

                return emailObject;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
