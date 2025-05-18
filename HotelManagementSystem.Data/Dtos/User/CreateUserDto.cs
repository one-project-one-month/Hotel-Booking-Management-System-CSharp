using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.User;

public class CreateUserDto
{
    
}

public class CreateUserRequestDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserResponseDto : BasedResponseModel
{
    
}