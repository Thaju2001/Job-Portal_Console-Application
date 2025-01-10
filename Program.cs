/*Title      - Job Portal Application
 Author     - C M Thajudheen
 Created at - 19/12/2024
 Updated at - 28/12/2024
Reviewed by - Saraswathi Sathiah*/
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace JobPortal
{
    class Program
    {
        private static IConfiguration _configuration;
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            LoadConfiguration();
            ConfigureServices();

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

                var authentication = _serviceProvider.GetService<Authentication>();

                switch (choice)
                {
                    case 1:
                        authentication.Login("Employer");
                        break;
                    case 2:
                        authentication.Login("JobSeeker");
                        break;
                    case 3:
                        authentication.RegisterEmployer();
                        break;
                    case 4:
                        authentication.RegisterJobSeeker();
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
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(@"D:\Projectworking\ExchEmp\Config\appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static void ConfigureServices()
        {
            //Container creation
            var serviceCollection = new ServiceCollection();

            // Register configuration 
            serviceCollection.AddSingleton<IConfiguration>(_configuration);

            // Register services
            serviceCollection.AddSingleton<ILoggerService, LoggerService>();
            serviceCollection.AddSingleton<IDatabaseConnection>(provider => new DatabaseConnection(_configuration.GetConnectionString("EmpexchDb")));
            serviceCollection.AddSingleton<Authentication>();
            serviceCollection.AddSingleton<Employer>();
            serviceCollection.AddSingleton<JobSeeker>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
