using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.SearchRoom
{
    public class SearchRoomDto
    {
    }
    public class SearchRoomRequestDto
    {
        public string? RoomType { get; set; }

        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }

        public DateOnly? CheckInDate { get; set; }

        public DateOnly? CheckOutDate { get; set; }
    }

    public class SearchRoomResponseDto : BasedResponseModel
    {
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    }


    public class RoomDto: BasedResponseModel
    {
        public Guid RoomId { get; set; }
        public string? RoomType { get; set; }
        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }
        public string? RoomNumber { get; set; }
        public string? Description { get; set; }
        public byte[]? ImgUrl { get; set; }
    }
}
