using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Reposities.Implementation;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _hotelDbContext;
    
    public RoomRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }
    
    public async Task<CustomEntityResult<RoomListResponseDto>>GetRooms()
    {
        var lst = await _hotelDbContext.TblRooms.Select(x=> new RoomResponseDto()
        {
            RoomNo = x.RoomNo,
            RoomTypeId = x.RoomTypeId,
            RoomStatus = x.RoomStatus,
            GuestLimit = x.GuestLimit,
        }).ToListAsync();
        RoomListResponseDto roomList = new RoomListResponseDto()
        {
            RoomList = lst
        };
        return CustomEntityResult<RoomListResponseDto>.GenerateSuccessEntityResult(roomList);
    }

    public async Task<CustomEntityResult<RoomResponseDto>>GetRoomById(Guid id)
    {
        var model = await _hotelDbContext.TblRooms.FirstOrDefaultAsync(x => x.RoomId == id);
        if (model is null) return CustomEntityResult<RoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Not Found");
        RoomResponseDto roomResponseDto = new RoomResponseDto()
        {
            RoomNo = model.RoomNo,
            GuestLimit = model.GuestLimit,
            RoomStatus= model.RoomStatus,
            RoomTypeId = model.RoomTypeId,
        };
        return CustomEntityResult<RoomResponseDto>.GenerateSuccessEntityResult(roomResponseDto);

    }

    public async Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto model)
    {
        try
        {
            var createRoom = new TblRoom()
            {
                RoomId = Guid.NewGuid(),
                RoomNo = model.RoomNo,
                RoomStatus = model.RoomStatus,
                RoomTypeId = model.RoomTypeId,
                GuestLimit = model.GuestLimit,
                IsFeatured = model.IsFeatured,
                CreatedAt = EntityConstantsHelper.GetMyanmarLocalTime()
            };

            await _hotelDbContext.TblRooms.AddAsync(createRoom);
            await _hotelDbContext.SaveChangesAsync();

            var createRoomResponse = new CreateRoomResponseDto()
            {
                RoomId = createRoom.RoomId
            };
            return CustomEntityResult<CreateRoomResponseDto>.GenerateSuccessEntityResult(createRoomResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<UpdateRoomResponseDto>>UpdateRoom(Guid id, UpdateRoomRequestDto model)
    {
        try
        {
            var room = await _hotelDbContext.TblRooms.FirstOrDefaultAsync(x => x.RoomId == id);
            if (model is null) return CustomEntityResult<UpdateRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Not Found");

            if (!string.IsNullOrEmpty(model.RoomNo)) room!.RoomNo = model.RoomNo;
            if (!string.IsNullOrEmpty(model.RoomStatus)) room!.RoomStatus = model.RoomStatus;
            if (model.GuestLimit.HasValue) room!.GuestLimit = model.GuestLimit;
            if (model.RoomTypeId.HasValue && model.RoomTypeId != Guid.Empty) room!.RoomTypeId = model.RoomTypeId.Value;
            if (model.IsFeatured.HasValue) room!.IsFeatured = model.IsFeatured.Value;
            room!.UpdatedAt = EntityConstantsHelper.GetMyanmarLocalTime();

            int result = await _hotelDbContext.SaveChangesAsync();
            var responseDto = new UpdateRoomResponseDto()
            {
                RoomNo = room.RoomNo,
                RoomStatus = room.RoomStatus,
                RoomTypeId = room.RoomTypeId,
                GuestLimit = room.GuestLimit,
                IsFeatured = room.IsFeatured,
            };
            return CustomEntityResult<UpdateRoomResponseDto>.GenerateSuccessEntityResult(responseDto);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<UpdateRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> DeteRoom(Guid id)
    {
        try
        {
            var room = await _hotelDbContext.TblRooms.FirstOrDefaultAsync(x => x.RoomId == id);
            if (room is null) return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Not Found");
            _hotelDbContext.TblRooms.Remove(room);
            int result = await _hotelDbContext.SaveChangesAsync();
            var responseModel = new BasedResponseModel()
            {
                RespCode = "200",
                RespDescription = "Delete Success",
            };
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(responseModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}