using HotelManagementSystem.Data.Models;
using Microsoft.AspNetCore.Http;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class CreateUserProfileDto
    {
    }
    public class CreateUserProfileRequestDto
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
    public class CreateUserProfileResponseDto : BasedResponseModel
    {

    }
    public class CreateUserProfileByAdminRequestDto
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
}
