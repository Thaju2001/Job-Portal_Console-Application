using System;
using System.Text.RegularExpressions;

namespace JobPortal
{
    public static class Validation
    {
        public static bool ValidateUsername(string username)
        {
            if (username.Length < 3 || username.Length > 20)
            {
                Console.WriteLine("Username must be between 3 and 20 characters.");
                return false;
            }

            if (!Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
            {
                Console.WriteLine("Username can only contain alphanumeric characters.");
                return false;
            }

            return true;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 6 || password.Length > 20)
            {
                Console.WriteLine("Password must be between 6 and 20 characters.");
                return false;
            }

            if (!Regex.IsMatch(password, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$"))
            {
                Console.WriteLine("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
                return false;
            }

            return true;
        }
    }
}
