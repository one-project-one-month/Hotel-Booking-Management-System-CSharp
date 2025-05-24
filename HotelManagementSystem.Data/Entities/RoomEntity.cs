using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Room")]
    public class RoomEntity
    {
        [Key]
        public Guid RoomId { get; set; }

        public string RoomNo { get; set; } = null!;

        public Guid RoomTypeId { get; set; }

        public string? RoomStatus { get; set; }

        public byte[]? ImgUrl { get; set; }

        public int? GuestLimit { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual RoomTypeEntity RoomType { get; set; } = null!;

        public virtual ICollection<RoomBookingEntity> RoomBookingEntities { get; set; } = new List<RoomBookingEntity>();
    }
}
