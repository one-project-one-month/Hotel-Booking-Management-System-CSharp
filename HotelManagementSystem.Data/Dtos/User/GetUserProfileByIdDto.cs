using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class GetUserProfileByIdDto
    {
    }
    public class GetUserProfileByIdRequestDto
    {
        public Guid UserId { get; set; }
    }
    public class GetUserProfileByIdResponseDto : BasedResponseModel
    {
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
}
