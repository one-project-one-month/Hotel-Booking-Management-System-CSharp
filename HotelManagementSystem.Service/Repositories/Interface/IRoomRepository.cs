namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IRoomRepository
{
    public Task<CustomEntityResult<RoomListResponseDto>> GetRooms();
    public Task<CustomEntityResult<RoomResponseDto>> GetRoomById(Guid id);
    public Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto model);
    public Task<CustomEntityResult<UpdateRoomResponseDto>> UpdateRoom(UpdateRoomRequestDto model);
    public Task<CustomEntityResult<BasedResponseModel>> DeteRoom(Guid id);
}