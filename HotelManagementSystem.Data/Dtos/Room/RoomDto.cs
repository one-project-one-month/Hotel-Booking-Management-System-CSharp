using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Room
{
    public class RoomDto
    {
        public string? RoomNo { get; set; }

        public string? RoomStatus { get; set; }

        public int? GuestLimit { get; set; }

        //public Guid RoomTypeId { get; set; }

        public bool IsFeatured { get; set; }

        public RoomTypeDto? RoomType { get; set; }
    }

    public class RoomResponseDto : BasedResponseModel
    {
        //public string? RoomNo { get; set; }

        //public string? RoomStatus { get; set; }

        //public int? GuestLimit { get; set; }

        ////public Guid RoomTypeId { get; set; }

        //public bool IsFeatured { get; set; }

        //public RoomTypeDto? RoomType { get; set; }  
        public RoomDto Room { get; set; }
    }

    public class RoomListResponseDto : BasedResponseModel
    {
        public List<RoomDto> RoomList { get; set; } = new();
    }
}
