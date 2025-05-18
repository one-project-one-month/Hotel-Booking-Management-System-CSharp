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
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? GuestId { get; set; }
        [Required]
        public int Guest_Count { get; set; }
        [Required]
        public string? Booking_Status { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public string? PaymentType { get; set; }
    }
    public class CreateBookingResponseDto : BasedResponseModel
    {
        public Guid BookingId { get; set; }
        public string? UserId { get; set; }
        public string? GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int? Guest_Count { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}