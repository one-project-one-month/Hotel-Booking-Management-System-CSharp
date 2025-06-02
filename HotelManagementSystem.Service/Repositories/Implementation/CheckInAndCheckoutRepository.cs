using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;
using HotelManagementSystem.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class CheckInAndCheckoutRepository: ICheckInAndCheckoutRepository
{
    private readonly HotelDbContext _hotelDbContext;

    public CheckInAndCheckoutRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }

    public async Task<CustomEntityResult<CreateCheckInAndCheckOutResponseDto>> CreateCheckInAndCheckout(CreateCheckInAndCheckOutRequestDto requestDto)
    {
        try
        {
            var guest = await _hotelDbContext.TblGuests
               .Where(rb => rb.GuestId == requestDto.GuestId)
               .FirstOrDefaultAsync();
            if (guest == null)
            {
                throw new GuestNotFoundException(requestDto.GuestId.ToString());
            }

            var checkInOut = new CheckInOut
            {
                GuestId = requestDto.GuestId,
                CheckInTime = requestDto.CheckInTime,
                CheckOutTime = requestDto.CheckOutTime,
                ExtraCharges = requestDto.ExtraCharges,
                Status = requestDto.Status
            };

            await _hotelDbContext.AddAsync(checkInOut);
            await _hotelDbContext.SaveChangesAsync();

            var checkInOutResponse = new CreateCheckInAndCheckOutResponseDto()
            {
                GuestNRC = guest!.Nrc,
                GuestPhone = guest.PhoneNo,
                CheckInTime = checkInOut.CheckInTime,
                CheckOutTime = checkInOut.CheckOutTime,
                ExtraCharges = checkInOut.ExtraCharges,
                Status = checkInOut.Status
            };

            return CustomEntityResult<CreateCheckInAndCheckOutResponseDto>.GenerateSuccessEntityResult(checkInOutResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateCheckInAndCheckOutResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}
