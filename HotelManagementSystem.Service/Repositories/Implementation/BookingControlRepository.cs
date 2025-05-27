using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Data.Models;

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

    public async Task<CustomEntityResult<GetBookingsResponseDto>> DeleteBooking(string BookingId)
    {
        try
        {
            var booking = await _hotelDbContext.TblBookings
                .Include(b => b.TblRoomBookings)
            .Where(b => b.BookingId.ToString() == BookingId)
            .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new BookingNotFoundException(BookingId);
            }
            var roomBookingId = booking.TblRoomBookings.Select(rb => rb.BookingId)
                .ToList();

            foreach(var roombooingIdIndex in roomBookingId)
            {
                var roomBooking = await _hotelDbContext.TblRoomBookings
                .Where(b => b.BookingId == roombooingIdIndex)
                .FirstOrDefaultAsync();

                if (booking == null)
                {
                    throw new BookingNotFoundException(BookingId);
                }
                _hotelDbContext.TblRoomBookings.Remove(roomBooking!);
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
}
