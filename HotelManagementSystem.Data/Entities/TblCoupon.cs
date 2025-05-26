using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblCoupon
{
    public Guid CouponId { get; set; }

    public Guid? GuestId { get; set; }

    public Guid? BookingId { get; set; }

    public string? CouponCode { get; set; }

    public decimal? DiscountPct { get; set; }

    public DateTime? ExpireDate { get; set; }

    public bool? IsClaimed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual TblBooking? Booking { get; set; }

    public virtual TblGuest? Guest { get; set; }
}
