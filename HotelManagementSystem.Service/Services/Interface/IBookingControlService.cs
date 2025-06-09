using HotelManagementSystem.Data.Dtos.BookingControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagementSystem.Data.Models.BookingControl;
using HotelManagementSystem.Data.Models.Booking;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IBookingControlService
{
    public Task<CustomEntityResult<GetBookingsResponseModel>> GetBookings();
    public Task<CustomEntityResult<UpdateBookingResponseModel>> UpdateBooking(UpdateBookingRequestModel requestModel);
    public Task<CustomEntityResult<DeleteBookingResponseModel>> DeleteBooking(DeleteBookingRequestModel BookingId);
    public Task<CustomEntityResult<CreateBookingByAdminResponseModel>> CreateBookingByAdmin(CreateBookingByAdminRequestModel model);
}
