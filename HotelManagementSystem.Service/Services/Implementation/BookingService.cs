using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingService(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }
        public async Task<CustomEntityResult<CreateBookingResponseModel>> CreaeBooking(CreateBookingRequestModel model)
        {
            try
            {
                #region call repo
                var createBookingRequest = new CreateBookingRequestDto
                {
                    BookingId = Guid.NewGuid().ToString(),
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    Guest_Count = model.Guest_Count,
                    Booking_Status = model.Booking_Status,
                    Deposit_Amount = model.Deposit_Amount,
                    Total_Amount = model.Total_Amount,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    PaymentType = model.PaymentType
                };
                var createBooking = await _bookingRepo.CreateBooking(createBookingRequest);
                if (createBooking.IsError)
                {
                    return CustomEntityResult<CreateBookingResponseModel>.GenerateFailEntityResult(createBooking.Result.RespCode,createBooking.Result.RespDescription);
                };
                #endregion
                var createBookingResponse = new CreateBookingResponseModel();
                return CustomEntityResult<CreateBookingResponseModel>.GenerateSuccessEntityResult(createBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateBookingResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}