namespace HotelManagementSystem.Data.Models.User;

public class CreateUserModel
{
    
}

public class CreateUserRequestModel : BasedRequestModel
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserResponseModel : BasedResponseModel
{
    public string UserId { get; set; }
}