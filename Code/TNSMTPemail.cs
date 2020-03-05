using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.DataHelper;
using TN.SendSMS.Entities;

namespace TN.SendSMS.Code
{
    class TNSMTPemail
    {
        FileProcess fp = new FileProcess();
        public ResponseSend SendMail(EmailObject email, string sendTo, string emailTemplate)
        {
            ResponseSend responseSend;
            SmtpClient client = new SmtpClient(email.Host, email.Port);
            client.EnableSsl = true;
            MailAddress from = new MailAddress(email.EmailHost, email.DisplayName);
            MailAddress to = new MailAddress(sendTo);
            MailMessage message = new MailMessage(from, to);
            message.Body = emailTemplate;
            message.Subject = email.Title;
            message.IsBodyHtml = true;
            NetworkCredential myCredential = new NetworkCredential(email.EmailHost, email.Password, "");
            client.Credentials = myCredential;
            try
            {
                client.Send(message);
                responseSend = new ResponseSend(200, "Send email success!");
                Utilities.WriteOperationLog($"[SEND EMAIL ({responseSend.Status}) ] : ", $"[SEND EMAIL TO {sendTo} SUCCESSFUL !] [ MESSAGE : {emailTemplate} ]");
                return responseSend;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Exception is:" + ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                responseSend = new ResponseSend(404, "Send email fail!");
                Utilities.WriteErrorLog($"[SEND EMAIL ({responseSend.Status})] : ", $"[SEND EMAIL TO {sendTo} FAIL. TRY AGAIN! ] [MESSAGE : {emailTemplate} ]");
                return responseSend;
            }
        }

        public void SendMails(object obj)
        {
            InputData inputData = (InputData) obj;
            var emailSendToList = inputData.EmailObject.SendTo.Split(';');
            foreach (var historic in inputData.Historics)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"ID : {historic.Id} -- IncType : {historic.IncType} -- StartInc : {historic.StartInc} -- CameraId : {historic.CameraId}");
                Console.ForegroundColor = ConsoleColor.White;
                if (fp.CheckLastId(historic.Id, AppSettings.LastIdEmail))
                {
                    foreach (var email in emailSendToList)
                    {
                        string message = inputData.EmailObject.Message.Replace("{IncType}", historic.IncType).Replace("{StartInc}", historic.StartInc.ToString()).Replace(" {CameraId}", historic.CameraId.ToString()).Replace("\\r\\n", "");
                        var response = SendMail(inputData.EmailObject, email, message);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{response.Status} - {response.Message}");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (response.Status == 200)
                        {
                            continue;
                        }
                    }
                    File.WriteAllText(AppSettings.LastIdEmail, String.Empty);
                    fp.AddLastId(historic.Id, AppSettings.LastIdEmail);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ID {historic.Id} HAS EXIST! Email was send!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

            }
        }
    }
    
}
