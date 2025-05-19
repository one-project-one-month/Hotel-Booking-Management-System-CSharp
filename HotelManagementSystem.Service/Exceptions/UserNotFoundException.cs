namespace HotelManagementSystem.Service.Exceptions
{
    public class UserNotFoundException(string email) : Exception($"User with {email} cannot be found");
}
