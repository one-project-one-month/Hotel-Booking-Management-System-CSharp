﻿using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.Booking
{
    public class CreateBookingByAdminResponseDto : BasedResponseModel
    {
        public Guid BookingId { get; set; }
    }
    public class CreateBookingByAdminRequestDto
    {
        public Guid? UserId { get; set; }

        public Guid? GuestId { get; set; }
        public string Name { get; set; } = null!;
        public string Nrc { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public int? GuestCount { get; set; }

        public DateOnly? CheckInTime { get; set; }

        public DateOnly? CheckOutTime { get; set; }

        public decimal? DepositAmount { get; set; }

        public string? BookingStatus { get; set; }

        public decimal? TotalAmount { get; set; }
        public string? PaymentType { get; set; }
    }
}
