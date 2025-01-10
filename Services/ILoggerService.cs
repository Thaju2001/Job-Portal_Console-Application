using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public interface ILoggerService
    {
        void Log(string message);
    }

    public class LoggerService : ILoggerService
    {
        private readonly IDatabaseConnection _databaseConnection;

        public LoggerService(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Log(string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertLog", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@logMessage", message);
                        cmd.Parameters.AddWithValue("@logDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Logging failed: {exception.Message}");
            }
        }
    }
}
