using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IFeatureRoomService
{
    public CustomEntityResult<GetFeatureRoomsResponseDto> GetFeatureRoom();
}
