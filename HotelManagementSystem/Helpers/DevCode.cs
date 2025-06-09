namespace HotelManagementSystem.Helpers
{
    public static class DevCode
    {
        public static bool IsValidImage(this string base64String)
        {
            var data = base64String.Substring(0, 5);
            return data == "IVBOR" || data == "/9J/";
        }

        public static PasswordValidationModel IsValidPassword(this string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                errors.Add("Password must contain at least one number.");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add("Password must contain at least one special character.");

            return new PasswordValidationModel { IsValid = errors.Count == 0, Errors = errors };
        }

        public static int ToInt32(this string str)
        {
            return Convert.ToInt32(str);
        }
    }

    public class PasswordValidationModel
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
