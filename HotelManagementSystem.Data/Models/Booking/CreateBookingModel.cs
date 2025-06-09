using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Booking
{
    public class CreateBookingModel
    {
    }
    public class CreateBookingRequestModel
    {
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public List<Guid>? Rooms { get; set; }
        public int Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal Total_Amount { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
    }
    public class CreateBookingResponseModel : BasedResponseModel
    {
        public string? BookingId { get; set; }
    }
}
