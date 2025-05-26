using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class BookingControlRepository : IBookingControlRepository
{
    private readonly HotelDbContext _hotelDbContext;

    public BookingControlRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }

    public async Task<CustomEntityResult<GetBookingsResponseDto>> GetBookingControl()
    {
        var bookings = await _hotelDbContext.TblBookings
            .AsNoTracking()
            .ToListAsync();

        var getBookingResponse = bookings.Select(b => new GetBookingResponseDto
        {
            BookingId = b.BookingId,
            UserId = b.UserId
        }).ToList();

        var getBookingsResponse = new GetBookingsResponseDto
        {
            Bookings = getBookingResponse
        };

        return CustomEntityResult<GetBookingsResponseDto>.GenerateSuccessEntityResult(getBookingsResponse);

    }
}
