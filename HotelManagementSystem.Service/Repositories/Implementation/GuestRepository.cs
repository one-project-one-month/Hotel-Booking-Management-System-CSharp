using HotelManagementSystem.Data.Dtos.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                _context.TblGuests.Add(guest);
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
    }
}
