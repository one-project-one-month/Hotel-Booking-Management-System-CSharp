namespace HotelManagementSystem.Data.Entities;

public partial class TblInvoice
{
    public Guid InvoiceId { get; set; }

    public Guid GuestId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime CheckOutTime { get; set; }

    public decimal? ExtraCharges { get; set; }

    public decimal Deposite { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? PaymentType { get; set; }

    public virtual TblGuest Guest { get; set; } = null!;
}
