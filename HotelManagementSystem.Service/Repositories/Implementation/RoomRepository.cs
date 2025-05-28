using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Dtos.RoomType;
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
        var lst = await _hotelDbContext.TblRooms
            .Include(x=>x.RoomType)
            .ThenInclude(x=>x.TblRoomTypeImage)
            .Select(x=>new RoomDto()
            {
                RoomNo = x.RoomNo,
                GuestLimit = x.GuestLimit,
                RoomStatus = x.RoomStatus,
                IsFeatured = x.IsFeatured,
                RoomType = new RoomTypeDto()
                {
                    RoomTypeName = x.RoomType.RoomTypeName,
                    Description = x.RoomType.Description,
                    RoomImg = x.RoomType.TblRoomTypeImage!=null?  x.RoomType.TblRoomTypeImage.RoomImg: null,
                    RoomImgMimeType = x.RoomType.TblRoomTypeImage!=null?  x.RoomType.TblRoomTypeImage.RoomImgMimeType : null,
                    Price = x.RoomType.Price
                }
            }).ToListAsync();

        var responseDto = new RoomListResponseDto()
        {
            RoomList = lst
        };

        return CustomEntityResult<RoomListResponseDto>.GenerateSuccessEntityResult(responseDto);
    }

    public async Task<CustomEntityResult<RoomResponseDto>>GetRoomById(Guid id)
    {
        var room = await _hotelDbContext.TblRooms
            .Include(x=>x.RoomType)
            .ThenInclude(x=>x.TblRoomTypeImage)
            .FirstOrDefaultAsync(x => x.RoomId == id);

        if (room is null) return CustomEntityResult<RoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Not Found");

        RoomResponseDto roomResponseDto = new RoomResponseDto()
        {
            Room = new RoomDto()
            {
                RoomNo = room.RoomNo,
                GuestLimit = room.GuestLimit,
                RoomStatus = room.RoomStatus,
                IsFeatured = room.IsFeatured,
                RoomType = new RoomTypeDto()
                {
                    RoomTypeName = room.RoomType.RoomTypeName,
                    Description = room.RoomType.Description,
                    RoomImg = room.RoomType.TblRoomTypeImage!=null ? room.RoomType.TblRoomTypeImage.RoomImg : null,
                    RoomImgMimeType = room.RoomType.TblRoomTypeImage != null ? room.RoomType.TblRoomTypeImage.RoomImgMimeType : null,
                    Price = room.RoomType.Price
                },
            }
        };
        return CustomEntityResult<RoomResponseDto>.GenerateSuccessEntityResult(roomResponseDto);

    }

    public async Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto requestDto)
    {
        try
        {
            var createRoom = new TblRoom()
            {
                RoomId = Guid.NewGuid(),
                RoomNo = requestDto.RoomNo,
                RoomStatus = requestDto.RoomStatus,
                RoomTypeId = requestDto.RoomTypeId,
                GuestLimit = requestDto.GuestLimit,
                IsFeatured = requestDto.IsFeatured,
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

    public async Task<CustomEntityResult<UpdateRoomResponseDto>>UpdateRoom(Guid id, UpdateRoomRequestDto requestDto)
    {
        try
        {
            var room = await _hotelDbContext.TblRooms.FirstOrDefaultAsync(x => x.RoomId == id);
            if (requestDto is null) return CustomEntityResult<UpdateRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Not Found");

            if (!string.IsNullOrEmpty(requestDto.RoomNo)) room!.RoomNo = requestDto.RoomNo;

            if (!string.IsNullOrEmpty(requestDto.RoomStatus)) room!.RoomStatus = requestDto.RoomStatus;

            if (requestDto.GuestLimit.HasValue) room!.GuestLimit = requestDto.GuestLimit;

            if (requestDto.RoomTypeId.HasValue && requestDto.RoomTypeId != Guid.Empty) room!.RoomTypeId = requestDto.RoomTypeId.Value;

            if (requestDto.IsFeatured.HasValue) room!.IsFeatured = requestDto.IsFeatured.Value;

            room!.UpdatedAt = EntityConstantsHelper.GetMyanmarLocalTime();

            await _hotelDbContext.SaveChangesAsync();
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
            await _hotelDbContext.SaveChangesAsync();
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