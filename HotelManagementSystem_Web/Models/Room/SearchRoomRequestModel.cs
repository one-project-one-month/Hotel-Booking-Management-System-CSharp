namespace HotelManagementSystem_Web.Models.Room
{
    public class SearchRoomRequestModel
    {
        public Guid? RoomTypeId { get; set; }

        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public int? GuestLimit { get; set; } = 1;

    }
}
