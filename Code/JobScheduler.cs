﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using TN.SendSMS.DataHelper;
using TN.SendSMS.Entities;

namespace TN.SendSMS.Code
{
    class JobScheduler
    {
        private static IScheduler scheduler;
        public static EmailObject EmailObject;

        public static void Start()
        {
            try
            {
                scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                //Job capture frames
                IJobDetail job = JobBuilder.Create<Job>().Build();
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
                          s.WithIntervalInSeconds(Int32.Parse(AppSettings.TimeSchedule))
                          )
                    .Build();
                scheduler.ScheduleJob(job, restartTrigger);
                JobAutoGetSystemParameters.Start();
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("restartJob_Start", ex.ToString());
            }
        }
        public static void Stop()
        {
            try
            {
                if (scheduler != null)
                {
                    ConnectionHelper.CloseConnection(CitilogModel.connectionCitiLog);
                    ConnectionHelper.CloseConnection(TNAIDModel.connectionTNAID);
                    scheduler.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("restartJob_Stop", ex.ToString());
            }
        }
    }

    partial class Job : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                TNSMTPemail tnsmtpemail = new TNSMTPemail();
                SendAll sendAll = new SendAll();
                CitilogModel citilogModel = new CitilogModel();
                TNAIDModel tnaidModel = new TNAIDModel();

                JobScheduler.EmailObject = tnaidModel.GetSystemParameters();

                InputData inputData = new InputData();
                inputData.Historics = citilogModel.GetAllIncidentsV80R2E23Historics();
                inputData.EmailObject = JobScheduler.EmailObject;

                Thread threadSendEmail = new Thread(tnsmtpemail.SendMails);
                threadSendEmail.Start(inputData);

                Thread threadSendSMS = new Thread(sendAll.SendSMSs);
                threadSendSMS.Start(inputData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Utilities.WriteErrorLog("restartJob_Execute", e.ToString());
            }
        }
    }

    class SendAll
    {
        TNGSMsms tngsmsms = new TNGSMsms();
        FileProcess fp = new FileProcess();
        public void SendSMSs(object obj)
        {
            InputData inputData = (InputData)obj;
            tngsmsms.Connect();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| TN.GSM.SMS connect : {tngsmsms.IsConnected}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("+----------------------------------------------------------------------------------------+");
            if (tngsmsms.IsConnected)
            {
                tngsmsms.Read();
                var phones = inputData.EmailObject.SMSSendTo.Split(';');
                List<int> checkSMS = new List<int>();
                foreach (var historic in inputData.Historics)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"ID : {historic.Id} -- IncType : {historic.IncType} -- StartInc : {historic.StartInc} -- CameraId : {historic.CameraId}");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (fp.CheckLastId(historic.Id, AppSettings.LastIdSMS))
                    {
                        foreach (var phone in phones)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(phone);
                            Console.ForegroundColor = ConsoleColor.White;
                            string message = inputData.EmailObject.SMSTemplate.Replace("{IncType}", historic.IncType).Replace("{StartInc}", historic.StartInc.ToString()).Replace(" {CameraId}", historic.CameraId.ToString());
                            var response = tngsmsms.Send(phone, message);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{response.Status} - {response.Message}");
                            Console.ForegroundColor = ConsoleColor.White;
                            checkSMS.Add(response.Status);
                            if (response.Status == 200)
                            {
                                continue;
                            }
                            //else
                            //{
                            //    fp.ReSend(phone, message, tngsmsms);
                            //}
                            //if (response.Status != 200)
                            //{
                            //    fp.ReSend(phone, message, tngsmsms);
                            //}
                        }
                        File.WriteAllText(AppSettings.LastIdSMS, String.Empty);
                        fp.AddLastId(historic.Id, AppSettings.LastIdSMS);
                        
                        //if (!checkSMS.Contains(404))
                        //{
                        //    File.WriteAllText(AppSettings.LastIdSMS, String.Empty);
                        //    fp.AddLastId(historic.Id, AppSettings.LastIdSMS);
                        //}
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ID {historic.Id} HAS EXIST! SMS was send!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                }
                tngsmsms.Disconnect();
            }
        }
    }
}
