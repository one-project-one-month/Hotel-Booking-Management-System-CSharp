using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Extensions;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using HotelManagementSystem.Service.Helpers.SMTP;
using HotelManagementSystem.Service.Reposities.Implementation;
using HotelManagementSystem.Service.Repositories.Implementation;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem;

public class ServiceInjectionFactory
{
    public static void ServiceInject(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var env = builder.Environment.EnvironmentName.ToLower();
        var envFile = $".env.{env}";
        if (File.Exists(envFile))
        {
            DotNetEnv.Env.Load(envFile);
        }
        else
        {
            DotNetEnv.Env.Load(); 
        }

        builder.Services.AddJwtServices();
        builder.Services.AddAuthorization();

        //service
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddScoped<IInvoicePdfService, InvoicePdfService>();

        //repository
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();


        //helpers
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddTransient<ITokenProcessors,TokenProcessor>();
        builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<ISmtpService, SmtpService>();
    }
}