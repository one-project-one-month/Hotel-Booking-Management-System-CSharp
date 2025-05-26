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

namespace HotelManagementSystem.Service.Services.Implementation;

public class BookingControlService : IBookingControlService
{
    private readonly IBookingControlRepository _bookingControlRepository;

    public BookingControlService(IBookingControlRepository bookingControlRepository)
    {
        _bookingControlRepository = bookingControlRepository;
    }

    public async Task<CustomEntityResult<GetBookingsResponseModel>> GetBookingControl()
    {
        var result = await _bookingControlRepository.GetBookingControl();
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
                PaymentType = b.PaymentType
            }).ToList()
        };

        return CustomEntityResult<GetBookingsResponseModel>.GenerateSuccessEntityResult(getBookingResponse); ;

    }
}
