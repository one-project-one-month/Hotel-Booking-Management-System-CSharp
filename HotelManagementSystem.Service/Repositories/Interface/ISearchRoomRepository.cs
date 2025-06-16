using HotelManagementSystem.Data.Dtos.SearchRoom;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface ISearchRoomRepository
    {
        Task<CustomEntityResult<SearchRoomResponseDto>> SearchRoom(SearchRoomRequestDto model);
    }
}