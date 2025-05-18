using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingService(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }
        public async Task<CustomEntityResult<CreateBookingResponseModel>> CreaeBooking(CreateBookingRequestModel model)
        {
            try
            {
                var createBookingResponse = new CreateBookingResponseModel();
                return CustomEntityResult<CreateBookingResponseModel>.GenerateSuccessEntityResult(createBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateBookingResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}