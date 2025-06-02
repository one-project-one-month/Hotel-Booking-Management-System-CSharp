using HotelManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.BookingControl;

public class GetBookingControlModel
{

}

public class GetBookingRequestModel
{

}

public class GetBookingResponseModel
{
    public string UserName { get; set; } = null!;
    public string GuestNrc { get; set; } = null!;

    public string GuestPhoneNo { get; set; } = null!;
    
    public List<string> RoomNo { get; set; } = new();
    public int? GuestCount { get; set; }
    public DateOnly? CheckIn_Time { get; set; }

    public DateOnly? CheckOut_Time { get; set; }

    public decimal? Deposit_Amount { get; set; }

    public string? BookingStatus { get; set; }

    public decimal? TotalAmount { get; set; }
    public string? PaymentType { get; set; }

    //public Guid BookingId { get; set; }

    //public Guid? UserId { get; set; }

    //public Guid? GuestId { get; set; }   

    public DateTime? CreatedAt { get; set; }
        
}
public class GetBookingsResponseModel : BasedResponseModel
{
    public List<GetBookingResponseModel>? Bookings { get; set; } = new();
}
