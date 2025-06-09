using HotelManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Booking
{
    public class CreateBookingDto
    {
        
    }
    public class CreateBookingRequestDto
    {
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public List<Guid>? Rooms { get; set; }
        public int Guest_Count { get; set; }
        public string? Booking_Status { get; set; }
        public decimal Total_Amount { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public string? PaymentType { get; set; }
        public decimal? Deposit_Amount { get; set; }
    }
    public class CreateBookingResponseDto : BasedResponseModel
    {
        public string? BookingId { get; set; }
    }
}