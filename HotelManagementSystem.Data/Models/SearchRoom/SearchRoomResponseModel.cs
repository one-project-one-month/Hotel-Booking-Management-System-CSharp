namespace HotelManagementSystem.Data.Models.SearchRoom
{
    public class SearchRoomResponseModel : BasedResponseModel
    {
        public List<RoomModel> Rooms { get; set; } = new();
    }

    public class RoomModel
    {
        public Guid RoomId { get; set; }
        public string? RoomType { get; set; }
        public decimal? Price { get; set; }
        public int? GuestLimit { get; set; }
        public string? RoomNumber { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
    }
}
