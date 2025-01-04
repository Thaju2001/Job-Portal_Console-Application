using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public static class Authentication
    {
        public static void Login(string role, string connectionString)
        {
            try
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = ReadPassword(); // Use the ReadPassword method for masking

                using (SqlConnection conn = new SqlConnection(connectionString))
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
                        Logger.Log($"Employer menu accessed by username: {username}");
                        Console.WriteLine($"Login successful as {role}");
                    
                        if (role == "Employer")
                        {
                            Employer.Menu(connectionString);
                        }
                        else if (role == "JobSeeker")
                        {
                            JobSeeker.ViewVacancies(connectionString);
                        }
                    }
                    else
                    {
                        Logger.Log($"Employer menu cannot accessed by username: {username}");
                        Console.WriteLine("Invalid credentials.");
                    }
                }
            }
            catch (Exception exceptionn)
            {
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
            }
        }

        public static void RegisterEmployer(string connectionString)
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
                        Console.WriteLine("Invalid username Please try again.");
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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RegisterEmployer", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                    Logger.Log($"Employer successfully registered by username: {username}");


                    Console.WriteLine("Employer registered successfully.");
                }
            }
            catch (Exception exceptionn)
            {
                
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
            }
        }

        public static void RegisterJobSeeker(string connectionString)
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
                         Logger.Log($"job seeker  cannot access menu {username}");
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
                        Logger.Log($"Jobseeker entered invalid password by username: {username}");
                        Console.WriteLine("Invalid password. Please try again.");
                    }
                } while (!isValidPassword);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("RegisterJobSeeker", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                    Logger.Log($"Jobseeker menu accessed by username: {username}");

                    Console.WriteLine("Job Seeker registered successfully.");
                    JobSeeker.ViewVacancies(connectionString);
                }
            }
            catch (Exception exceptionn)
            {
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
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
