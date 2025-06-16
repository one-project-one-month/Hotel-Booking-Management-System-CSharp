using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Service.Services.Interface;
using HotelManagementSystem.Data.Models.BookingControl;
using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Implementation;

public class BookingControlService : IBookingControlService
{
    private readonly IBookingControlRepository _bookingControlRepository;

    public BookingControlService(IBookingControlRepository bookingControlRepository)
    {
        _bookingControlRepository = bookingControlRepository;
    }

    public async Task<CustomEntityResult<GetBookingsResponseModel>> GetBookings()
    {
        var result = await _bookingControlRepository.GetBookings();
        if (result.IsError)
        {
            return CustomEntityResult<GetBookingsResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }
        var getBookingResponse = new GetBookingsResponseModel()
        {
            Bookings = result.Result.Bookings.Select(b => new GetBookingResponseModel
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                GuestId = b.GuestId,
                GuestCount = b.GuestCount,
                CheckInTime = b.CheckInTime,
                CheckOutTime = b.CheckOutTime,
                DepositAmount = b.DepositAmount,
                BookingStatus = b.BookingStatus,
                TotalAmount = b.TotalAmount,
                CreatedAt = b.CreatedAt,
                PaymentType = b.PaymentType,
                GuestNrc = b.GuestNrc,
                GuestPhoneNo = b.GuestPhoneNo,
                UserName = b.UserName,
                GuestName = b.GuestName,
                RoomNo = b.RoomNo
            }).ToList()
        };

        return CustomEntityResult<GetBookingsResponseModel>.GenerateSuccessEntityResult(getBookingResponse);
    }

    public async Task<CustomEntityResult<DeleteBookingResponseModel>> DeleteBooking(DeleteBookingRequestModel Booking)
    {
        var bookingRequestDto = new DeleteBookingRequestDto()
        {
            BookingId = Booking.BookingId
        };
        var result = await _bookingControlRepository.DeleteBooking(bookingRequestDto);
        if (result.IsError)
        {
            return CustomEntityResult<DeleteBookingResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }
        var returnModel = new DeleteBookingResponseModel();
        return CustomEntityResult<DeleteBookingResponseModel>.GenerateSuccessEntityResult(returnModel);
    }

    public async Task<CustomEntityResult<UpdateBookingResponseModel>> UpdateBooking(UpdateBookingRequestModel requestModel)
    {
        var bookingRequestDto = new UpdateBookingRequestDto()
        {
            BookingId = requestModel.BookingId,
            UserId = requestModel.UserId,
            GuestId = requestModel.GuestId,
            GuestCount = requestModel.GuestCount,
            CheckInTime = requestModel.CheckInTime,
            CheckOutTime = requestModel.CheckOutTime,
            DepositAmount = requestModel.DepositAmount,
            BookingStatus = requestModel.BookingStatus,
            TotalAmount = requestModel.TotalAmount,
            CreatedAt = requestModel.CreatedAt,
            PaymentType = requestModel.PaymentType
        };

        bookingRequestDto.Rooms = requestModel.Rooms;

        var result = await _bookingControlRepository.UpdateBooking(bookingRequestDto);
        if (result.IsError)
        {
            return CustomEntityResult<UpdateBookingResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }

        var returnModel = new UpdateBookingResponseModel();
        return CustomEntityResult<UpdateBookingResponseModel>.GenerateSuccessEntityResult(returnModel);
    }

    public async Task<CustomEntityResult<CreateBookingByAdminResponseModel>> CreateBookingByAdmin(CreateBookingByAdminRequestModel model)
    {
        try
        {
            var createBookingRequest = new CreateBookingByAdminRequestDto
            {
                UserId = model.UserId,
                Name = model.Name,
                Rooms = model.Rooms,
                GuestCount = model.GuestCount,
                BookingStatus = model.BookingStatus,
                DepositAmount = model.DepositAmount,
                TotalAmount = model.TotalAmount,
                CheckInTime = model.CheckInTime,
                CheckOutTime = model.CheckOutTime,
                PhoneNo = model.PhoneNo,
                Nrc = model.Nrc,
                PaymentType = model.PaymentType
            };
            var createBooking = await _bookingControlRepository.CreateBookingByAdmin(createBookingRequest);
            if (createBooking.IsError)
            {
                return CustomEntityResult<CreateBookingByAdminResponseModel>.GenerateFailEntityResult(createBooking.Result.RespCode, createBooking.Result.RespDescription);
            }
            var createBookingResponse = new CreateBookingByAdminResponseModel()
            {
                BookingId = createBooking.Result.BookingId,
                GuestId = createBooking.Result.GuestId,
            };
            return CustomEntityResult<CreateBookingByAdminResponseModel>.GenerateSuccessEntityResult(createBookingResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateBookingByAdminResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}
