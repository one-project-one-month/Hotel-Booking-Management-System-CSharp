using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagementSystem.Service.Repositories.Implementation;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Service.Services.Interface;
using HotelManagementSystem.Data.Models.BookingControl;
using HotelManagementSystem.Data.Entities;

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
                CheckIn_Time = b.CheckIn_Time,
                CheckOut_Time = b.CheckOut_Time,
                Deposit_Amount = b.Deposit_Amount,
                BookingStatus = b.BookingStatus,
                TotalAmount = b.TotalAmount,
                CreatedAt = b.CreatedAt,
                PaymentType = b.PaymentType,
                GuestNrc = b.GuestNrc,
                GuestPhoneNo = b.GuestPhoneNo,
                UserName = b.UserName,
                RoomNo = b.RoomNo
            }).ToList()
        };

        return CustomEntityResult<GetBookingsResponseModel>.GenerateSuccessEntityResult(getBookingResponse);
    }

    public async Task<CustomEntityResult<GetBookingsResponseModel>> DeleteBooking(string BookingId)
    {
        var result = await _bookingControlRepository.DeleteBooking(BookingId);
        if (result.IsError)
        {
            return CustomEntityResult<GetBookingsResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }
        var returnModel = new GetBookingsResponseModel();
        return CustomEntityResult<GetBookingsResponseModel>.GenerateSuccessEntityResult(returnModel);
    }

    public async Task<CustomEntityResult<UpdateBookingResponseModel>> UpdateBooking(string BookingId, UpdateBookingRequestModel requestModel)
    {
        var bookingRequestDto = new UpdateBookingRequestDto()
        {
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
        var roomBookingId = await _bookingControlRepository.UpdateBooking(BookingId, bookingRequestDto);

        var result = await _bookingControlRepository.UpdateBooking(BookingId, bookingRequestDto);
        if (result.IsError)
        {
            return CustomEntityResult<UpdateBookingResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }

        var returnModel = new UpdateBookingResponseModel();
        return CustomEntityResult<UpdateBookingResponseModel>.GenerateSuccessEntityResult(returnModel);
    }
}
