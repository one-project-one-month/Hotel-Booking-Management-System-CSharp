using HotelManagementSystem.Data.Models;

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
        public List<RoomSearchDto> Rooms { get; set; } = new List<RoomSearchDto>();
    }


    public class RoomSearchDto: BasedResponseModel
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
