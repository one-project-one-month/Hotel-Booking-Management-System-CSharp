namespace HotelManagementSystem_Web.Models.Guest
{
    public class CheckOutReqModel
    {
        public Guid GuestId { get; set; }
    }
    public class CheckOutRespModel : BaseResponseModel
    {
        public Guid InvoiceId { get; set; }
        public Guid GuestId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public decimal? DepositeAmount { get; set; }
        public decimal? Extracharges { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentType { get; set; }
    }
}
