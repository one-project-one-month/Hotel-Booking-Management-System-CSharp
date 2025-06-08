using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.Constants;
using HotelManagementSystem.Data.Models.Guest;
using HotelManagementSystem.Data.Models.RoomType;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Helpers.Auth.PasswordHash;
using HotelManagementSystem.Service.Helpers.Auth.SMTP;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using HotelManagementSystem.Service.Services.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManagementSystem.Service.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenProcessors _tokenProcessor;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISmtpService _smtpService;

    public UserService(IUserRepository userRepo, ITokenProcessors tokenProcessor, 
        IPasswordHasher passwordHasher, ISmtpService smtpService)
    {
        _userRepo = userRepo;
        _tokenProcessor = tokenProcessor;
        _passwordHasher = passwordHasher;
        _smtpService = smtpService;
    }

    public async Task<CustomEntityResult<RegisterUserResponseModel>> RegisterUser(RegisterUserRequestModel model)
    {
        try
        {
            var registerUserRequest = new RegisterUserrequestDto
            {
                UserName = model.UserName,
                Email = model.Email,
                NewPassword = model.Password,
            };
            var registerUser = await _userRepo.RegisterUser(registerUserRequest);
            if (registerUser.IsError)
            {
                return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(registerUser.Result.RespCode, registerUser.Result.RespDescription);
            }

            var registerUserResponse = new RegisterUserResponseModel
            {
                RespCode = registerUser.Result.RespCode,
                RespDescription = registerUser.Result.RespDescription
            };
            return CustomEntityResult<RegisterUserResponseModel>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<SeedRoleResponseModel>> SeedRole()
    {
        try
        {
            var roles = new[]
            {
                RoleConstants.Admin,
                RoleConstants.User
            };
            foreach (var role in roles)
            {
                if (!await _userRepo.RoleExitAsync(role))
                {
                    var seedRoleRequest = new SeedRoleDto
                    {
                        RoleName = role,
                    };
                    await _userRepo.SeedRoleAsync(seedRoleRequest);
                }
            }
            var seedRoleResponse = new SeedRoleResponseModel {
                RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS,
                RespDescription = "Roles seeded successfully."
            };
            return CustomEntityResult<SeedRoleResponseModel>.GenerateSuccessEntityResult(seedRoleResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<SeedRoleResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<LoginResponseModel>> LoginAsync(LoginRequestModel model)
    {
        try
        {
            var email = model.Email;
            var user = await _userRepo.GetUserByEmail(email);
            var existingPassword = user.Password;
            if(existingPassword == null)
            {
                throw new PasswordCorruptedException("Password is corrupted or not set for this user. Please reset your password.");
            }
            var LoginResponse = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };
            bool isValid = _passwordHasher.VerifyPassword(existingPassword, model.Password);
            if (!isValid)
            {
                throw new IncorrectPasswordException("Password is incorrect");
            }
            var result = await _tokenProcessor.GenerateToken(LoginResponse);
            if (result.IsError)
            {
                return CustomEntityResult<LoginResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            var refreshToken = _tokenProcessor.GenerateRefreshToken();
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = refreshToken;
            user.TokenExpireAt = refreshTokenExpireAt;
            await _userRepo.UpdateTokenAsync(user);
            _tokenProcessor.WriteTokenInHttpOnlyCookie("refresh_token", refreshToken, refreshTokenExpireAt);
            var loginResponse = new LoginResponseModel {
                RespCode = result.Result.RespCode,
                RespDescription = result.Result.RespDescription,
            };
            return CustomEntityResult<LoginResponseModel>.GenerateSuccessEntityResult(loginResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<LoginResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<ForgotPasswordResponseModel>> ForgotPasswordAsync(ForgetPasswordRequestModel model)
    {
        try
        {
            var Otp = _tokenProcessor.GenerateOTPToken();
            var forgotPasswordRequest = new ForgotPasswordRequestDto
            {
                Email = model.Email,
                Otp = Otp
            };
            var response = await _userRepo.UpdateOTPAsync(forgotPasswordRequest);
            if (response.IsError)
            {
                return CustomEntityResult<ForgotPasswordResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Failed to update the token. Please check the request and try again.");
            }
            var subject = "Reset Your Password";
            var body = $"your password reset OTP is: {Otp}";

            var result = await _smtpService.SentPasswordOTPAsync(forgotPasswordRequest.Email, subject, body);
            if (result.IsError)
            {
                return CustomEntityResult<ForgotPasswordResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }
            var forgotPasswordResponse = new ForgotPasswordResponseModel
            {
                RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS,
                RespDescription = "OTP sent successfully to your email.",
            };
            return CustomEntityResult<ForgotPasswordResponseModel>.GenerateSuccessEntityResult(forgotPasswordResponse);
        
        }
        catch (Exception ex)
        {
            return CustomEntityResult<ForgotPasswordResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
    public async Task<CustomEntityResult<ResetPasswordResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel model)
    {
        try
        {
            var OTP = await _userRepo.GetValidPasswordOtpByEmailAsync(model.Email);
            if (OTP != model.OTP)
            {
                throw new OTPNotFoudException("OTP not found");
            }
            var existingUser = await _userRepo.GetUserByEmail(model.Email);
            if(existingUser.OtpExpireAt < DateTime.UtcNow)
            {
                throw new OTPNotFoudException("OTP expired");
            }
            var hashedPassword = _passwordHasher.HashPassword(model.Password);
            existingUser.Password = hashedPassword;
            await _userRepo.UpdatePasswordAsync(existingUser.UserId, hashedPassword);
            await _userRepo.DeletePasswordOTPAsync(existingUser);
            var resetPasswordResponse = new ResetPasswordResponseModel
            {
                RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS,
                RespDescription = "Password reset successfully. You can now login with your new password.",
            };
            return CustomEntityResult<ResetPasswordResponseModel>.GenerateSuccessEntityResult(resetPasswordResponse);

        }
        catch(Exception ex)
        {
            return CustomEntityResult<ResetPasswordResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
    public async Task<CustomEntityResult<CreateUserResponseModel>> CreateUserProfileAsync(string Id, CreateUserProfileRequestModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new UserNotFoundException("User not found. Please login again.");
            }

            if (!Guid.TryParse(Id, out var userId))
            {
                throw new InvalidUserIdException("Invalid user ID format.");
            }

            byte[]? imageByte = null;
            if (model.ProfileImg != null && model.ProfileImg.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.ProfileImg.CopyToAsync(ms);
                imageByte = ms.ToArray();
            }

            var createProfile = new CreateUserProfileRequestDto
            {
                UserId = userId,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                UserName = model.UserName,
                ProfileImg = imageByte,
                ProfileImgMimeType = model.ProfileImg?.ContentType
            };
            var result = await _userRepo.CreateUserProfileAsync(createProfile);
            if (result.IsError)
            {
                return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }
            var response = new CreateUserResponseModel
            {
                RespCode = result.Result.RespCode,
                RespDescription = result.Result.RespDescription
            };
            return CustomEntityResult<CreateUserResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch(Exception ex)
        {
            return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
    public async Task<CustomEntityResult<CreateUserResponseModel>> CreateUserProfileByAdminAsync(CreateUserProfileByAdminRequestModel model)
    {
        try
        {
            byte[]? imgBytes = null;

            if (!string.IsNullOrWhiteSpace(model.ProfileImg))
            {
                try
                {
                    imgBytes = Convert.FromBase64String(model.ProfileImg);
                }
                catch (FormatException ex)
                {
                    return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(
                        "400",
                        "Invalid Base64 image data: " + ex.Message
                    );
                }
            }
            var dto = new CreateUserProfileByAdminRequestDto
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Address = model.Address,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                ProfileImg = imgBytes,
                ProfileImgMimeType = model.ProfileImgMimeType
            };
            var result = await _userRepo.CreateUserProfileByAdminAsync(dto);
            if (result.IsError)
            {
                return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }
            return CustomEntityResult<CreateUserResponseModel>.GenerateSuccessEntityResult(new CreateUserResponseModel
            {
                RespCode = result.Result.RespCode,
                RespDescription = result.Result.RespDescription
            });
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
    public async Task<CustomEntityResult<UpdateUserProfileByIdResponseModel>> UpdateUserProfileByIdAsync(string Id, CreateUserProfileRequestModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new UserNotFoundException("User not found. Please login again.");
            }

            if (!Guid.TryParse(Id, out var userId))
            {
                throw new InvalidUserIdException("Invalid user ID format.");
            }

            byte[]? imageByte = null;
            if (model.ProfileImg != null && model.ProfileImg.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.ProfileImg.CopyToAsync(ms);
                imageByte = ms.ToArray();
            }

            var createProfile = new CreateUserProfileRequestDto
            {
                UserId = userId,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                UserName = model.UserName,
                ProfileImg = imageByte,
                ProfileImgMimeType = model.ProfileImg?.ContentType
            };
            var result = await _userRepo.CreateUserProfileAsync(createProfile);
            if (result.IsError)
            {
                return CustomEntityResult<UpdateUserProfileByIdResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }
            var response = new UpdateUserProfileByIdResponseModel
            {
                RespCode = result.Result.RespCode,
                RespDescription = result.Result.RespDescription,
            };
            return CustomEntityResult<UpdateUserProfileByIdResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<UpdateUserProfileByIdResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<GetUserProfileByIdResponseModel>> GetUserProfileByIdAsync(string Id)
    {
        try
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new UserNotFoundException("User not found. Please login again.");
            }
            if (!Guid.TryParse(Id, out var userId))
            {
                throw new InvalidUserIdException("Invalid user ID format.");
            }
            var id = new GetUserProfileByIdRequestDto
            {
                UserId = userId
            };
            var result = await _userRepo.GetUserProfileByIdAsync(id);

            if (result.IsError)
            {
                return CustomEntityResult<GetUserProfileByIdResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            var response = new GetUserProfileByIdResponseModel
            {
                UserName = result.Result.UserName,
                Address = result.Result.Address,
                DateOfBirth = result.Result.DateOfBirth,
                Gender = result.Result.Gender,
                ProfileImg = result.Result.ProfileImg != null ? Convert.ToBase64String(result.Result.ProfileImg) : null,
                ProfileImgMimeType = result.Result.ProfileImgMimeType
            };
            return CustomEntityResult<GetUserProfileByIdResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch(Exception ex)
        {
            return CustomEntityResult<GetUserProfileByIdResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<SeedRoleToAdminResponseModel>> SeedRoleToAdmin(SeedRoleToAdminRequestModel model)
    {
        try
        {
            var request = new SeedRoleToAdminRequestDto
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
            };
            var operate = await _userRepo.SeedRoleToAdmin(request);
            if (operate.IsError)
            {
                return CustomEntityResult<SeedRoleToAdminResponseModel>.GenerateFailEntityResult(operate.Result.RespCode,operate.Result.RespDescription);
            }
            var response = new SeedRoleToAdminResponseModel();
            return CustomEntityResult<SeedRoleToAdminResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch(Exception ex)
        {
            return CustomEntityResult<SeedRoleToAdminResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<GetAllUserInfoResponseModel>> GetAllUserInfoAsync()
    {
        try
        {
            var userListResult = await _userRepo.GetAllUserInfoAsync();

            if (userListResult.IsError)
            {
                return CustomEntityResult<GetAllUserInfoResponseModel>.GenerateFailEntityResult(userListResult.Result.RespCode, userListResult.Result.RespDescription);
            }

            var userListResponse = new GetAllUserInfoResponseModel
            {
                Users = userListResult.Result.Users!.Select(u => new GetAllUSerInfoModel
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    RoleName = u.RoleName,
                    Gender = u.Gender,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    CreatedAt = u.CreatedAt,
                }).ToList()
            };

            if (userListResponse.Users == null || !userListResponse.Users.Any())
            {
                return CustomEntityResult<GetAllUserInfoResponseModel>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_NOTFOUND,
                    "No Guest found");
            }

            return CustomEntityResult<GetAllUserInfoResponseModel>.GenerateSuccessEntityResult(userListResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<GetAllUserInfoResponseModel>.GenerateFailEntityResult(
                    ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                    ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
}