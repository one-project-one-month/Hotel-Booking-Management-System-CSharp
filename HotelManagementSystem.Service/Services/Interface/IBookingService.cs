using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        public Task<CustomEntityResult<CreateBookingResponseModel>> CreateBookingByUser(CreateBookingRequestModel model);
        Task<CustomEntityResult<CreateBookingByAdminResponseModel>> CreateBookingByAdmin(CreateBookingByAdminRequestModel model);
        public Task<CustomEntityResult<GetBookingByIdResponseModel>> GetBookingById(GetBookingByIdRequestModel bookingId);
        public Task<CustomEntityResult<ListBookingResponseModel>> GetAllBookingByUserId(string Id);
        public Task<CustomEntityResult<ListBookingResponseModel>> GetAllBookingList();
    }
}
