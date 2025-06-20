using static HotelManagementSystem_Web.Models.Room.SearchRoomResponseModel;

namespace HotelManagementSystem_Web.Models.Room
{
    public class SearchRoomState
    {
        public SearchRoomRequestModel SharedStateRequestModel { get; set; } = new();


        public SearchRoomResponse SharedStateResponseModel { get; set; } = new();


        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
