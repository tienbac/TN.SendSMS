using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Code;
using TN.SendSMS.DataHelper;
using Topshelf;

namespace TN.SendSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TopshelfExitCode exitCode = HostFactory.Run(x =>
                {
                    x.Service<SyncService>(s =>
                    {
                        s.ConstructUsing(name => new SyncService());
                        s.WhenStarted(cb => cb.Start());
                        s.WhenStopped(cb => cb.Stop());
                        s.WhenShutdown(cb => cb.Stop());
                    });
                    x.RunAsLocalSystem();

                    x.SetServiceName(AppSettings.ServiceName);
                    x.SetDisplayName(AppSettings.ServiceDisplayName);
                    x.SetDescription(AppSettings.ServiceDescription);

                });
            }
            catch (Exception ex)
            {
                Utilities.WriteErrorLog("Program_Main", ex.ToString());
            }
        }
    }
}
