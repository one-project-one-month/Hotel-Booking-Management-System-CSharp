using HotelManagementSystem.Data.Data.FeatureModels;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
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
            try
            {
                var accessToken = context.Request.Cookies["access_token"];
                var refreshToken = context.Request.Cookies["refresh_token"];

                if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
                {
                    if (!context.Request.Headers.ContainsKey("Authorization"))
                    {
                        context.Request.Headers.Append("Authorization", $"Bearer {accessToken}");
                    }

                    var handler = new JwtSecurityTokenHandler();

                    if (handler.CanReadToken(accessToken))
                    {
                        var jwtToken = handler.ReadJwtToken(accessToken);
                        var expUnix = long.Parse(jwtToken.Claims.First(x => x.Type == "exp").Value);
                        var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix);

                        var timeLeft = expTime - DateTimeOffset.UtcNow;

                        if (timeLeft < TimeSpan.FromMinutes(2))
                        {
                            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                            if (!Guid.TryParse(id, out var userId))
                                throw new InvalidUserIdException("Invalid user ID format.");

                            var user = await userRepo.GetUserById(userId);

                            if (user != null && user.RefreshToken == refreshToken && user.TokenExpireAt > DateTime.UtcNow)
                            {
                                var refreshModel = new RefreshTokenRequestDto { Id = userId };
                                await tokenProcessor.GenerateTokenInMiddleWare(refreshModel);

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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in JwtAutoRefreshMiddleware: {Message}", ex.Message);
            }

            await _next(context);
        }
    }
}
