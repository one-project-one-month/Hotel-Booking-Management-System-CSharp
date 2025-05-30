using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IRoomTypeRepository
{
    public Task<CustomEntityResult<CreateRoomTypeResponseDto>> CreateRoomType(CreateRoomTypeRequestDto model);
    public Task<CustomEntityResult<RoomTypeListResponseDto>> GetRoomTypes();
    public Task<CustomEntityResult<RoomTypeResponseDto>> GetRoomTypeById(Guid id);
    public Task<CustomEntityResult<UpdateRoomTypeResponseDto>> UpdateRoomType(Guid id, UpdateRoomTypeRequestDto requestModel);
    public Task<CustomEntityResult<BasedResponseModel>> DeleteRoomType(Guid id);
}