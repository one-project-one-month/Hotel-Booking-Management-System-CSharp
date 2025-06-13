namespace HotelManagementSystem.Data.Models.SearchRoom
{
    public class SearchRoomResponseModel : BasedResponseModel
    {
        public List<RoomModel> Rooms { get; set; } = new();
    }

    public class RoomModel
    {
        public Guid RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }
        public string? RoomNumber { get; set; }
        public string? Description { get; set; }
        public byte[]? ImgUrl { get; set; }
    }
}
