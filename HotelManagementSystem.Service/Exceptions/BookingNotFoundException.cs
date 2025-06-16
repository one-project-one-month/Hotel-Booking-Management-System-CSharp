namespace HotelManagementSystem.Service.Exceptions
{
    public class BookingNotFoundException(string BookingId) : Exception($"Booking with {BookingId} cannot be found");
}
