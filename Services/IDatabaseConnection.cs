namespace JobPortal
{
    public interface IDatabaseConnection
    {
        string ConnectionString { get; }
    }

    public class DatabaseConnection : IDatabaseConnection
    {
        public string ConnectionString { get; }

        public DatabaseConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
