using HotelManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;

public class CreateCheckInAndCheckOutDto
{

}

public class CreateCheckInAndCheckOutRequestDto
{
    public Guid GuestId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? ExtraCharges { get; set; }

    public string Status { get; set; } = null!;
}

public class CreateCheckInAndCheckOutResponseDto : BasedResponseModel
{
    //public virtual TblGuest Guest { get; set; } = null!;

    public string GuestNRC { get; set; } = null!;
    public string GuestPhone { get; set; } = null!;

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? ExtraCharges { get; set; }

    public string Status { get; set; } = null!;
}
