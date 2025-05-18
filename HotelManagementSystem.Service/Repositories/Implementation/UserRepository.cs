using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Service.Repositories.Interface;

namespace HotelManagementSystem.Service.Reposities.Implementation;

public class UserRepository : IUserRepository
{
    public UserRepository()
    {
        
    }
    
    public async Task<CustomEntityResult<CreateUserResponseDto>> CreateUser(CreateUserRequestDto model)
    {
        try
        {
            var createUserResponse = new CreateUserResponseDto();
            return CustomEntityResult<CreateUserResponseDto>.GenerateSuccessEntityResult(createUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}