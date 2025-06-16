using HotelManagementSystem.Data.Dtos.Booking;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.Constants;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Helpers.Auth.PasswordHash;
using Org.BouncyCastle.Crypto.Operators;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly HotelDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(HotelDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model)
    {
        try
        {
            var registerUserRequest = new TblUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var hashedPassword = _passwordHasher.HashPassword(model.NewPassword);
            registerUserRequest.Password = hashedPassword;
            registerUserRequest.RoleId = await SeedRoleToUser();
            await _context.AddAsync(registerUserRequest);
            await _context.SaveChangesAsync();
            var registerUserResponse = new RegisterUserResponseDto();
            return CustomEntityResult<RegisterUserResponseDto>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<TblUser> GetUserByEmail(string email)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException(email);
        }

        return user;
    }

    public async Task<TblUser> GetUserById(Guid userId)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
        {
            throw new UserDoesNotExitException(userId);
        }

        return user;
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
            return CustomEntityResult<SeedRoleResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<string> GetUserRolebyIdAsync(Guid id)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == id);
        if (user == null)
        {
            throw new UserDoesNotExitException(id);
        }

        var role = await _context.TblRoles.FirstOrDefaultAsync(x => x.RoleId == user.RoleId);
        if (role == null)
        {
            throw new RoleDoesNotExistException("Role does not exit!");
        }

        return role.RoleName;
    }

    public async Task<bool> RoleExitAsync(string roleName)
    {
        return await _context.TblRoles.AnyAsync(r => r.RoleName == roleName);
    }

    public async Task<Guid> SeedRoleToUser()
    {
        var Role = await _context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == RoleConstants.User);
        if (Role == null)
        {
            throw new RoleDoesNotExistException("User Role does not exit!");
        }

        var RoleId = Role.RoleId;
        return RoleId;
    }

    public async Task UpdateTokenAsync(TblUser user)
    {
        try
        {
            var existingUser = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null)
            {
                throw new UserNotFoundException(user.Email);
            }

            if (user.RefreshToken != null)
                existingUser.RefreshToken = user.RefreshToken;

            if (user.TokenExpireAt != default)
                existingUser.TokenExpireAt = user.TokenExpireAt;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update user: {ex.Message}", ex);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> DeletePasswordOTPAsync(TblUser user)
    {
        try
        {
            var existingUser = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null)
            {
                throw new UserNotFoundException(user.Email);
            }

            existingUser.ForgetPasswordOtp = null;
            existingUser.OtpExpireAt = default;
            await _context.SaveChangesAsync();

            var returnModel = new BasedResponseModel();
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<string> GetValidPasswordOtpByEmailAsync(string email)
    {
        try
        {
            var user = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }

            var token = user.ForgetPasswordOtp;
            if (token == null)
            {
                throw new OTPNotFoudException("OTP not found");
            }

            return token;
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}", ex);
        }
    }

    public async Task<CustomEntityResult<ForgotPasswordResponseDto>> UpdateOTPAsync(ForgotPasswordRequestDto dto)
    {
        try
        {
            var user = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                throw new UserNotFoundException(dto.Email);
            }

            user.ForgetPasswordOtp = dto.Otp;
            user.OtpExpireAt = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();
            var returnModel = new ForgotPasswordResponseDto();
            return CustomEntityResult<ForgotPasswordResponseDto>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<ForgotPasswordResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> UpdatePasswordAsync(Guid userId, string newPassword)
    {
        try
        {
            var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new UserDoesNotExitException(userId);
            }

            await _context.SaveChangesAsync();
            var returnModel = new BasedResponseModel();
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<CreateUserProfileResponseDto>> CreateUserProfileAsync(
        CreateUserProfileRequestDto dto)
    {
        try
        {
            var existingUser = await _context.TblUsers
                .Include(u => u.TblUserProfileImage)
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (existingUser == null)
                throw new UserNotFoundException(dto.UserId.ToString());

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                existingUser.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                existingUser.Address = dto.Address;

            if (!string.IsNullOrWhiteSpace(dto.Gender))
                existingUser.Gender = dto.Gender;

            if (dto.DateOfBirth.HasValue)
                existingUser.DateOfBirth = dto.DateOfBirth;

            if (dto.ProfileImg != null && dto.ProfileImg.Length > 0)
            {
                if (existingUser.TblUserProfileImage == null)
                {
                    var newImage = new TblUserProfileImage
                    {
                        UserId = existingUser.UserId,
                        ProfileImg = dto.ProfileImg,
                        ProfileImgMimeType = dto.ProfileImgMimeType
                    };
                    await _context.TblUserProfileImages.AddAsync(newImage);
                }
                else
                {
                    existingUser.TblUserProfileImage.ProfileImg = dto.ProfileImg;
                    existingUser.TblUserProfileImage.ProfileImgMimeType = dto.ProfileImgMimeType;
                }
            }

            var result = await _context.SaveChangesAsync();
            var response = new CreateUserProfileResponseDto();
            return CustomEntityResult<CreateUserProfileResponseDto>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserProfileResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<GetUserProfileByIdResponseDto>> GetUserProfileByIdAsync(
        GetUserProfileByIdRequestDto dto)
    {
        try
        {
            var user = await _context.TblUsers
                .Include(u => u.TblUserProfileImage)
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(dto.UserId.ToString());
            }

            var result = new GetUserProfileByIdResponseDto
            {
                UserName = user.UserName,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfileImg = user.TblUserProfileImage?.ProfileImg
            };

            return CustomEntityResult<GetUserProfileByIdResponseDto>.GenerateSuccessEntityResult(result);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<GetUserProfileByIdResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<SeedRoleToAdminResponseDto>> SeedRoleToAdmin(SeedRoleToAdminRequestDto dto)
    {
        try
        {
            var registerUserRequest = new TblUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };
            var hashedPassword = _passwordHasher.HashPassword(dto.Password);
            registerUserRequest.Password = hashedPassword;
            var Role = await _context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == RoleConstants.Admin);
            if (Role == null)
            {
                throw new RoleDoesNotExistException("User Role does not exit!");
            }

            registerUserRequest.RoleId = Role.RoleId;
            await _context.AddAsync(registerUserRequest);
            await _context.SaveChangesAsync();
            var result = new SeedRoleToAdminResponseDto();
            return CustomEntityResult<SeedRoleToAdminResponseDto>.GenerateSuccessEntityResult(result);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<SeedRoleToAdminResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<CreateUserProfileResponseDto>> CreateUserProfileByAdminAsync(
        CreateUserProfileByAdminRequestDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var role = await RoleExitAsync(RoleConstants.User);
            if (!role)
            {
                throw new RoleDoesNotExistException("User role does not exist.");
            }

            var roleid = await _context.TblRoles
                .Where(r => r.RoleName == RoleConstants.User)
                .Select(r => r.RoleId)
                .FirstOrDefaultAsync();
            var newUser = new TblUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = _passwordHasher.HashPassword(dto.Password),
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Gender = dto.Gender,
                RoleId = roleid
            };

            await _context.TblUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            if (dto.ProfileImg != null && dto.ProfileImg.Length > 0)
            {
                var userImage = new TblUserProfileImage
                {
                    UserId = newUser.UserId,
                    ProfileImg = dto.ProfileImg,
                    ProfileImgMimeType = dto.ProfileImgMimeType
                };

                await _context.TblUserProfileImages.AddAsync(userImage);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return CustomEntityResult<CreateUserProfileResponseDto>.GenerateSuccessEntityResult(
                new CreateUserProfileResponseDto
                {
                    RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS,
                    RespDescription = "User profile created successfully."
                });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return CustomEntityResult<CreateUserProfileResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                $"Failed to create user profile: {ex.Message} {(ex.InnerException?.Message ?? "")}");
        }
    }

    public async Task<CustomEntityResult<GetAllUserInforResponseDto>> GetAllUserInfoAsync()
    {
        try
        {
            var userList = await _context.TblUsers.Include(u => u.Role)
                .Select(u => new GetAllUSerInfoDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    RoleName = u.Role.RoleName,
                    Gender = u.Gender,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

            var response = new GetAllUserInforResponseDto
            {
                Users = userList
            };

            return CustomEntityResult<GetAllUserInforResponseDto>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<GetAllUserInforResponseDto>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}