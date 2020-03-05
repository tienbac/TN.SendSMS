using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using TN.SendSMS.DataHelper;

namespace TN.SendSMS.Code
{
    class JobAutoGetSystemParameters
    {
        private static IScheduler scheduler;
        public static void Start()
        {
            try
            {
                scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                //Job capture frames
                IJobDetail job = JobBuilder.Create<Job2>().Build();
                ITrigger restartTrigger = TriggerBuilder.Create()
                    //.WithDailyTimeIntervalSchedule
                    //      (s =>
                    //         s.WithIntervalInHours(24)
                    //        .OnEveryDay()
                    //        .StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(0, 0, 0))
                    //      )
                    .WithDailyTimeIntervalSchedule
                          (s =>
                          //s.WithIntervalInMinutes(2)
                          //s.WithIntervalInHours(1)
                          s.WithIntervalInSeconds(Int32.Parse(AppSettings.TimeGetSystemParameters))
                          )
                    .Build();
                scheduler.ScheduleJob(job, restartTrigger);

            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("restartJob_Start", ex.ToString());
            }
        }
    }

    class Job2 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            TNAIDModel tnaidModel = new TNAIDModel();
            JobScheduler.EmailObject = tnaidModel.GetSystemParameters();
            Console.WriteLine($"GET NEW SYSTEM PARAMETERS !");
        }
    }
}
