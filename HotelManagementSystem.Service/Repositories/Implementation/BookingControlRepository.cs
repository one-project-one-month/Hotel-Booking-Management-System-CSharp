using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class BookingControlRepository : IBookingControlRepository
{
    private readonly HotelDbContext _hotelDbContext;

    public BookingControlRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }

    public async Task<CustomEntityResult<GetBookingsResponseDto>> GetBookings()
    {
        var bookings = await _hotelDbContext.TblBookings
            .AsNoTracking()
            .Include(g => g.Guest)
            .Include(u => u.User)
            .Include(rb => rb.TblRoomBookings)
                .ThenInclude(rb => rb.Room)
            .ToListAsync();

        var getBookingResponse = bookings.Select(b => new GetBookingResponseDto
        {
            BookingId = b.BookingId,
            UserId = b.UserId,
            GuestId = b.GuestId,
            GuestCount = b.GuestCount,
            CheckIn_Time = b.CheckInTime,
            CheckOut_Time = b.CheckOutTime,
            Deposit_Amount = b.DepositAmount,
            BookingStatus = b.BookingStatus,
            TotalAmount = b.TotalAmount,
            CreatedAt = b.CreatedAt,
            PaymentType = b.PaymentType,
            GuestNrc = b.Guest!.Nrc,
            GuestPhoneNo = b.Guest!.PhoneNo, 
            UserName = b.User!.UserName,
            GuestName = b.Guest.Name,

            RoomNo = b.TblRoomBookings
                .Where(rb => rb.Room != null)
                .Select(rb => rb.Room.RoomNo.ToString())
                .ToList()
        }).ToList();

        var getBookingsResponse = new GetBookingsResponseDto
        {
            Bookings = getBookingResponse
        };

        return CustomEntityResult<GetBookingsResponseDto>.GenerateSuccessEntityResult(getBookingsResponse);
    }

    public async Task<CustomEntityResult<GetBookingsResponseDto>> DeleteBooking(DeleteBookingRequestDto Booking)
    {
        try
        {
            var booking = await _hotelDbContext.TblBookings
                .Include(b => b.TblRoomBookings)
            .Where(b => b.BookingId == Booking.BookingId)
            .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new BookingNotFoundException(Booking.BookingId.ToString());
            }
            var existingRoomBookings = booking.TblRoomBookings.ToList();

            foreach(var existingRoomBooking in existingRoomBookings)
            {
                _hotelDbContext.TblRoomBookings.Remove(existingRoomBooking);
            }   

            _hotelDbContext.TblBookings.Remove(booking);
            var result = await _hotelDbContext.SaveChangesAsync();
            var returnModel = new GetBookingsResponseDto();
            return CustomEntityResult<GetBookingsResponseDto>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<GetBookingsResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }        
    }

    public async Task<CustomEntityResult<UpdateBookingResponseDto>> UpdateBooking(UpdateBookingRequestDto requestBookingDto)
    {
        try
        {
            var booking = await _hotelDbContext.TblBookings
                .Include(b => b.TblRoomBookings)
            .Where(b => b.BookingId == requestBookingDto.BookingId)
            .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new BookingNotFoundException(requestBookingDto.BookingId.ToString());
            }

            var existingRoomBookings = booking.TblRoomBookings.ToList();
            foreach (var existingRoomBooking in existingRoomBookings)
            {
                _hotelDbContext.TblRoomBookings.Remove(existingRoomBooking);
            }

            foreach (var requestRoomBooking in requestBookingDto.Rooms)
            {                
                var roomId = requestRoomBooking;
                var roomBooking = new TblRoomBooking
                {
                    BookingId = booking.BookingId,
                    RoomId = roomId
                };
                booking.TblRoomBookings.Add(roomBooking);
            }
            
            booking.UserId = requestBookingDto.UserId;
            booking.GuestId = requestBookingDto.GuestId;
            booking.GuestCount = requestBookingDto.GuestCount;
            booking.CheckInTime = requestBookingDto.CheckInTime;
            booking.CheckOutTime = requestBookingDto.CheckOutTime;
            booking.DepositAmount = requestBookingDto.DepositAmount;
            booking.BookingStatus = requestBookingDto.BookingStatus;
            booking.TotalAmount = requestBookingDto.TotalAmount;
            booking.CreatedAt = requestBookingDto.CreatedAt;
            booking.PaymentType = requestBookingDto.PaymentType;

            var result = await _hotelDbContext.SaveChangesAsync();
            var returnModel = new UpdateBookingResponseDto();
            return CustomEntityResult<UpdateBookingResponseDto>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<UpdateBookingResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<CreateBookingByAdminResponseDto>> CreateBookingByAdmin(CreateBookingByAdminRequestDto dto)
    {
        await using var transaction = await _hotelDbContext.Database.BeginTransactionAsync();
        try
        {
            var guest = new TblGuest
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Nrc = dto.Nrc,
                PhoneNo = dto.PhoneNo,
                CreatedAt = DateTime.UtcNow
            };
            await _hotelDbContext.TblGuests.AddAsync(guest);
            await _hotelDbContext.SaveChangesAsync();

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
                TotalAmount = dto.TotalAmount,
                PaymentType = dto.PaymentType,
                CreatedAt = DateTime.UtcNow
            };

            var createBooking = await _hotelDbContext.TblBookings.AddAsync(createBookingRequest);
            await _hotelDbContext.SaveChangesAsync();

            if (dto.Rooms != null && dto.Rooms.Any())
            {
                var roomBookings = dto.Rooms.Select(roomId => new TblRoomBooking
                {
                    RoomId = roomId,
                    BookingId = createBooking.Entity.BookingId
                });

                await _hotelDbContext.TblRoomBookings.AddRangeAsync(roomBookings);
                await _hotelDbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            var creteBookingResponse = new CreateBookingByAdminResponseDto
            {
                BookingId = createBooking.Entity.BookingId
            };

            return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateSuccessEntityResult(creteBookingResponse);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                $"Failed to create user profile: {ex.Message} {(ex.InnerException?.Message ?? "")}");
        }
    }
}
