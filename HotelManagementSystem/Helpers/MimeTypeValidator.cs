namespace HotelManagementSystem.Helpers
{
    public class MimeTypeValidator
    {
        public static bool IsValidatorMimeType(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR": 
                    return true; 
                case "/9J/": 
                    return true;
                default:
                    return false;
            }
        }
    }
}
