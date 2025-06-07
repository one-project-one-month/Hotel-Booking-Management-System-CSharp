using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<CustomEntityResult<CreateBookingByAdminResponseDto>> CreateBookingByAdmin(CreateBookingByAdminRequestDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var guest = new TblGuest
                {
                    UserId = dto.UserId,
                    Nrc = dto.Nrc,
                    PhoneNo = dto.PhoneNo,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.TblGuests.AddAsync(guest);
                await _context.SaveChangesAsync();

                var GuestId = guest.GuestId;
                var createBookingRequest = new TblBooking
                {
                    UserId = dto.UserId,
                    GuestId = GuestId,
                    GuestCount = dto.GuestCount,
                    CheckInTime = dto.CheckInTime,
                    CheckOutTime = dto.CheckOutTime,
                    DepositAmount = dto.DepositAmount,
                    BookingStatus = "Booked",
                    TotalAmount = dto.TotalAmount
                };

                var createBooking = await _context.TblBookings.AddAsync(createBookingRequest);
                await _context.SaveChangesAsync();

                var creteBookingResponse = new CreateBookingByAdminResponseDto
                {
                    BookingId = createBooking.Entity.BookingId
                };

                await transaction.CommitAsync();

                return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateSuccessEntityResult(creteBookingResponse);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    $"Failed to create user profile: {ex.Message} {(ex.InnerException?.Message ?? "")}");
            }
        }

        public async Task<CustomEntityResult<CreateBookingResponseDto>> CreateBookingByUser(CreateBookingRequestDto model)
        {
            try
            {
                var createBookingRequest = new TblBooking
                {
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    CheckInTime = model.CheckInDate,
                    CheckOutTime = model.CheckOutDate,
                    DepositAmount = model.Deposit_Amount,
                    BookingStatus = model.Booking_Status,
                    TotalAmount = model.Total_Amount,
                    PaymentType = model.PaymentType
                };
                var createBooking = await _context.TblBookings.AddAsync(createBookingRequest);
                await _context.SaveChangesAsync();
                var creteBookingResponse = new CreateBookingResponseDto();
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
                var booking = await _context.TblBookings.FindAsync(bookingId);
                if (booking is null)
                {
                    return CustomEntityResult<GetBookingByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Booking not found");
                }
                var getBookingResponse = new GetBookingByIdResponseDto
                {
                };
                return CustomEntityResult<GetBookingByIdResponseDto>.GenerateSuccessEntityResult(getBookingResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<GetBookingByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }

        public async Task<CustomEntityResult<ListBookingResponseDto>> GetAllBookingByUserId(ListBookingRequestByUserDto dto)
        {
            try
            {
                var bookings = await _context.TblBookings.Where(x => x.UserId ==  dto.UserId).ToListAsync();
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

        public async Task<CustomEntityResult<ListBookingResponseDto>> GetAllBookingList()
        {
            try
            {
                var bookings = await _context.TblBookings.ToListAsync();

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