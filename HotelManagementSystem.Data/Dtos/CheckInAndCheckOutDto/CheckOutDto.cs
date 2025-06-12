using HotelManagementSystem.Data.Models;
namespace HotelManagementSystem.Data.Dtos.CheckInAndCheckOutDto
{
    internal class CheckOutDto
    {
    }
    public class CheckOutRequestDto : BasedRequestModel
    {
        public Guid GuestId { get; set; }
    }
    public class CheckOutResponseDto : BasedResponseModel
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

    public class InvoiceDto
    {
        public Guid GuestId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public decimal? DepositeAmount { get; set; }
        public decimal? Extracharges { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentType { get; set; }
    }
}
