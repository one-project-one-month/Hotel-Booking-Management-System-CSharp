using Microsoft.AspNetCore.Http;

namespace HotelManagementSystem.Data.Models.User
{
    public class CreateUserProfileModel
    {
    }
    public class CreateUserProfileRequestModel
    {
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public IFormFile? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
    public class  CreateUserResponseModel : BasedResponseModel
    {
        
    }
    public class CreateUserProfileByAdminRequestModel
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public IFormFile? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
}
