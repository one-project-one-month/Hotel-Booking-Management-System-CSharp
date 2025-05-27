using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        public Task<CustomEntityResult<CreateBookingResponseModel>> CreateBookingByUser(string userId, CreateBookingRequestModel model);
        public Task<CustomEntityResult<GetBookingByIdResponseModel>> GetBookingById(GetBookingByIdRequestModel bookingId);
        public Task<CustomEntityResult<ListBookingResponseModel>> GetAllBookingByUserId(string Id);
        public Task<CustomEntityResult<ListBookingResponseModel>> GetAllBookingList();
    }
}
