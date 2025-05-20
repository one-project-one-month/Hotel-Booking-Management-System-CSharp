using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Service.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomEntityResult<CreateBookingResponseDto>> CreateBooking(CreateBookingRequestDto model)
        {
            try
            {
                var createBookingRequest = new BookingEntity
                {
                    Guest_Count = model.Guest_Count,
                    Booking_Status = model.Booking_Status,
                    Deposit_Amount = model.Deposit_Amount,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    PaymentType = model.PaymentType
                };
                var createBooking = await _context.Bookings.AddAsync(createBookingRequest);
                await _context.SaveChangesAsync();
                var creteBookingResponse = new CreateBookingResponseDto();
                return CustomEntityResult<CreateBookingResponseDto>.GenerateSuccessEntityResult(creteBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateBookingResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}