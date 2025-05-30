using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.RoomType;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IRoomTypeService
{
    public Task<CustomEntityResult<CreateRoomTypeResponseModel>> CreateRoomType(CreateRoomTypeRequestModel model);
    public Task<CustomEntityResult<RoomTypeListResponseModel>> GetRoomTypes();
    public Task<CustomEntityResult<RoomTypeResponseModel>> GetRoomTypeById(Guid id);
    public Task<CustomEntityResult<UpdateRoomTypeResponseModel>> UpdateRoomType(Guid id, UpdateRoomTypeRequestModel requestModel);
    public Task<CustomEntityResult<BasedResponseModel>> DeleteRoomType(Guid id);

}