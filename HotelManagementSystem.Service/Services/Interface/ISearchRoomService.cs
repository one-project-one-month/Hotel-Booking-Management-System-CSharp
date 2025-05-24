using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.SearchRoom;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface ISearchRoomService
    {
        Task<CustomEntityResult<SearchRoomResponseModel>> SearchRoom(SearchRoomRequestModel model);
    }
}