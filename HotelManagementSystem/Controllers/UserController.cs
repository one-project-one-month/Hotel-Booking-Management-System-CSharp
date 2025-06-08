using HotelManagementSystem.Data.Models.User;
using Sprache;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IHttpContextAccessor _httpContext;

    public UserController(IUserService service, IHttpContextAccessor httpContext)
    {
        _service = service;
        _httpContext = httpContext;
    }
    
    [HttpPost]
    [Route("SeedRole")]
    public async Task<ActionResult<SeedRoleResponseModel>> SeedRoleAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.SeedRole();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [Authorize(Roles ="Admin")]
    [HttpPost]
    [Route("SeedRoleToAdmin")]
    public async Task<ActionResult<BasedResponseModel>> SeedRoleToAdmin(SeedRoleToAdminRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.SeedRoleToAdmin(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<BasedResponseModel>> RegisterUserAsync(RegisterUserRequestModel model)
    {
        #region UserGetClaimsValue

        #endregion

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            #region CheckRequiredField
            //if (String.IsNullOrEmpty(model.UserName))
            //{
            //return APIHelper.GenerateResponseForRequiredField(nameof(model.UserName), _sharedLocalizer);
            //}
            #endregion

            #region Check Format
            // if(model.UserType != EntitiesConstant.USER_TYPE.USER.GetHashCode())
            // {
            //     return BadRequest(ErrorMessageConstant.EM_UserTypeNotAcceptable); ///Not Acceptable
            // }
            #endregion

            var result = await _service.RegisterUser(model);

            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<BasedResponseModel>> LoginAsync(LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.LoginAsync(model);

            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("forgotpassword")]
    public async Task<ActionResult<BasedResponseModel>> ForgotPasswordAsync(ForgetPasswordRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.ForgotPasswordAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<ActionResult<BasedResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.ResetPasswordAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [Authorize]
    [HttpPost]
    [Route("createuserprofilebyuser")]
    public async Task<ActionResult<CreateUserResponseModel>> CreateUserProfileByUserAsync([FromForm] CreateUserProfileRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found. Please login again.");
            }
            var result = await _service.CreateUserProfileAsync(userId, model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("createuserprofilebyadmin")]
    public async Task<ActionResult<CreateUserResponseModel>> CreateUserProfileByAdminAsync(CreateUserProfileByAdminRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.CreateUserProfileByAdminAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [Authorize]
    [HttpPatch]
    [Route("updateuserprofile")]
    public async Task<ActionResult<UpdateUserProfileByIdResponseModel>> UpdateUserProfileByIdAsync(CreateUserProfileRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found. Please login again.");
            }
            var result = await _service.UpdateUserProfileByIdAsync(userId, model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    //[Authorize]
    [HttpGet]
    [Route("getalluserprofile")]
    public async Task<ActionResult<GetAllUserInfoResponseModel>> GetAllUserProfileAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.GetAllUserInfoAsync();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    //[Authorize]
    [HttpGet]
    [Route("getuserprofilebyid")]
    public async Task<ActionResult<GetUserProfileByIdResponseModel>> GetUserById()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found. Please login again.");
            }
            var result = await _service.GetUserProfileByIdAsync(userId);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("getclaim")]
    public async Task<ActionResult<BasedResponseModel>> GetClaimAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Role = role
            });
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }
}