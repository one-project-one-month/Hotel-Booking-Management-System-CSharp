using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_RoomBooking")]
    public class RoomBookingEntity
    {
        [Key]
        public int Id { get; set; }

        public Guid RoomId { get; set; }

        public Guid BookingId { get; set; }

        public virtual BookingEntity Booking { get; set; } = null!;

        public virtual RoomEntity Room { get; set; } = null!;
    }
}
