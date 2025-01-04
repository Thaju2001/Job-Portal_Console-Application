
using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public static class Employer
    {
        public static void Menu(string connectionString)
{
    bool exit = false;

    while (!exit)
    {
        try
        {
            Console.WriteLine("\n1. View Vacancy");
            Console.WriteLine("2. Add Vacancy");
            Console.WriteLine("3. Delete Vacancy");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    JobSeeker.ViewVacancies(connectionString);
                    break;
                case 2:
                    AddVacancy(connectionString);
                    break;
                case 3:
                    DeleteVacancy(connectionString);
                    break;
                case 4:
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
                    Logger.Log("Exited from employers menu");
                    exit = true;
                    Console.WriteLine("Exiting the menu.");
                }
            }
        }
        catch (Exception exceptionn)
        {
            Console.WriteLine($"An error occurred: {exceptionn.Message}");
        }
    }
}

        public static void AddVacancy(string connectionString)
        {
            try
            {
                Console.Write("Enter Employer ID ");
                string EmployerId = Console.ReadLine();   
                Console.Write("Enter job title: ");
                string jobTitle = Console.ReadLine();
                Console.Write("Enter job description: ");
                string jobDescription = Console.ReadLine();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AddVacancy", conn);
                    {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }
                    cmd.Parameters.AddWithValue("@employerId",EmployerId);
                    cmd.Parameters.AddWithValue("@jobTitle", jobTitle);
                    cmd.Parameters.AddWithValue("@jobDescription", jobDescription);
                    cmd.ExecuteNonQuery();
                    Logger.Log($"Vacancies are added by Employer_ID {EmployerId}");

                    Console.WriteLine("Vacancy added successfully.");
                }
            }
            catch (Exception exceptionn)
            {
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
            }
        }

        public static void DeleteVacancy(string connectionString)
        {
            try
            {
                Console.Write("Enter vacancy ID: ");
                int vacancyId = Convert.ToInt32(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DeleteVacancy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vacancyId", vacancyId);
                    cmd.ExecuteNonQuery();
                    Logger.Log($"vacancies are deleated by username {vacancyId}");

                    Console.WriteLine("Vacancy deleted successfully.");
                }
            }
            catch (Exception exceptionn)
            {
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
            }
        }
    }
}
