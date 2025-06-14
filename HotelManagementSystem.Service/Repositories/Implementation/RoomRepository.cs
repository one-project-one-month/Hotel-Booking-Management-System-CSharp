using HotelManagementSystem.Data.Models.Room;

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
        try
        {
            var rooms = await _hotelDbContext.TblRooms.ToListAsync();
            if (rooms is null) return CustomEntityResult<RoomListResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "No Rooms Found");
            var room = new RoomListResponseDto()
            {
                RoomList = rooms.Select(x => new RoomDto()
                {
                    RoomId = x.RoomId,
                    RoomNo = x.RoomNo,
                    GuestLimit = x.GuestLimit,
                    RoomStatus = x.RoomStatus,
                    IsFeatured = x.IsFeatured,
                    RoomTypeId = x.RoomTypeId,
                }).ToList()
            };
            return CustomEntityResult<RoomListResponseDto>.GenerateSuccessEntityResult(room);
        }

        catch (Exception ex)
        {
            return CustomEntityResult<RoomListResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }

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
                RoomId = room.RoomId,
                RoomNo = room.RoomNo,
                GuestLimit = room.GuestLimit,
                RoomStatus = room.RoomStatus,
                IsFeatured = room.IsFeatured,
                RoomTypeId = room.RoomTypeId,
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