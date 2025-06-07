using HotelManagementSystem.Data.Dtos.Guest;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Data.Models.Guest;
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

        public async Task<CustomEntityResult<GetAllGuestListResponseModel>> GetAllGuestList()
        {
            try
            {
                var bookingListResult = await _guestRepo.GetAllGeuestList();

                if (bookingListResult.IsError)
                {
                    return CustomEntityResult<GetAllGuestListResponseModel>.GenerateFailEntityResult(bookingListResult.Result.RespCode, bookingListResult.Result.RespDescription);
                }

                var listGuestResponse = new GetAllGuestListResponseModel
                {
                    Guests = bookingListResult.Result.Guests!.Select(g => new GetAllGuestListModel
                    {
                        UserId = g.UserId,
                        GuestId = g.GuestId,
                        Name = g.Name,
                        Nrc = g.Nrc,
                        PhoneNo = g.PhoneNo,
                        Email = g.Email,
                        CreatedAt = g.CreatedAt,
                    }).ToList()
                };

                if (listGuestResponse.Guests == null || !listGuestResponse.Guests.Any())
                {
                    return CustomEntityResult<GetAllGuestListResponseModel>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                        "No Guest found");
                }

                return CustomEntityResult<GetAllGuestListResponseModel>.GenerateSuccessEntityResult(listGuestResponse);
            }
            catch(Exception ex)
            {
                return CustomEntityResult<GetAllGuestListResponseModel>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }

        public async Task<CustomEntityResult<GetGuestByIdResponseModel>> GetGuestById(GetGuestByIdRequestModel model)
        {
            try
            {
                var getGuestByIdRequestDto = new GetGuestByIdRequestDto
                {
                    GuestId = model.GuestId
                };
                var getGuestByIdResponse = await _guestRepo.GetGuestById(getGuestByIdRequestDto);
                if (getGuestByIdResponse.IsError)
                {
                    return CustomEntityResult<GetGuestByIdResponseModel>.GenerateFailEntityResult(getGuestByIdResponse.Result.RespCode, getGuestByIdResponse.Result.RespDescription);
                }
                var getGuestByIdResponseModel = new GetGuestByIdResponseModel
                {
                    UserId = getGuestByIdResponse.Result.UserId,
                    GuestId = getGuestByIdResponse.Result.GuestId,
                    Name = getGuestByIdResponse.Result.Name,
                    Nrc = getGuestByIdResponse.Result.Nrc,
                    PhoneNo = getGuestByIdResponse.Result.PhoneNo,
                    Email = getGuestByIdResponse.Result.Email,
                    CreatedAt = getGuestByIdResponse.Result.CreatedAt
                };
                return CustomEntityResult<GetGuestByIdResponseModel>.GenerateSuccessEntityResult(getGuestByIdResponseModel);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<GetGuestByIdResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }
    }
}
