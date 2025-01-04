using System;
using System.Data.SqlClient;

namespace JobPortal
{
    public static class JobSeeker
    {
        public static void ViewVacancies(string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetVacancyDetails", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Logger.Log("Vacancies are viewed");

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
            catch (Exception exceptionn)
            {
                Console.WriteLine($"An error occurred: {exceptionn.Message}");
            }
        }
    }
}
