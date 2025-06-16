using HotelManagementSystem.Data.Dtos.Guest;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface IGuestRepository
    {
        Task<CustomEntityResult<CreateGuestResponseDto>> CreateGuest(CreateGuestRequestDto model);
        Task<CustomEntityResult<GetAllGuestListResponseDto>> GetAllGeuestList();
        Task<CustomEntityResult<GetGuestByIdResponseDto>> GetGuestById(GetGuestByIdRequestDto dto);
    }
}