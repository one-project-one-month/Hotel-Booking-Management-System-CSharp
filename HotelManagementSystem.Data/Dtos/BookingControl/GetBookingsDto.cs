using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.BookingControl;

public class GetBookingsDto
{

}

public class GetBookingRequestDto
{

}

public class GetBookingResponseDto
{
    public Guid BookingId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? GuestId { get; set; }

    public string? UserName { get; set; }
    public string? GuestName { get; set; }
    public string GuestNrc { get; set; } = null!;
    public string GuestPhoneNo { get; set; } = null!;

    public List<string> RoomNo { get; set; } = new();
    public int? GuestCount { get; set; }
    public DateOnly? CheckInTime { get; set; }
    public DateOnly? CheckOutTime { get; set; }
    public decimal? DepositAmount { get; set; }
    public string? BookingStatus { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? PaymentType { get; set; }
    public DateTime? CreatedAt { get; set; }
}
public class GetBookingsResponseDto : BasedResponseModel
{
    public List<GetBookingResponseDto> Bookings { get; set; } = new();
}
