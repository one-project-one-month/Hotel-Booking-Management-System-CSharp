using HotelManagementSystem.Data.Dtos.CheckInAndCheckOutDto;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;
using HotelManagementSystem.Service.Exceptions;

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

    private const int GracePeriodMinutes = 15;

    public async Task<CustomEntityResult<CheckOutResponseDto>> CheckOutAsync(CheckOutRequestDto dto)
    {
        if(dto.GuestId == Guid.Empty)
        {
            return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST, "GuestId is required.");
        }
        await using var transaction = await _hotelDbContext.Database.BeginTransactionAsync();
        try
        {
            var booking = await _hotelDbContext.TblBookings
                              .FirstOrDefaultAsync(b => b.GuestId == dto.GuestId);

            if (booking is null)
                return CustomEntityResult<CheckOutResponseDto>
                       .GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                                                 "Booking record with this guest id does not exist.");

            var roomBookings = await _hotelDbContext.TblRoomBookings
                                  .Where(rb => rb.BookingId == booking.BookingId)
                                  .ToListAsync();

            if (!roomBookings.Any())
                return CustomEntityResult<CheckOutResponseDto>
                       .GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                                                 "This guest hasn't got any room yet.");

            var rooms = await _hotelDbContext.TblRooms
                            .Where(r => roomBookings.Select(rb => rb.RoomId)
                                                    .Contains(r.RoomId))
                            .ToListAsync();

            if (rooms.Count != roomBookings.Count)
                return CustomEntityResult<CheckOutResponseDto>
                       .GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                                                 "Some rooms could not be found.");

            var roomTypes = await _hotelDbContext.TblRoomTypes
                               .Where(rt => rooms.Select(r => r.RoomTypeId).Contains(rt.RoomTypeId))
                               .ToListAsync();

            if (!roomTypes.Any())
                return CustomEntityResult<CheckOutResponseDto>
                       .GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                                                 "Some RoomTypes could not be found.");

            var checkInOut = await _hotelDbContext.CheckInOuts
                                 .FirstOrDefaultAsync(co => co.GuestId == dto.GuestId &&
                                                            co.CheckOutTime == null);

            if (checkInOut is null || checkInOut.CheckInTime is null)
                return CustomEntityResult<CheckOutResponseDto>
                       .GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                                                 "Check‑in record does not exist or has no check‑in time.");

            
            var checkInLocal = checkInOut.CheckInTime.Value;          
            var checkOutLocal = EntityConstantsHelper.GetMyanmarLocalTime();

            checkInOut.CheckOutTime = checkOutLocal;
            checkInOut.Status = "Out";
            await _hotelDbContext.SaveChangesAsync();

            int nights = Math.Max(1,
                           (int)Math.Ceiling((checkOutLocal.Date - checkInLocal.Date).TotalDays));

            decimal baseCharge = 0m;
            decimal highestNightlyRate = 0m;

            foreach (var room in rooms)
            {
                var rate = roomTypes.First(rt => rt.RoomTypeId == room.RoomTypeId).Price;
                baseCharge += nights * rate;
                highestNightlyRate = Math.Max(highestNightlyRate, rate);

                room.RoomStatus = "Available";   
            }
            await _hotelDbContext.SaveChangesAsync();
            decimal extraCharges = 0m;

            if ((checkOutLocal - checkInLocal) >= TimeSpan.FromMinutes(GracePeriodMinutes))
            {
                var stay = new StayLog
                {
                    ActualCheckIn = checkInLocal,
                    ActualCheckOut = checkOutLocal,
                    RoomRatePerNight = highestNightlyRate  
                };

                extraCharges += stay.CalculateEarlyCheckInFee();
                extraCharges += stay.CalculateLateCheckOutFee();
            }

            var invoice = new TblInvoice
            {
                GuestId = dto.GuestId,
                CheckInTime = checkInLocal,
                CheckOutTime = checkOutLocal,
                Deposite = booking.DepositAmount ?? 0m,
                ExtraCharges = extraCharges,
                TotalAmount = baseCharge + extraCharges,
                PaymentType = booking.PaymentType
            };

            await _hotelDbContext.TblInvoices.AddAsync(invoice);
            await _hotelDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = new CheckOutResponseDto
            {
                InvoiceId = invoice.InvoiceId,
                GuestId = dto.GuestId,
                CheckInTime = checkInLocal,
                CheckOutTime = checkOutLocal,
                Extracharges = extraCharges,
                DepositeAmount = booking.DepositAmount,
                TotalAmount = invoice.TotalAmount,
                PaymentType = booking.PaymentType
            };

            return CustomEntityResult<CheckOutResponseDto>
                   .GenerateSuccessEntityResult(response);
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();

            return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                $"Failed to check out: {ex.Message} {(ex.InnerException?.Message ?? "")}");
        }
    }
}
