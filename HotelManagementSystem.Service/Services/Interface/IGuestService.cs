using HotelManagementSystem.Data.Models.Guest;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IGuestService
    {
        Task<CustomEntityResult<CreateGuestResponseModel>> CreateGuest(CreateGuestRequestModel model);
        Task<CustomEntityResult<GetAllGuestListResponseModel>> GetAllGuestList();
        Task<CustomEntityResult<GetGuestByIdResponseModel>> GetGuestById(GetGuestByIdRequestModel model);
    }
}