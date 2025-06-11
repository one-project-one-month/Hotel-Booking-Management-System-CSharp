using Azure;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Data.Models.RoomType;
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
        
        var roomlists =  rooms.Result.RoomList.Select(x=> new RoomModel
        {
            RoomNo = x.RoomNo,
            GuestLimit = x.GuestLimit,
            RoomStatus = x.RoomStatus.IsRoomAvailable(),
            IsFeatured = x.IsFeatured,
            RoomType = x.RoomType != null ? new RoomTypeModel
            {
                RoomTypeName = x.RoomType.RoomTypeName,
                RoomImg = x.RoomType.RoomImg != null ? Convert.ToBase64String(x.RoomType.RoomImg) : null,
                RoomImgMimeType = x.RoomType.RoomImgMimeType != null ? x.RoomType.RoomImgMimeType : null,
                Description = x.RoomType.Description,
                Price = x.RoomType.Price,
            } : null, 
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
        RoomResponseModel roomResponse = new RoomResponseModel
        {
            Room = new RoomModel
            {
                RoomNo = room.Result.Room.RoomNo,
                GuestLimit = room.Result.Room.GuestLimit,
                RoomStatus = room.Result.Room.RoomStatus.IsRoomAvailable(),
                IsFeatured = room.Result.Room.IsFeatured,
                RoomType = room.Result.Room.RoomType != null ? new RoomTypeModel
                {
                    RoomTypeName = room.Result.Room.RoomType.RoomTypeName,
                    RoomImg = room.Result.Room.RoomType.RoomImg != null ? Convert.ToBase64String(room.Result.Room.RoomType.RoomImg) : null,
                    RoomImgMimeType = room.Result.Room.RoomType.RoomImgMimeType != null ? room.Result.Room.RoomType.RoomImgMimeType : null,
                    Description = room.Result.Room.RoomType.Description,
                    Price = room.Result.Room.RoomType.Price,
                } : null,
            }
        };
        return CustomEntityResult<RoomResponseModel>.GenerateSuccessEntityResult(roomResponse);
    }
    public async Task<CustomEntityResult<CreateRoomResponseModel>> CreateRoom(CreateRoomRequestModel requestModel)
    {
        try
        {
            #region call repo

            var createRoomRequest = new CreateRoomRequestDto()
            {
                RoomNo = requestModel.RoomNo,
                RoomStatus = requestModel.RoomStatus,
                GuestLimit = requestModel.GuestLimit,
                RoomTypeId = requestModel.RoomTypeId,
            };
            var result = await _roomRepository.CreateRoom(createRoomRequest);
            if (result.IsError)
            {
                return CustomEntityResult<CreateRoomResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            #endregion
            
            
            var createRoomResponse = new CreateRoomResponseModel()
            {
                RoomId = result.Result.RoomId
            };
            return CustomEntityResult<CreateRoomResponseModel>.GenerateSuccessEntityResult(createRoomResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateRoomResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
    public async Task<CustomEntityResult<UpdateRoomResponseModel>> UpdateRoom(Guid id,UpdateRoomRequestModel requestModel)
    {
        try
        {
            var updateRequestDto = new UpdateRoomRequestDto()
            {
                RoomNo = requestModel.RoomNo,
                RoomStatus = requestModel.RoomStatus,
                RoomTypeId = requestModel.RoomTypeId,
                GuestLimit = requestModel.GuestLimit,
                IsFeatured = requestModel.IsFeatured,
            };
            var result = await _roomRepository.UpdateRoom(id, updateRequestDto);
            if (result.IsError)
            {
                return CustomEntityResult<UpdateRoomResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            var response = new UpdateRoomResponseModel()
            {
                RoomNo = result.Result.RoomNo,
                RoomStatus = result.Result.RoomStatus,
                RoomTypeId = result.Result.RoomTypeId,
                GuestLimit = result.Result.GuestLimit,
                IsFeatured = result.Result.IsFeatured,
            };
            return CustomEntityResult<UpdateRoomResponseModel>.GenerateSuccessEntityResult(response);
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