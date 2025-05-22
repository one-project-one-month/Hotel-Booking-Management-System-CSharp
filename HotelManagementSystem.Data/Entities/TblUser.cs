using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblUser
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid RoleId { get; set; }

    public byte[]? ProfileImg { get; set; }

    public int? Points { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? TokenExpireAt { get; set; }

    public string? ForgetPasswordOtp { get; set; }

    public DateTime? OtpExpireAt { get; set; }

    public string? Password { get; set; }

    public virtual TblRole Role { get; set; } = null!;

    public virtual ICollection<TblBooking> TblBookings { get; set; } = new List<TblBooking>();

    public virtual ICollection<TblGuest> TblGuests { get; set; } = new List<TblGuest>();
}
