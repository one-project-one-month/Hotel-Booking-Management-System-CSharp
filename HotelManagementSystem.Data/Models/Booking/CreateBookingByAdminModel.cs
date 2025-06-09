using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Booking
{
    internal class CreateBookingByAdminModel
    {
    }
    public class CreateBookingByAdminRequestModel
    {

        public Guid? UserId { get; set; }

        public Guid? GuestId { get; set; }
        public string Name { get; set; } = null!;
        public string Nrc { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;
        public List<Guid> Rooms { get; set; } = new();

        public int? GuestCount { get; set; }

        public DateOnly? CheckInTime { get; set; }

        public DateOnly? CheckOutTime { get; set; }

        public decimal? DepositAmount { get; set; }

        public string? BookingStatus { get; set; }

        public decimal? TotalAmount { get; set; }
        public string? PaymentType { get; set; }
    }

    public class CreateBookingByAdminResponseModel : BasedResponseModel
    {
        public Guid BookingId { get; set; }
    }
}
