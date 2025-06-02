using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.BookingControl;

public class UpdateBookingDto
{
}

public class UpdateBookingRequestDto
{
    public Guid BookingId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GuestId { get; set; }
    public List<Guid> Rooms { get; set; } = new();

    public int? GuestCount { get; set; }

    public DateOnly? CheckInTime { get; set; }

    public DateOnly? CheckOutTime { get; set; }

    public decimal? DepositAmount { get; set; }

    public string? BookingStatus { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? PaymentType { get; set; }
}

public class UpdateBookingResponseDto : BasedResponseModel { }
