using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Room
{
    public class RoomModel
    {
    }

    public class RoomResponseModel : BasedResponseModel
    {
        public string RoomNo { get; set; }

        public string? RoomStatus { get; set; }

        public int? GuestLimit { get; set; }

        public Guid RoomTypeId { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class RoomListResponseModel : BasedResponseModel
    {
        public List<RoomResponseModel> RoomList { get; set; }
    }
}
