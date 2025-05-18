using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class LoginDto
    {

    }

    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponseDto : BasedResponseModel
    {

    }
}
