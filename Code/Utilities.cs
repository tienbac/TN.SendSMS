using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.SendSMS.Code
{
    class Utilities
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TN.OrganizeImageFiles.Code");

        public static bool ActiveOperationLog = true;
        public static bool ActiveDebugLog = true;
        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;
        }

        public static void StartLog(bool activeOperationLog, bool activeDebugLog)
        {
            ActiveOperationLog = activeOperationLog;
            ActiveDebugLog = activeDebugLog;
        }

        public static void CloseLog()
        {
            foreach (log4net.Appender.IAppender app in log.Logger.Repository.GetAppenders())
            {
                app.Close();
            }
        }

        public static void WriteErrorLog(string logtype, string logcontent)
        {
            try
            {
                log.Error($"{logtype} \t {logcontent}");
            }
            catch
            {
                // ignored
            }
        }

        public static void WriteOperationLog(string logtype, string logcontent)
        {
            if (!ActiveOperationLog)
                return;

            try
            {
                log.Info($"{logtype} \t {logcontent}");
            }
            catch
            {
                // ignored
            }
        }

        public static void WriteDebugLog(string logtype, string logcontent)
        {
            if (!ActiveDebugLog)
                return;

            try
            {
                log.Debug($"{logtype} \t {logcontent}");
            }
            catch
            {
                // ignored
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                try
                {
                    if (stream != null)
                        stream.Close();
                }
                catch
                {

                }
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static string GetBitStr(byte[] data)
        {
            BitArray bits = new BitArray(data);

            string strByte = string.Empty;
            for (int i = 0; i <= bits.Count - 1; i++)
            {
                if (i % 8 == 0)
                {
                    strByte += " ";
                }
                strByte += (bits[i] ? "1" : "0");
            }

            return strByte;
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public static DateTime FormatDateTime(DateTime dt, int sec)
        {
            TimeSpan t = TimeSpan.FromSeconds(sec);
            if (sec > 0)
            {
                string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
                return Convert.ToDateTime(dt.ToString("yyyy/MM/dd") + " " + answer);
            }
            else
            {
                string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
                return Convert.ToDateTime(dt.ToString("MM/dd/yyyy"));
            }
        }

        public static List<string> ReadFile(string path)
        {
            List<string> list = new List<string>(File.ReadAllLines(path));
            return list;
        }
    }
}
