using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Booking
{
    public class ListBookingModel
    {
        public Guid? BookingId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public string UserName { get; set; } = null!;
        public string Emai { get; set; } = null!;
        public int? Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class ListBookingRequestModel : BasedRequestModel
    {
        
    }
    public class ListBookingResponseModel : BasedResponseModel
    {
        public List<ListBookingModel>? Booking { get; set; }
    }
}
