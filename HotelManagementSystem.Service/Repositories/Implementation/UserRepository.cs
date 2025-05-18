using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Repositories.Interface;

namespace HotelManagementSystem.Service.Reposities.Implementation;

public class UserRepository : IUserRepository
{
    private readonly HotelDbContext _context;
    public UserRepository(HotelDbContext context)
    {
        _context = context;
    }
    public async Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model)
    {
        try
        {
            var registerUserRequest = new TblUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
            };

            var registerUser = await _context.AddAsync(registerUserRequest);
            await _context.SaveChangesAsync();

            var registerUserResponse = new RegisterUserResponseDto();
            return CustomEntityResult<RegisterUserResponseDto>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<SeedRoleResponseDto>> SeedRoleAsync(SeedRoleDto roleName)
    {
        try
        {
            var roles = new TblRole
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName.RoleName
            };
            await _context.AddAsync(roles);
            await _context.SaveChangesAsync();

            var seedRoleResponse = new SeedRoleResponseDto();
            return CustomEntityResult<SeedRoleResponseDto>.GenerateSuccessEntityResult(seedRoleResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<SeedRoleResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}