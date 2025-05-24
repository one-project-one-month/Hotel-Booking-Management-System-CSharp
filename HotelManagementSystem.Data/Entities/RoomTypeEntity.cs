using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_RoomType")]
    public class RoomTypeEntity
    {
        [Key]
        public Guid RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<RoomEntity> RoomEntities { get; set; } = new List<RoomEntity>();
    }
}
