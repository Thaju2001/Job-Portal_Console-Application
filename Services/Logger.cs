using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public static class Logger
    {
        private static string connectionString =  "Server=ASDLAPKCH0633\\SQLEXPRESS; Database=Jobportal; Trusted_Connection=True";

        public static void Log(string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Logs (LogMessage, LogDate) VALUES (@logMessage, @logDate)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@logMessage", message);
                    cmd.Parameters.AddWithValue("@logDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exceptionn)
            {
                Console.WriteLine($"Logging failed: {exceptionn.Message}");
            }
        }
    }
}
