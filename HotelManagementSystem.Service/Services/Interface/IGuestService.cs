using HotelManagementSystem.Data.Models.Guest;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IGuestService
    {
        Task<CustomEntityResult<CreateGuestResponseModel>> CreateGuest(CreateGuestRequestModel model);
    }
}