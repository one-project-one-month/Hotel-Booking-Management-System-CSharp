namespace HotelManagementSystem.Service.Services.Interface;

public interface IFeatureRoomService
{
    public CustomEntityResult<GetFeatureRoomsResponseDto> GetFeatureRoom();
}
