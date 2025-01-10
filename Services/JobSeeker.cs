using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace JobPortal
{
    public class JobSeeker
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ILoggerService _loggerService;

        public List<string> InterestedVacancies { get; private set; } = new List<string>();

        public JobSeeker(IDatabaseConnection databaseConnection, ILoggerService loggerService)
        {
            _databaseConnection = databaseConnection;
            _loggerService = loggerService;
        }

        public void JobseekerMenu()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.WriteLine("\n1. View Vacancy");
                    Console.WriteLine("2. Enter the interested job vacancy id ");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            ViewVacancies();
                            break;
                        case 2:
                            InterestedVacancy();
                            break;
                        case 3:
                            exit = true;
                            Console.WriteLine("Exiting the menu.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;  
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\nWould you like to perform another action? (yes/no)");
                        string anotherAction = Console.ReadLine().ToLower();

                        if (anotherAction != "yes")
                        {
                            _loggerService.Log("Exited from jobseeker's menu");
                            exit = true;
                            Console.WriteLine("Exiting the menu.");
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"An error occurred: {exception.Message}");
                }
            }
        }

        //----------------------

        public void InterestedVacancy()
        {
            try
            {
                Console.Write("Enter the job vacancy ID you're interested in: ");
                string vacancyId = Console.ReadLine();
                InterestedVacancies.Add(vacancyId); // Corrected the property name
                _loggerService.Log($"Interested in vacancy ID: {vacancyId}");
                Console.WriteLine("Vacancy ID added to your list of interested vacancies.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        //----------------------

        public void ViewVacancies()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetVacancyDetails", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    _loggerService.Log("Vacancies are viewed");

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Print table header
                    Console.WriteLine("+------------+---------------------+--------------------------------+");
                    Console.WriteLine("| Vacancy ID | Job Title           | Job Description                |");
                    Console.WriteLine("+------------+---------------------+--------------------------------+");

                    while (reader.Read())
                    {
                        string vacancyId = reader["VacancyId"].ToString();
                        string jobTitle = reader["JobTitle"].ToString();
                        string jobDescription = reader["JobDescription"].ToString();

                        // Print each row
                        Console.WriteLine($"| {vacancyId,-10} | {jobTitle,-19} | {jobDescription,-30} |");
                    }

                    // Print table footer
                    Console.WriteLine("+------------+---------------------+--------------------------------+");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }
    }
}
