using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Room;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IRoomService
{
    public Task<CustomEntityResult<CreateRoomResponseModel>> CreateRoom(CreateRoomRequestModel model);
}