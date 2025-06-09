using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Booking
{
    public class ListBookingDto
    {
        public Guid BookingId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class ListBookingRequestByUserDto
    {
        public Guid UserId { get; set; }
    }
    public class ListBookingResponseDto : BasedResponseModel
    {
        public List<ListBookingDto>? Bookings { get; set; }
    }
}
