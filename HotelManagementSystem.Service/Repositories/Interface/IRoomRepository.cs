using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IRoomRepository
{
    public Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto model);
}