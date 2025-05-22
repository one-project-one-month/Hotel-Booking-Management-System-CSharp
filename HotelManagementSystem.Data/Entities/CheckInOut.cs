namespace HotelManagementSystem.Data.Entities;

public partial class CheckInOut
{
    public int CheckInOutId { get; set; }

    public Guid GuestId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? ExtraCharges { get; set; }

    public string Status { get; set; } = null!;

    public virtual TblGuest Guest { get; set; } = null!;
}
