using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.SendSMS.Code;

namespace TN.SendSMS.DataHelper
{
    class ConnectionHelper
    {
        public static SqlConnection GetConnection(string connectionString, SqlConnection connection)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                Console.WriteLine("+----------------------------------------------------------------------------------------+");
                Console.WriteLine($"| Opening Connection To {connectionString.Split(';')[1].Replace("Initial Catalog=","Database : ")} ...");
                connection.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("| Connection successful!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("+----------------------------------------------------------------------------------------+");
                Utilities.WriteDebugLog("CONNECTION TO SQL SERVER : ", "Connection successful !");
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine("+----------------------------------------------------------------------------------------+");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"| Can't Connection To {connectionString.Split(';')[1].Replace("Initial Catalog=", "Database : ")}!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("+----------------------------------------------------------------------------------------+");
                return null;
            }

        }

        public static void CloseConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                try
                {
                    Console.WriteLine("+----------------------------------------------------------------------------------------+");

                    Console.WriteLine("| Closing Connection ...");
                    connection.Close();
                    Console.WriteLine("| Close successful ...");
                    Utilities.WriteDebugLog("CONNECTION TO SQL SERVER : ", "Connection Closed !");
                }
                catch (Exception e)
                {
                    Console.WriteLine("+----------------------------------------------------------------------------------------+");
                    Console.WriteLine("| No connection to Db !");
                }
            }

        }
    }
}
