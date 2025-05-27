using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
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
                    BookingId = Guid.NewGuid().ToString(),
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    Guest_Count = model.Guest_Count,
                    Booking_Status = model.Booking_Status,
                    Deposit_Amount = model.Deposit_Amount,
                    Total_Amount = model.Total_Amount,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    PaymentType = model.PaymentType
                };
                //var createBooking = await _context.Bookings.AddAsync(createBookingRequest);
                //await _context.SaveChangesAsync();
                var creteBookingResponse = new CreateBookingResponseDto()
                {
                    BookingId = createBookingRequest.BookingId
                };
                return CustomEntityResult<CreateBookingResponseDto>.GenerateSuccessEntityResult(creteBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateBookingResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
        public async Task<CustomEntityResult<GetBookingByIdResponseDto>> GetBookingById(GetBookingByIdRequestDto bookingId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking is null)
                {
                    return CustomEntityResult<GetBookingByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Booking not found");
                }
                var getBookingResponse = new GetBookingByIdResponseDto
                {
                    BookingId = booking.BookingId,
                    UserId = booking.UserId,
                    GuestId = booking.GuestId,
                    Guest_Count = booking.Guest_Count,
                    Booking_Status = booking.Booking_Status,
                    Deposit_Amount = booking.Deposit_Amount,
                    Total_Amount = booking.Total_Amount,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    PaymentType = booking.PaymentType
                };
                return CustomEntityResult<GetBookingByIdResponseDto>.GenerateSuccessEntityResult(getBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<GetBookingByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
        public async Task<CustomEntityResult<ListBookingResponseDto>> BookingList()
        {
            try
            {
                var bookings = await _context.Bookings.ToListAsync();

                if (bookings == null || !bookings.Any())
                {
                    return CustomEntityResult<ListBookingResponseDto>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                        "No bookings found");
                }

                var bookingList = new ListBookingResponseDto
                {
                    Bookings = bookings.Select(b => new ListBookingDto
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        GuestId = b.GuestId,
                        Guest_Count = b.Guest_Count,
                        Booking_Status = b.Booking_Status,
                        Deposit_Amount = b.Deposit_Amount,
                        Total_Amount = b.Total_Amount,
                        CheckInDate = b.CheckInDate,
                        CheckOutDate = b.CheckOutDate,
                        PaymentType = b.PaymentType
                    }).ToList()
                };

                return CustomEntityResult<ListBookingResponseDto>.GenerateSuccessEntityResult(bookingList);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<ListBookingResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    ex.Message + ex.InnerException?.Message);
            }
        }

    }
}