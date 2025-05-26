using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IBookingControlService
{
    public Task<CustomEntityResult<GetBookingsResponseDto>> GetBookingControl();
}
