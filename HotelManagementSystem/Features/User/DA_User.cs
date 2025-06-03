namespace HotelManagementSystem.Features.User;
public class DA_User
{
    public async Task<CustomEntityResult<CreateUserResponseModel>> CreateUser(CreateUserRequestModel model)
    {
        try
        {
            var createUserResponse = new CreateUserResponseModel();
            return CustomEntityResult<CreateUserResponseModel>.GenerateSuccessEntityResult(createUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                ex.Message + ex.InnerException);
        }
    }
}
