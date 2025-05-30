using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface IBookingRepository
    {
        Task<CustomEntityResult<CreateBookingResponseDto>> CreateBookingByUser(CreateBookingRequestDto model);
        Task<CustomEntityResult<GetBookingByIdResponseDto>> GetBookingById(GetBookingByIdRequestDto bookingId);
        Task<CustomEntityResult<ListBookingResponseDto>> GetAllBookingByUserId(ListBookingRequestByUserDto dto);
        Task<CustomEntityResult<ListBookingResponseDto>> GetAllBookingList();
    }
}
