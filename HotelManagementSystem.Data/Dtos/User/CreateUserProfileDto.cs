using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class CreateUserProfileDto
    {
    }
    public class CreateUserProfileRequestDto
    {
        public Guid UserId { get; set; } 
        public string? UserName { get; set; }
        public byte[]? ProfileImg { get; set; }
    }
    public class CreateUserProfileResponseDto : BasedResponseModel
    {

    }
}
