namespace HotelManagementSystem.Service.Exceptions
{
    public class UserDoesNotExitException(Guid Id) : Exception($"User does not exit with this {Id}");
}
