namespace HotelManagementSystem_Web.Models.UserProfile
{
    public class CreateUserProfileReqModel
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImg { get; set; }

        public string? ProfileImgMimeType { get; set; }
    }
}
