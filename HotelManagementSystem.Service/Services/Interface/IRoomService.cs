using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IRoomService
{
    public Task<CustomEntityResult<RoomListResponseModel>> GetRooms();
    public Task<CustomEntityResult<RoomResponseModel>> GetRoomById(Guid id);

    public Task<CustomEntityResult<CreateRoomResponseModel>> CreateRoom(CreateRoomRequestModel model);
    public Task<CustomEntityResult<UpdateRoomResponseModel>> UpdateRoom(Guid id, UpdateRoomRequestModel model);
    public Task<CustomEntityResult<BasedResponseModel>> DeleteRoom(Guid id);

}