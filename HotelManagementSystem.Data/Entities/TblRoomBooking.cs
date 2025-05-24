using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblRoomBooking
{
    public int Id { get; set; }

    public Guid RoomId { get; set; }

    public Guid BookingId { get; set; }

    public virtual TblBooking Booking { get; set; } = null!;

    public virtual TblRoom Room { get; set; } = null!;
}
