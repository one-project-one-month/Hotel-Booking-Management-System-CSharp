using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Booking")]
    public class BookingEntity
    {
        [Key]
        [Column("BookingId")]
        public string BookingId { get; set; } = Guid.NewGuid().ToString();
        [Column("UserId")]
        public string? UserId { get; set; }
        [Column("GuestId")]
        public string? GuestId { get; set; }
        [Column("Guest_Count")]
        public int Guest_Count { get; set; }
        [Column("Booking_Status")]
        public string? Booking_Status { get; set; }
        [Column("CheckIn_Time")]
        public DateTime CheckInDate { get; set; }
        [Column("Deposit_Amount")]
        public decimal? Deposit_Amount { get; set; }
        [Column("Total_Amount")]
        public decimal Total_Amount { get; set; }
        public DateTime CheckOutDate { get; set; }
        [Column("PaymentType")]
        public string? PaymentType { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
