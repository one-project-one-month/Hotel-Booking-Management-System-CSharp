using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Service.Exceptions;
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
        public async Task<CustomEntityResult<CreateBookingResponseModel>> CreateBookingByUser(CreateBookingRequestModel model)
        {
            try
            {
                #region call repo

                var createBookingRequest = new CreateBookingRequestDto
                {
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    Guest_Count = model.Guest_Count,
                    Booking_Status = model.Booking_Status,
                    Deposit_Amount = model.Deposit_Amount,
                    Total_Amount = model.Total_Amount,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    PaymentType = model.PaymentType,
                    Rooms = model.Rooms,
                };
                var createBooking = await _bookingRepo.CreateBookingByUser(createBookingRequest);
                if (createBooking.IsError)
                {
                    return CustomEntityResult<CreateBookingResponseModel>.GenerateFailEntityResult(createBooking.Result.RespCode,createBooking.Result.RespDescription);
                };
                #endregion
                var createBookingResponse = new CreateBookingResponseModel()
                {
                    BookingId = createBooking.Result.BookingId
                };
                return CustomEntityResult<CreateBookingResponseModel>.GenerateSuccessEntityResult(createBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateBookingResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }

        public async Task<CustomEntityResult<GetBookingByIdResponseModel>> GetBookingById(GetBookingByIdRequestModel bookingId)
        {
            try
            {
                var getBookingByIdRequest = new GetBookingByIdRequestDto
                {
                    BookingId = bookingId.BookingId
                };
                var getBookingById = await _bookingRepo.GetBookingById(getBookingByIdRequest);
                if (getBookingById.IsError)
                {
                    return CustomEntityResult<GetBookingByIdResponseModel>.GenerateFailEntityResult(getBookingById.Result.RespCode, getBookingById.Result.RespDescription);
                }
                var GetBookingByIdResponse = new GetBookingByIdResponseModel()
                {
                    BookingId = getBookingById.Result.BookingId,
                    UserId = getBookingById.Result.UserId,
                    GuestId = getBookingById.Result.GuestId,
                    Guest_Count = getBookingById.Result.Guest_Count,
                    Booking_Status = getBookingById.Result.Booking_Status,
                    Deposit_Amount = getBookingById.Result.Deposit_Amount,
                    Total_Amount = getBookingById.Result.Total_Amount,
                    CheckInDate = getBookingById.Result.CheckInDate,
                    CheckOutDate = getBookingById.Result.CheckOutDate,
                    PaymentType = getBookingById.Result.PaymentType,
                    RoomNumbers = getBookingById.Result.RoomNumbers,
                };
                return CustomEntityResult<GetBookingByIdResponseModel>.GenerateSuccessEntityResult(GetBookingByIdResponse);
            }
            catch(Exception ex)
            {
                return CustomEntityResult<GetBookingByIdResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }

        public async Task<CustomEntityResult<ListBookingResponseModel>> GetAllBookingByUserId(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    throw new UserNotFoundException("User not found. Please login again.");
                }
                if (!Guid.TryParse(Id, out var userId))
                {
                    throw new InvalidUserIdException("Invalid user ID format.");
                }
                var UserID = new ListBookingRequestByUserDto
                {
                    UserId = userId
                };
                var bookingListResult = await _bookingRepo.GetAllBookingByUserId(UserID);
                if (bookingListResult.IsError)
                {
                    return CustomEntityResult<ListBookingResponseModel>.GenerateFailEntityResult(bookingListResult.Result.RespCode, bookingListResult.Result.RespDescription);
                }
                var listBookingResponse = new ListBookingResponseModel
                {
                    Booking = bookingListResult.Result.Bookings?.Select(b => new ListBookingModel
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        GuestId = b.GuestId,
                        Guest_Count = b.Guest_Count,
                        Booking_Status = b.Booking_Status,
                        Deposit_Amount = b.Deposit_Amount,
                        Total_Amount = b.Total_Amount,
                        CheckInDate = b.CheckInDate,
                        CheckOutDate = b.CheckOutDate,
                        PaymentType = b.PaymentType,
                        CreatedAt = b.CreatedAt,
                        RoomNumbers = b.RoomNumbers,
                    }).ToList()
                };
                if (listBookingResponse.Booking == null || !listBookingResponse.Booking.Any())
                {
                    return CustomEntityResult<ListBookingResponseModel>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                        "No bookings found for this user");
                }
                return CustomEntityResult<ListBookingResponseModel>.GenerateSuccessEntityResult(listBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<ListBookingResponseModel>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }

        public async Task<CustomEntityResult<CancelResponseModel>> CancelBookingByUser(CancelRequestModel model)
        {
            try
            {
                var bookingId = new CancelRequestDto
                {
                    BookingId = model.BookingId,
                };
                var result = await _bookingRepo.CancelBookingByUser(bookingId);
                if (result.IsError)
                {
                    return CustomEntityResult<CancelResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
                }
                var response = new CancelResponseModel
                {
                    RespCode = result.Result.RespCode,
                    RespDescription = result.Result.RespDescription,
                };
                return CustomEntityResult<CancelResponseModel>.GenerateSuccessEntityResult(response);
            }
            catch(Exception ex)
            {
                return CustomEntityResult<CancelResponseModel>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }
    }
}