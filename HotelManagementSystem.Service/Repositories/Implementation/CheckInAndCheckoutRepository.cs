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

    public async Task<CustomEntityResult<CheckOutResponseDto>> CheckOutAsync(CheckOutRequestDto dto)
    {
        if(dto.GuestId == Guid.Empty)
        {
            return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_BADREQUEST, "GuestId is required.");
        }
        await using var transaction = await _hotelDbContext.Database.BeginTransactionAsync();
        try
        {
            var invoice = new InvoiceDto
            {
                GuestId = dto.GuestId
            };
            var booking = await _hotelDbContext.TblBookings.FirstOrDefaultAsync(b => b.GuestId == dto.GuestId);

            if(booking == null)
            {
                return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                    "Booking record with this guestid does not exist"
                    );
            }
            invoice.DepositeAmount = booking.DepositAmount;
            invoice.PaymentType = booking.PaymentType;

            // Get RoomBookings
            var roomBookings = await _hotelDbContext.TblRoomBookings
                .Where(rb => rb.BookingId == booking.BookingId)
                .ToListAsync();

            if (!roomBookings.Any())
            {
                return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                    "This guest hasn't got any room yet"
                );
            }

            var roomIds = roomBookings.Select(rb => rb.RoomId).ToList();

            var rooms = await _hotelDbContext.TblRooms
                .Where(r => roomIds.Contains(r.RoomId))
                .ToListAsync();

            if (rooms.Count != roomIds.Count)
            {
                return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                    "Some rooms could not be found."
                );
            }

            // Set rooms to available and collect RoomTypeIds
            rooms.ForEach(r => r.RoomStatus = "Available");
            await _hotelDbContext.SaveChangesAsync();

            // Get prices from room types
            var roomTypeIds = rooms.Select(r => r.RoomTypeId).ToList();
            var roomTypes = await _hotelDbContext.TblRoomTypes
                    .Where(rt => roomTypeIds.Contains(rt.RoomTypeId))
                    .ToListAsync();

            if (roomTypes.Count <= 0)
            {
                return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                    "Some RoomTypes could not be found."
                );
            }

            // Get check-in/out record
            var checkInOut = await _hotelDbContext.CheckInOuts
                .FirstOrDefaultAsync(co => co.GuestId == dto.GuestId && co.CheckOutTime == null);

            if (checkInOut == null || checkInOut.CheckInTime == null)
            {
                return CustomEntityResult<CheckOutResponseDto>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_BADREQUEST,
                    "Check-in record with this guest ID does not exist or has no check-in time."
                );
            }

            var checkInTime = checkInOut.CheckInTime.Value;
            var checkOutTime = DateTime.UtcNow;

            checkInOut.CheckOutTime = checkOutTime;
            checkInOut.Status = "Out";
            await _hotelDbContext.SaveChangesAsync();

            invoice.CheckInTime = checkInTime;
            invoice.CheckOutTime = checkOutTime;

            // Calculate base charges and extra charges
            decimal baseCharge = 0;
            decimal totalExtraCharges = 0;

            int numberOfNights = (int)Math.Ceiling((checkOutTime - checkInTime).TotalDays);
            if (numberOfNights == 0) numberOfNights = 1; // charge at least for one night

            foreach (var room in rooms)
            {
                var roomType = roomTypes.First(rt => rt.RoomTypeId == room.RoomTypeId);
                decimal price = roomType.Price;

                baseCharge += numberOfNights * price;

                var stayLog = new StayLog
                {
                    ActualCheckIn = checkInTime,
                    ActualCheckOut = checkOutTime,
                    RoomRatePerNight = price
                };

                totalExtraCharges += stayLog.CalculateEarlyCheckInFee();
                totalExtraCharges += stayLog.CalculateLateCheckOutFee();
            }

            invoice.Extracharges = totalExtraCharges;
            invoice.TotalAmount = baseCharge + totalExtraCharges;
            decimal depositeAmount = invoice.DepositeAmount ?? 0;
            var createInvoice = new TblInvoice
                {
                    GuestId = invoice.GuestId,
                    CheckInTime = invoice.CheckInTime,
                    CheckOutTime = invoice.CheckOutTime,
                    ExtraCharges = invoice.Extracharges,
                    Deposite = depositeAmount,
                    TotalAmount = invoice.TotalAmount,
                    PaymentType = invoice.PaymentType
                };
            await _hotelDbContext.TblInvoices.AddAsync(createInvoice);
            await _hotelDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = new CheckOutResponseDto
            {
                InvoiceId = createInvoice.InvoiceId,
                GuestId = invoice.GuestId,
                CheckInTime = invoice.CheckInTime,
                CheckOutTime = invoice.CheckOutTime,
                Extracharges = invoice.Extracharges,
                DepositeAmount = invoice.DepositeAmount,
                TotalAmount = invoice.TotalAmount,
                PaymentType = invoice.PaymentType,
            };

            return CustomEntityResult<CheckOutResponseDto>.GenerateSuccessEntityResult(response);
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
