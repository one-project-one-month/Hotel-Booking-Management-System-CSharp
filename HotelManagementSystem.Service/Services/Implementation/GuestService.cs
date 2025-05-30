using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Guest;
using HotelManagementSystem.Data.Models.Guest;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepo;
        public GuestService(IGuestRepository guestRepo)
        {
            _guestRepo = guestRepo;
        }
        public async Task<CustomEntityResult<CreateGuestResponseModel>> CreateGuest(CreateGuestRequestModel model)
        {
            try
            {
                var createGuestRequest = new CreateGuestRequestDto
                {
                    UserId = model.UserId,
                    Nrc = model.Nrc,
                    PhoneNo = model.PhoneNo
                };
                var createGuestResponse = await _guestRepo.CreateGuest(createGuestRequest);
                if (createGuestResponse.IsError)
                {
                    return CustomEntityResult<CreateGuestResponseModel>.GenerateFailEntityResult(createGuestResponse.Result.RespCode, createGuestResponse.Result.RespDescription);
                }
                var createGuestResponseModel = new CreateGuestResponseModel
                {
                    RespCode = createGuestResponse.Result.RespCode,
                    RespDescription = createGuestResponse.Result.RespDescription
                };
                return CustomEntityResult<CreateGuestResponseModel>.GenerateSuccessEntityResult(createGuestResponseModel);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateGuestResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}
