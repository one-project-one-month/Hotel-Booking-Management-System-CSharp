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

namespace HotelManagementSystem.Service.Services.Implementation;

public class BookingControlService : IBookingControlService
{
    private readonly IBookingControlRepository _bookingControlRepository;

    public BookingControlService(IBookingControlRepository bookingControlRepository)
    {
        _bookingControlRepository = bookingControlRepository;
    }

    public async Task<CustomEntityResult<GetBookingsResponseDto>> GetBookingControl()
    {
        var result = await _bookingControlRepository.GetBookingControl();
        if (result.IsError)
        {
            return CustomEntityResult<GetBookingsResponseDto>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }
        return result;

    }
}
