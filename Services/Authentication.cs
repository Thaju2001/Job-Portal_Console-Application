using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public class Authentication
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ILoggerService _loggerService;
        private readonly Employer _employer; 
        private readonly JobSeeker _jobSeeker;

        public Authentication(IDatabaseConnection databaseConnection, ILoggerService loggerService, Employer employer, JobSeeker jobSeeker)
        { 
            _databaseConnection = databaseConnection;
            _loggerService = loggerService;
            _employer = employer;
            _jobSeeker = jobSeeker;
        }

        public void Login(string role)
        {
            try
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = ReadPassword(); // Use the ReadPassword method for masking

                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UserLogin", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@role", role);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        _loggerService.Log($"Employer menu accessed by username: {username}");
                        Console.WriteLine($"Login successful as {role}");
                    
                        if (role == "Employer")
                        {
                            _employer.Menu();
                        }
                        else if (role == "JobSeeker")
                        {
                            _jobSeeker.JobseekerMenu();
                        }
                    }
                    else
                    {
                        _loggerService.Log($"Employer menu cannot be accessed by username: {username}");
                        Console.WriteLine("Invalid credentials.");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        public void RegisterEmployer()
        {
            try
            {
                string username;
                bool isValidUsername = false;

                do
                {
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    isValidUsername = Validation.ValidateUsername(username);

                    if (!isValidUsername)
                    {
                        Console.WriteLine("Invalid username. Please try again.");
                    }
                } while (!isValidUsername);

                string password;
                bool isValidPassword = false;

                do
                {
                    Console.Write("Enter password: ");
                    password = ReadPassword();
                    isValidPassword = Validation.ValidatePassword(password);

                    if (!isValidPassword)
                    {
                        Console.WriteLine("Invalid password. Please try again.");
                    }
                } while (!isValidPassword);

                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RegisterEmployer", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                    _loggerService.Log($"Employer successfully registered by username: {username}");

                    Console.WriteLine("Employer registered successfully.");
                     _employer.Menu();

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        public void RegisterJobSeeker()
        {
            try
            {
                string username;
                bool isValidUsername = false;

                do
                {
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    isValidUsername = Validation.ValidateUsername(username);

                    if (!isValidUsername)
                    {
                        _loggerService.Log($"Job seeker cannot access menu {username}");
                        Console.WriteLine("Invalid username. Please try again.");
                    }
                } while (!isValidUsername);

                string password;
                bool isValidPassword = false;

                do
                {
                    Console.Write("Enter password: ");
                    password = ReadPassword();
                    isValidPassword = Validation.ValidatePassword(password);

                    if (!isValidPassword)
                    {
                        _loggerService.Log($"Job seeker entered invalid password by username: {username}");
                        Console.WriteLine("Invalid password. Please try again.");
                    }
                } while (!isValidPassword);

                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RegisterJobSeeker", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                    _loggerService.Log($"Job seeker menu accessed by username: {username}");

                    Console.WriteLine("Job Seeker registered successfully.");
                    _jobSeeker.JobseekerMenu();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        private static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password.Substring(0, password.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}
