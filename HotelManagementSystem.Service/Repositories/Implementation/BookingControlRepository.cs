using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Service.Exceptions;

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

            foreach (var existingRoomBooking in existingRoomBookings)
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

    public async Task<CustomEntityResult<UpdateBookingResponseDto>> UpdateBooking(UpdateBookingRequestDto dto)
    {
        try
        {
            var booking = await _hotelDbContext.TblBookings
                .Include(b => b.TblRoomBookings)
            .Where(b => b.BookingId == dto.BookingId)
            .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new BookingNotFoundException(dto.BookingId.ToString());
            }

            if (dto.UserId.HasValue)
                booking.UserId = dto.UserId;

            if (dto.GuestId.HasValue)
                booking.GuestId = dto.GuestId;

            if (dto.GuestCount.HasValue)
                booking.GuestCount = dto.GuestCount;

            if (dto.CheckInTime.HasValue)
                booking.CheckInTime = dto.CheckInTime;

            if (dto.CheckOutTime.HasValue)
                booking.CheckOutTime = dto.CheckOutTime;

            if (dto.DepositAmount.HasValue)
                booking.DepositAmount = dto.DepositAmount;

            if (!string.IsNullOrWhiteSpace(dto.BookingStatus))
                booking.BookingStatus = dto.BookingStatus;

            if (dto.TotalAmount.HasValue)
                booking.TotalAmount = dto.TotalAmount;

            if (dto.CreatedAt.HasValue)
                booking.CreatedAt = dto.CreatedAt;

            if (!string.IsNullOrWhiteSpace(dto.PaymentType))
                booking.PaymentType = dto.PaymentType;
            if (dto.Rooms != null && dto.Rooms.Any())
            {
                _hotelDbContext.TblRoomBookings.RemoveRange(booking.TblRoomBookings);
                foreach (var roomId in dto.Rooms)
                {
                    booking.TblRoomBookings.Add(new TblRoomBooking
                    {
                        BookingId = booking.BookingId,
                        RoomId = roomId
                    });
                }
            }

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
        if (dto.Rooms != null && dto.Rooms.Any())
        {
            var rooms = await _hotelDbContext.TblRooms
                .Where(r => dto.Rooms.Contains(r.RoomId))
                .ToListAsync();

            foreach (var room in rooms)
            {
                if (room.RoomStatus == "Occupied" || room.RoomStatus == "Maintenance")
                {
                    return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                        $"Room {room.RoomNo} is already booked or under maintenance.");
                }
            }
        }

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
            var booking = new TblBooking
            {
                UserId = dto.UserId,
                GuestId = guest.GuestId,
                GuestCount = dto.GuestCount,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                DepositAmount = dto.DepositAmount,
                BookingStatus = dto.BookingStatus,
                TotalAmount = dto.TotalAmount,
                PaymentType = dto.PaymentType,
                CreatedAt = DateTime.UtcNow
            };

            await _hotelDbContext.TblBookings.AddAsync(booking);
            await _hotelDbContext.SaveChangesAsync();

            if (dto.Rooms != null && dto.Rooms.Any())
            {
                var roomBookings = dto.Rooms.Select(roomId => new TblRoomBooking
                {
                    RoomId = roomId,
                    BookingId = booking.BookingId
                });

                await _hotelDbContext.TblRoomBookings.AddRangeAsync(roomBookings);

                var roomsToUpdate = await _hotelDbContext.TblRooms
                    .Where(r => dto.Rooms.Contains(r.RoomId))
                    .ToListAsync();

                foreach (var room in roomsToUpdate)
                {
                    room.RoomStatus = "Occupied";
                }

                await _hotelDbContext.SaveChangesAsync();
            }

            var checkInOut = new CheckInOut
            {
                GuestId = guest.GuestId,
                CheckInTime = DateTime.UtcNow,
                Status = "In"
            };

            await _hotelDbContext.CheckInOuts.AddAsync(checkInOut);
            await _hotelDbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            var response = new CreateBookingByAdminResponseDto
            {
                BookingId = booking.BookingId,
                GuestId = guest.GuestId
            };

            return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return CustomEntityResult<CreateBookingByAdminResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                $"Failed to create booking: {ex.Message} {(ex.InnerException?.Message ?? "")}");
        }
    }
}
