using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Invoice")]
    public class InvoiceEntity
    {
        [Key]
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

        public virtual GuestEntity Guest { get; set; } = null!;
    }
}
