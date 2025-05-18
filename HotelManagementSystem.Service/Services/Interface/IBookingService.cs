using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        public Task<CustomEntityResult<CreateBookingResponseModel>> CreaeBooking(CreateBookingRequestModel model);
    }
}
