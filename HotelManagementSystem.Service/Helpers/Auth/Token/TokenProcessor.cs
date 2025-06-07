using HotelManagementSystem.Data.Data.FeatureModels;
using HotelManagementSystem.Data.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelManagementSystem.Service.Helpers.Auth.Token
{
    public class TokenProcessor : ITokenProcessors
    {
        private readonly Jwt _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public TokenProcessor(Jwt jwt, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _jwt = jwt;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<CustomEntityResult<LoginResponseDto>> GenerateToken(LoginRequestDto user)
        {
            try
            {
                var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
                var credential = new SigningCredentials(
                    signingkey,
                    SecurityAlgorithms.HmacSha256);
                var existingUser = await _userRepository.GetUserByEmail(user.Email);
                var userId = existingUser.UserId;
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
                var userRole = await _userRepository.GetUserRolebyIdAsync(userId);

                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireTime);
                var token = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credential
                );
                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
                WriteTokenInHttpOnlyCookie("access_token", tokenHandler, expires);
                var result = new LoginResponseDto();
                return CustomEntityResult<LoginResponseDto>.GenerateSuccessEntityResult(result);
            }
            catch(Exception ex)
            {
                return CustomEntityResult<LoginResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }

        public async Task<CustomEntityResult<RefreshTokenResponseDto>> GenerateTokenInMiddleWare(RefreshTokenRequestDto dto)
        {
            try
            {
                var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
                var credential = new SigningCredentials(
                    signingkey,
                    SecurityAlgorithms.HmacSha256);
                var existingUser = await _userRepository.GetUserById(dto.Id);
                var userId = existingUser.UserId;
                var userEmail = existingUser.Email;
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, userEmail),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                };
                var userRole = await _userRepository.GetUserRolebyIdAsync(userId);

                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireTime);
                var token = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credential
                );
                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
                WriteTokenInHttpOnlyCookie("access_token", tokenHandler, expires);
                var result = new RefreshTokenResponseDto();
                return CustomEntityResult<RefreshTokenResponseDto>.GenerateSuccessEntityResult(result);
            }
            catch(Exception ex)
            {
                return CustomEntityResult<RefreshTokenResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
            }
        }
        public void WriteTokenInHttpOnlyCookie(string cookieName, string token, DateTime expireTime)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieName, token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = expireTime,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict
                });
        }

        public string GenerateOTPToken()
        {
            int code = RandomNumberGenerator.GetInt32(100_000, 1_000_000);
            return code.ToString("D6");
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
    }
}
