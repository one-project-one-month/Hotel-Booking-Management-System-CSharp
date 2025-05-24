using Azure;
using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;
using System.Security.AccessControl;

namespace HotelManagementSystem.Service.Services.Implementation;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<CustomEntityResult<RoomListResponseModel>> GetRooms()
    {
        var rooms = await _roomRepository.GetRooms();
        
        var roomlists =  rooms.Result.RoomList.Select(x=> new RoomResponseModel()
        {
            RoomNo = x.RoomNo,
            GuestLimit = x.GuestLimit,
            RoomStatus = x.RoomStatus,
            RoomTypeId = x.RoomTypeId,
        }).ToList();

        RoomListResponseModel roomList = new RoomListResponseModel()
        {
            RoomList = roomlists
        };
        return CustomEntityResult<RoomListResponseModel>.GenerateSuccessEntityResult(roomList);
    }

    public async Task<CustomEntityResult<RoomResponseModel>>GetRoomById(Guid id)
    {
        var room = await _roomRepository.GetRoomById(id);
        if(room.IsError)
        {
            return CustomEntityResult<RoomResponseModel>.GenerateFailEntityResult(room.Result.RespCode, room.Result.RespDescription);
        }
        RoomResponseModel roomResponse = new RoomResponseModel()
        {
            RoomNo = room.Result.RoomNo,
            RoomTypeId=room.Result.RoomTypeId,
            RoomStatus = room.Result.RoomStatus,
            GuestLimit=room.Result.GuestLimit,
        };
        return CustomEntityResult<RoomResponseModel>.GenerateSuccessEntityResult(roomResponse);
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
                GuestLimit = model.GuestLimit,
                RoomTypeId = model.RoomTypeId,
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
    public async Task<CustomEntityResult<UpdateRoomResponseModel>> UpdateRoom(Guid id,UpdateRoomRequestModel model)
    {
        try
        {
            var updateRequestDto = new UpdateRoomRequestDto()
            {
                RoomNo = model.RoomNo,
                RoomStatus = model.RoomStatus,
                RoomTypeId = model.RoomTypeId,
                GuestLimit = model.GuestLimit,
                IsFeatured = model.IsFeatured,
            };
            var response = await _roomRepository.UpdateRoom(id, updateRequestDto);
            if (response.IsError)
            {
                return CustomEntityResult<UpdateRoomResponseModel>.GenerateFailEntityResult(response.Result.RespCode, response.Result.RespDescription);
            }

            var updateResonseModel = new UpdateRoomResponseModel()
            {
                RoomNo = response.Result.RoomNo,
                RoomStatus = response.Result.RoomStatus,
                RoomTypeId = response.Result.RoomTypeId,
                GuestLimit = response.Result.GuestLimit,
                IsFeatured = response.Result.IsFeatured,
            };
            return CustomEntityResult<UpdateRoomResponseModel>.GenerateSuccessEntityResult(updateResonseModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<UpdateRoomResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> DeleteRoom(Guid id)
    {
        try
        {
            var response = await _roomRepository.DeteRoom(id);
            if (response.IsError)
            {
                return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(response.Result.RespCode, response.Result.RespDescription);
            }
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(response.Result);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }

    }
}