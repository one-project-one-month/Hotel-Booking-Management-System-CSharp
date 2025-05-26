using HotelManagementSystem.Data.Data.FeatureModels;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelManagementSystem.Service.Helpers.Auth.MiddleWare
{
    public class JwtAutoRefreshMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAutoRefreshMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenProcessors tokenProcessor, IUserRepository userRepo, Jwt jwtConfig)
        {
            var accessToken = context.Request.Cookies["access_token"];
            var refreshToken = context.Request.Cookies["refresh_token"];

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                var handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(accessToken))
                {
                    var jwtToken = handler.ReadJwtToken(accessToken);
                    var expUnix = long.Parse(jwtToken.Claims.First(x => x.Type == "exp").Value);
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix);

                    var timeLeft = expTime - DateTimeOffset.UtcNow;

                    if (timeLeft < TimeSpan.FromMinutes(2))
                    {
                        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        var user = await userRepo.GetUserByEmail(email);

                        if (user != null && user.RefreshToken == refreshToken && user.TokenExpireAt > DateTime.UtcNow)
                        {
                            var loginDto = new LoginRequestDto
                            {
                                Email = email,
                                Password = "" 
                            };

                            await tokenProcessor.GenerateToken(loginDto);

                            var newRefreshToken = tokenProcessor.GenerateRefreshToken();
                            var newExpire = DateTime.UtcNow.AddDays(7);
                            user.RefreshToken = newRefreshToken;
                            user.TokenExpireAt = newExpire;
                            await userRepo.UpdateTokenAsync(user);

                            tokenProcessor.WriteTokenInHttpOnlyCookie("refresh_token", newRefreshToken, newExpire);
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
