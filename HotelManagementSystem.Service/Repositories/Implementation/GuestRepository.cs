using HotelManagementSystem.Data.Dtos.Guest;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;
        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomEntityResult<CreateGuestResponseDto>> CreateGuest(CreateGuestRequestDto model)
        {
            try
            {
                var guest = new TblGuest
                {
                    UserId = model.UserId,
                    Nrc = model.Nrc,
                    PhoneNo = model.PhoneNo,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.TblGuests.AddAsync(guest);
                await _context.SaveChangesAsync();
                var response = new CreateGuestResponseDto
                {
                    RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS,
                    RespDescription = "Guest created successfully."
                };
                return CustomEntityResult<CreateGuestResponseDto>.GenerateSuccessEntityResult(response);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<CreateGuestResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message);
            }
        }

        public async Task<CustomEntityResult<GetAllGuestListResponseDto>> GetAllGeuestList()
        {
            try
            {
                var guestList = await _context.TblGuests.ToListAsync();
                if (guestList == null || !guestList.Any())
                {
                    return CustomEntityResult<GetAllGuestListResponseDto>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                        "No bookings found");
                }

                var guests = new GetAllGuestListResponseDto
                {
                    Guests = guestList.Select(b => new GetAllGuestListDto
                    {
                    }).ToList()
                };

                return CustomEntityResult<GetAllGuestListResponseDto>.GenerateSuccessEntityResult(guests);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<GetAllGuestListResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message);
            }
        }

        public async Task<CustomEntityResult<GetGuestByIdResponseDto>> GetGuestById(GetGuestByIdRequestDto dto)
        {
            try
            {
                var guest = await _context.TblGuests.FindAsync(dto.GuestId);
                if (guest == null)
                {
                    return CustomEntityResult<GetGuestByIdResponseDto>.GenerateFailEntityResult(
                        ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                        "Guest not found");
                }
                var response = new GetGuestByIdResponseDto
                {
                    GuestId = guest.GuestId,
                    UserId = guest.UserId,
                    Name = guest.Name,
                    Nrc = guest.Nrc,
                    PhoneNo = guest.PhoneNo,
                    Email = guest.Email,
                    CreatedAt = guest.CreatedAt
                };
                return CustomEntityResult<GetGuestByIdResponseDto>.GenerateSuccessEntityResult(response);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<GetGuestByIdResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message);
            }
        }
    }
}
