using System.Text.Json.Serialization;

namespace HotelManagementSystem_Web.Models.Room
{
    public class SearchRoomResponseModel
    {

        public class Rooms
        {
            public Guid RoomId { get; set; }
            public Guid RoomTypeId { get; set; }
            public string? RoomTypeName { get; set; }
            public decimal? Price { get; set; }
            public int? GuestLimit { get; set; }
            public string? RoomNumber { get; set; }
            public string? Description { get; set; }
            public string? ImgUrl { get; set; }
        }


        public class SearchRoomResponse
        {
            [JsonPropertyName("rooms")]
            public List<Rooms> SearchedRoomList { get; set; } = new();

            [JsonPropertyName("respCode")]
            public string ResponseCode { get; set; }

            [JsonPropertyName("respDescription")]
            public string Description { get; set; }


        }
    }
}
