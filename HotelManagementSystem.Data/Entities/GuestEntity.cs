using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Guest")]
    public class GuestEntity
    {
        [Key]
        public Guid GuestId { get; set; }

        public Guid? UserId { get; set; }

        public string Nrc { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<CheckInOutEntity> CheckInOutEntities { get; set; } = new List<CheckInOutEntity>();

        public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new List<BookingEntity>();

        public virtual ICollection<CouponEntity> CouponEntities { get; set; } = new List<CouponEntity>();

        public virtual ICollection<InvoiceEntity> InvoiceEntities { get; set; } = new List<InvoiceEntity>();

        public virtual UserEntity? User { get; set; }
    }
}
