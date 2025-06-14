using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Room
{
    public class RoomAndRoomTypeModel
    {
        public Guid Roomid { get; set; }
        public string? RoomNo { get; set; }
        public Guid RoomTypeId { get; set; }
    }
}
