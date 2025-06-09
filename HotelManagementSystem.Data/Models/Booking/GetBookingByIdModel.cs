namespace HotelManagementSystem.Data.Models.Booking
{
    public class GetBookingByIdModel
    {
    }
    public class GetBookingByIdRequestModel : BasedRequestModel
    {
        public Guid BookingId { get; set; }
    }
    public class GetBookingByIdResponseModel : BasedResponseModel
    {
        public Guid BookingId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public List<string>? RoomNumbers { get; set; }
        public int? Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
    }
}
