using HotelManagementSystem.Data.Models;
namespace HotelManagementSystem.Data.Dtos.User
{
    public class RegisterUserDto
    {

    }
    public class RegisterUserrequestDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class RegisterUserResponseDto : BasedResponseModel
    {

    }
}
