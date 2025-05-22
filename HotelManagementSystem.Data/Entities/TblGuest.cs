namespace HotelManagementSystem.Data.Entities;

public partial class TblGuest
{
    public Guid GuestId { get; set; }

    public Guid? UserId { get; set; }

    public string Nrc { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CheckInOut> CheckInOuts { get; set; } = new List<CheckInOut>();

    public virtual ICollection<TblBooking> TblBookings { get; set; } = new List<TblBooking>();

    public virtual ICollection<TblCoupon> TblCoupons { get; set; } = new List<TblCoupon>();

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();

    public virtual TblUser? User { get; set; }
}
