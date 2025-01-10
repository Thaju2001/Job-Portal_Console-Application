using System;
using System.Data.SqlClient;
using JobPortal;

namespace JobPortal
{
    public class Employer
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ILoggerService _loggerService;
        private readonly JobSeeker _jobSeeker; // Add JobSeeker as a dependency
        public Employer(IDatabaseConnection databaseConnection, ILoggerService loggerService, JobSeeker jobSeeker)
        {
            _databaseConnection = databaseConnection;
            _loggerService = loggerService;
            _jobSeeker = jobSeeker; // Initialize JobSeeker dependency
        }

        public void Menu()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.WriteLine("\n1. View Vacancy");
                    Console.WriteLine("2. Add Vacancy");
                    Console.WriteLine("3. Delete Vacancy");
                    Console.WriteLine("4.Vacancies interested by JobSeekers ");
                    Console.WriteLine("5. Exit");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            _jobSeeker.ViewVacancies();
                            break;
                        case 2:
                            AddVacancy();
                            break;
                        case 3:
                            DeleteVacancy();
                            break;
                        case 4:
                        DisplayInterestedVacancies();
                        break;


                        case 5:
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
                            _loggerService.Log("Exited from employer's menu");
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

        public void AddVacancy()
        {
            try
            {
                Console.Write("Enter Employer ID: ");
                string employerId = Console.ReadLine();
                Console.Write("Enter job title: ");
                string jobTitle = Console.ReadLine();
                Console.Write("Enter job description: ");
                string jobDescription = Console.ReadLine();

                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AddVacancy", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@employerId", employerId);
                    cmd.Parameters.AddWithValue("@jobTitle", jobTitle);
                    cmd.Parameters.AddWithValue("@jobDescription", jobDescription);
                    cmd.ExecuteNonQuery();
                    _loggerService.Log($"Vacancies added by Employer_ID {employerId}");

                    Console.WriteLine("Vacancy added successfully.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        public void DeleteVacancy()
        {
            try
            {
                Console.Write("Enter vacancy ID: ");
                int vacancyId = Convert.ToInt32(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(_databaseConnection.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DeleteVacancy", conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@vacancyId", vacancyId);
                    cmd.ExecuteNonQuery();
                    _loggerService.Log($"Vacancies deleted by vacancy ID {vacancyId}");

                    Console.WriteLine("Vacancy deleted successfully.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

public void DisplayInterestedVacancies()
        {
            try
            {
                Console.WriteLine("\nVacancies interested by JobSeekers:");
                if (_jobSeeker.InterestedVacancies.Count > 0)
                {
                    foreach (var vacancy in _jobSeeker.InterestedVacancies)
                    {
                        Console.WriteLine($"- Vacancy ID: {vacancy}");
                    }
                }
                else
                {
                    Console.WriteLine("No vacancies have been marked as interested by JobSeekers.");
                }
                _loggerService.Log("Displayed interested vacancies");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }
   

    }
}
