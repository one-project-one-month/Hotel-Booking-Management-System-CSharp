using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Check_In_Out")]
    public class CheckInOutEntity
    {
        [Key]
        public int CheckInOutId { get; set; }

        public Guid GuestId { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public decimal? ExtraCharges { get; set; }

        public string Status { get; set; } = null!;

        public virtual GuestEntity Guest { get; set; } = null!;
    }
}
