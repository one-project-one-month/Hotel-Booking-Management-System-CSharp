using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        public Task<CustomEntityResult<CreateBookingResponseModel>> CreateBooking(CreateBookingRequestModel model);
        public Task<CustomEntityResult<GetBookingByIdResponseModel>> GetBookingById(GetBookingByIdRequestModel bookingId);
        public Task<CustomEntityResult<ListBookingResponseModel>> BookingList();
    }
}
