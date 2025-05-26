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

public class GetBookingsRequestDto
{

}

public class GetBookingResponseDto : BasedResponseModel
{

    public Guid BookingId { get; set; }

    public Guid? UserId { get; set; }

    //public Guid GuestId { get; set; }

    //public int GuestCount { get; set; }

    //public DateTime CheckIn_Time { get; set; }

    //public DateTime CheckOut_Time { get; set; }

    //public decimal Deposit_Amount { get; set; }

    //public string BookingStatus { get; set; }

    //public decimal TotalAmount { get; set; }

    //public string GuestName { get; set; }

    //public string GuestPhoneNumber { get; set; }
}
public class GetBookingsResponseDto : BasedResponseModel
{
    public List<GetBookingResponseDto> Bookings { get; set; } = new();
}
