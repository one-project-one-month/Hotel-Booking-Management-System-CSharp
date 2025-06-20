using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.SearchRoom
{
    public class SearchRoomDto
    {
    }
    public class SearchRoomRequestDto
    {
        public Guid? RoomTypeId { get; set; }
     
        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public int? GuestLimit { get; set; }
    }

    public class SearchRoomResponseDto : BasedResponseModel
    {
        public List<RoomSearchDto> Rooms { get; set; } = new List<RoomSearchDto>();
    }


    public class RoomSearchDto: BasedResponseModel
    {
        public Guid RoomId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }
        public string? RoomNumber { get; set; }
        public string? Description { get; set; }
        public string ImgUrl { get; set; }

        public string ImgMimeType { get; set; } = "image/png"; // Default to PNG if not specified
    }

    public class RoomQueryResult
    {
        public Guid RoomId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }
        public string? RoomNumber { get; set; }
        public string? Description { get; set; }

        public byte[] ImgUrl { get; set; }
        public string RoomImgMimeType { get; set; }
    }

}
