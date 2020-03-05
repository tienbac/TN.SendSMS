using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Code;
using TN.SendSMS.DataHelper;

namespace TN.SendSMS
{
    class SyncService
    {
        public static int s_TimeOutCheckAutomaticReviewAlarm = 0;
        public void Start()
        {
            try
            {
                Utilities.WriteDebugLog("Start", "App Started!");
                JobScheduler.Start();
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("ProgramInit_Start", ex.ToString());
            }
        }
        public void Stop()
        {
            try
            {
                JobScheduler.Stop();
                Utilities.WriteDebugLog("ProgramInit_Stop", "Try to stop service...");
                ConnectionHelper.CloseConnection(CitilogModel.connectionCitiLog);
                ConnectionHelper.CloseConnection(TNAIDModel.connectionTNAID);
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("ProgramInit_Stop", ex.ToString());
            }
        }
    }
}
