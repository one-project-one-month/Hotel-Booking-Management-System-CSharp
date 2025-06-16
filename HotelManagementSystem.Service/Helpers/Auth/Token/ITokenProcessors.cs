using HotelManagementSystem.Data.Dtos.User;
namespace HotelManagementSystem.Service.Helpers.Auth.Token
{
    public interface ITokenProcessors
    {
        string GenerateOTPToken();
        string GenerateRefreshToken();
        void WriteTokenInHttpOnlyCookie(string cookieName, string token, DateTime expireTime);
        Task<CustomEntityResult<LoginResponseDto>> GenerateToken(LoginRequestDto user);
        Task<CustomEntityResult<RefreshTokenResponseDto>> GenerateTokenInMiddleWare(RefreshTokenRequestDto dto);
    }
}
