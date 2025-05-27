using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Entities
{
    [Table("Tbl_Role")]
    public class RoleEntity
    {
        [Key]
        public Guid RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public virtual ICollection<UserEntity> UserEntities { get; set; } = new List<UserEntity>();
    }
}
