namespace HotelManagementSystem.Data.Models.CheckInAndCheckOutModel
{
    public class CheckOutModel
    {
    }
    public class CheckOutRequestModel : BasedRequestModel
    {
        public Guid GuestId { get; set; }
    }

    public class  CheckOutResponseModel : BasedResponseModel
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
