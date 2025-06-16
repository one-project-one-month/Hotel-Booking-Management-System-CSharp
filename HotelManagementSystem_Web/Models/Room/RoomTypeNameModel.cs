namespace HotelManagementSystem_Web.Models.Room
{
    public class RoomType
    {
        public string? RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string RoomImg { get; set; } = null!;
        public string RoomImgMimeType { get; set; } = null!;
    }


    public class RoomResponse
    {
        public List<RoomType> RoomList { get; set; } = new();
        public string RespCode { get; set; }
        public string RespDescription { get; set; }
    }

}
