using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Guest;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface IGuestRepositoery
    {
        Task<CustomEntityResult<CreateGuestResponseDto>> CreateGuest(CreateGuestRequestDto model);
    }
}