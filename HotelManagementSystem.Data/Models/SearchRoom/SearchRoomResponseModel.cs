using HotelManagementSystem.Data.Dtos.SearchRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.SearchRoom
{
    public class SearchRoomResponseModel : BasedResponseModel
    {
        //public string? RoomType { get; set; }
        //public decimal? Price { get; set; }
        //public int? GuestLimit { get; set; }
        //public string? RoomNumber { get; set; }
        //public string? Description { get; set; }
        //public byte[]? ImgUrl { get; set; }

        public List<RoomDto> Rooms { get; set; } = new();
    }
}
