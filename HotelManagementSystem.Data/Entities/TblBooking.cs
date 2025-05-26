using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblBooking
{
    public Guid BookingId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GuestId { get; set; }

    public int? GuestCount { get; set; }

    public DateOnly? CheckInTime { get; set; }

    public DateOnly? CheckOutTime { get; set; }

    public decimal? DepositAmount { get; set; }

    public string? BookingStatus { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? PaymentType { get; set; }

    public virtual TblGuest? Guest { get; set; }

    public virtual ICollection<TblCoupon> TblCoupons { get; set; } = new List<TblCoupon>();

    public virtual ICollection<TblRoomBooking> TblRoomBookings { get; set; } = new List<TblRoomBooking>();

    public virtual TblUser? User { get; set; }
}
