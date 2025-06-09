using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Booking;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomEntityResult<CreateBookingResponseDto>> CreateBookingByUser(CreateBookingRequestDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createBookingRequest = new TblBooking
                {
                    UserId = dto.UserId,
                    GuestId = dto.GuestId,
                    CheckInTime = dto.CheckInDate,
                    CheckOutTime = dto.CheckOutDate,
                    DepositAmount = dto.Deposit_Amount,
                    BookingStatus = dto.Booking_Status,
                    TotalAmount = dto.Total_Amount,
                    PaymentType = dto.PaymentType
                };
                var createBooking = await _context.TblBookings.AddAsync(createBookingRequest);
                await _context.SaveChangesAsync();

                if (dto.Rooms != null && dto.Rooms.Any())
                {
                    var roomBookings = dto.Rooms.Select(roomId => new TblRoomBooking
                    {
                        RoomId = roomId,
                        BookingId = createBooking.Entity.BookingId
                    });

                    await _context.TblRoomBookings.AddRangeAsync(roomBookings);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                var creteBookingResponse = new CreateBookingResponseDto();
                return CustomEntityResult<CreateBookingResponseDto>.GenerateSuccessEntityResult(creteBookingResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return CustomEntityResult<CreateBookingResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }

        public async Task<CustomEntityResult<GetBookingByIdResponseDto>> GetBookingById(GetBookingByIdRequestDto dto)
        {
            try
            {
                var booking = await _context.TblBookings.FindAsync(dto);
                if (booking is null)
                {
                    return CustomEntityResult<GetBookingByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Booking not found");
                }
                var getBookingResponse = new GetBookingByIdResponseDto
                {
                    BookingId = booking.BookingId,
                    UserId = booking.UserId,
                    GuestId = booking.GuestId,
                    Guest_Count = booking.GuestCount,
                    Booking_Status = booking.BookingStatus,
                    Deposit_Amount = booking.DepositAmount,
                    Total_Amount = booking.TotalAmount,
                    CheckInDate = booking.CheckInTime,
                    CheckOutDate = booking.CheckOutTime,
                    PaymentType = booking.PaymentType,
                    RoomNumbers = booking.TblRoomBookings
                .Where(rb => rb.Room != null)
                .Select(rb => rb.Room.RoomNo.ToString())
                .ToList()
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
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        GuestId = b.GuestId,
                        UserName = b.Guest!.Name,
                        Email = b.Guest!.Email,
                        Guest_Count = b.GuestCount,
                        Booking_Status = b.BookingStatus,
                        Deposit_Amount = b.DepositAmount,
                        Total_Amount = b.TotalAmount,
                        CheckInDate = b.CheckInTime,
                        CheckOutDate = b.CheckOutTime,
                        PaymentType = b.PaymentType,
                        CreatedAt = b.CreatedAt,
                        RoomNumbers = b.TblRoomBookings
                            .Where(rb => rb.Room != null)
                            .Select(rb => rb.Room.RoomNo.ToString())
                            .ToList()
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