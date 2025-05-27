using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Repositories.Interface;

namespace HotelManagementSystem.Service.Reposities.Implementation;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _hotelDbContext;
    
    public RoomRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }
    
    public async Task<CustomEntityResult<CreateRoomResponseDto>> CreateRoom(CreateRoomRequestDto model)
    {
        try
        {
            var createRoom = new TblRoom()
            {
            };

            // await _hotelDbContext.TblRooms.AddAsync(createRoom);
            // await _hotelDbContext.SaveChangesAsync();
            
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
}