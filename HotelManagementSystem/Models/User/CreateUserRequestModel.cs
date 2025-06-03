namespace HotelManagementSystem.Models.User;
public class CreateUserRequestModel :BasedRequestModel
{
    public string Name { get; set; }
    public string Email { get; set; }
}
