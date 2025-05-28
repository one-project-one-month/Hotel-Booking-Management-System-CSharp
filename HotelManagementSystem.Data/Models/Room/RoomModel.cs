using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models.RoomType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Room
{
    public class RoomModel
    {
        public string? RoomNo { get; set; }

        public string? RoomStatus { get; set; }

        public int? GuestLimit { get; set; }

        //public Guid RoomTypeId { get; set; }

        public bool IsFeatured { get; set; }

        public RoomTypeModel? RoomType { get; set; }
    }

    public class RoomResponseModel : BasedResponseModel
    {
        //public string RoomNo { get; set; }

        //public string? RoomStatus { get; set; }

        //public int? GuestLimit { get; set; }

        ////public Guid RoomTypeId { get; set; }

        //public bool IsFeatured { get; set; }

        //public RoomTypeDto? RoomType { get; set; }

        public RoomModel Room { get; set; }
    }

    public class RoomListResponseModel : BasedResponseModel
    {
        public List<RoomModel> RoomList { get; set; }
    }
}
