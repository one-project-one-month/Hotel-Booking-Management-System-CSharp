using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        public Task<CustomEntityResult<CreateBookingResponseModel>> CreaeBooking(CreateBookingRequestModel model);
    }
}
