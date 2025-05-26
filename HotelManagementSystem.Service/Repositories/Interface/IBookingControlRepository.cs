using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IBookingControlRepository
{
    public Task<CustomEntityResult<GetBookingsResponseDto>> GetBookingControl();
}
