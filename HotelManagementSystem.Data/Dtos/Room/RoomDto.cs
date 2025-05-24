using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Room
{
    public class RoomDto
    {
    }

    public class RoomResponseDto : BasedResponseModel
    {
        public string RoomNo { get; set; }

        public string? RoomStatus { get; set; }

        public int? GuestLimit { get; set; }

        public Guid RoomTypeId { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class RoomListResponseDto : BasedResponseModel
    {
        public List<RoomResponseDto> RoomList { get; set; } = new();
    }
}
