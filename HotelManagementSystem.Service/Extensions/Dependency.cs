using HotelManagementSystem.Data.Data.FeatureModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelManagementSystem.Service.Extensions
{
    public static class Dependency
    {
        public static IServiceCollection AddJwtServices(this IServiceCollection services)
        {
            //JWT values from environment
            var jwtOptions = new Jwt
            {
                Secret = Environment.GetEnvironmentVariable("JWT_SECRET"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                ExpireTime = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRE_TIME"), out var expire) ? expire : 15
            };

            if (string.IsNullOrEmpty(jwtOptions.Secret))
                throw new ArgumentException("JWT_SECRET is missing in environment variables.");

            services.AddSingleton(jwtOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.TryGetValue("access_token", out var token) && !string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            //SMTP info from environment
            var smtpSettings = new Smtp
            {
                SmtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? throw new ArgumentException("SMTP_EMAIL is missing"),
                SmtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new ArgumentException("SMTP_PASSWORD is missing")
            };

            services.AddSingleton(smtpSettings);

            return services;
        }
    }
}
