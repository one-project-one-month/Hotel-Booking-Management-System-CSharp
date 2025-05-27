using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Coupon")]
    public class CouponEntity
    {
        [Key]
        public Guid CouponId { get; set; }

        public Guid? GuestId { get; set; }

        public Guid? BookingId { get; set; }

        public string? CouponCode { get; set; }

        public decimal? DiscountPct { get; set; }

        public DateTime? ExpireDate { get; set; }

        public bool? IsClaimed { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual BookingEntity? Booking { get; set; }

        public virtual GuestEntity? Guest { get; set; }
    }
}
