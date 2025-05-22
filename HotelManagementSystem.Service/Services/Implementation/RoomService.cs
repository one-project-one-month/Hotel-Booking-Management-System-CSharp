using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<CustomEntityResult<CreateRoomResponseModel>> CreateRoom(CreateRoomRequestModel model)
    {
        try
        {
            #region call repo

            var createRoomRequest = new CreateRoomRequestDto()
            {
                RoomNo = model.RoomNo,
                RoomStatus = model.RoomStatus,
                ImgUrl = model.ImgUrl,
                GuestLimit = model.GuestLimit
            };
            var createRoom = await _roomRepository.CreateRoom(createRoomRequest);
            if (createRoom.IsError)
            {
                return CustomEntityResult<CreateRoomResponseModel>.GenerateFailEntityResult(createRoom.Result.RespCode, createRoom.Result.RespDescription);
            }

            #endregion
            
            
            var createRoomResponse = new CreateRoomResponseModel()
            {
                RoomId = createRoom.Result.RoomId
            };
            return CustomEntityResult<CreateRoomResponseModel>.GenerateSuccessEntityResult(createRoomResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateRoomResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
}