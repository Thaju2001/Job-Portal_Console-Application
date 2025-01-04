//Tiltle      - Job Portal Application
//Authot      - C M Thajudheen
//Created at  -19/12/2024
//Updated at  -28/12/2024
//Reviewed by - Sabapathi Shanmugam
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JobPortal
{
    class Program
    {
        private static string _connectionString;

        static void Main(string[] args)
        {
            LoadConfiguration();
            
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n_________________________________");

                Console.WriteLine("\nWelcome to Job Portal, Choose an option:");
                Console.WriteLine("1. Login as Employer");
                Console.WriteLine("2. Login as Job Seeker");
                Console.WriteLine("3. Register as Employer");
                Console.WriteLine("4. Register as Jobseeker");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Authentication.Login("Employer", _connectionString);
                        break;
                    case 2:
                        Authentication.Login("JobSeeker", _connectionString);
                        break;
                    case 3:
                        Authentication.RegisterEmployer(_connectionString);
                        break;
                    case 4:
                        Authentication.RegisterJobSeeker(_connectionString);
                        break;
                    case 5:
                        exit = true;
                        Console.WriteLine("Exiting the application.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("Utils/appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("EmpexchDb") ?? throw new InvalidOperationException("Connection string 'EmpexchDb' is not configured.");
        }
    }
}
