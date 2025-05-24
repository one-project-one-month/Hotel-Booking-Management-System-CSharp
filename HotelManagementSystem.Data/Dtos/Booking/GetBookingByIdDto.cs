using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Booking
{
    public class GetBookingByIdDto
    {
    }
    public class GetBookingByIdRequestDto
    {
        public string? BookingId { get; set; }
    }
    public class GetBookingByIdResponseDto : BasedResponseModel
    {
        public string? BookingId { get; set; }
        public string? UserId { get; set; }
        public string? GuestId { get; set; }
        public int Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
    }
}
