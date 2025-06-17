namespace HotelManagementSystem_Web.Models.Booking;

public class BookingCreateResModel : BaseResponseModel
{
    public Guid bookingId { get; set; }
    public Guid guestId { get; set; }
}