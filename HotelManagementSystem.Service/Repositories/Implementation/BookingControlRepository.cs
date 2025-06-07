using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.BookingControl;

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
            //BookingId = b.BookingId,
            //UserId = b.UserId,
            //GuestId = b.GuestId,
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
}
