using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IRoomRepository
{
    public Task<CustomEntityResult<RoomListResponseDto>> GetRooms();
    public Task<CustomEntityResult<RoomResponseDto>> GetRoomById(Guid id);
    public Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto model);
    public Task<CustomEntityResult<UpdateRoomResponseDto>> UpdateRoom(Guid id, UpdateRoomRequestDto model);
    public Task<CustomEntityResult<BasedResponseModel>> DeteRoom(Guid id);
}