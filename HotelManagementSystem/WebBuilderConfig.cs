using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Service.Extensions;
using HotelManagementSystem.Service.Helpers.Auth.PasswordHash;
using HotelManagementSystem.Service.Helpers.Auth.SMTP;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using HotelManagementSystem.Service.Reposities.Implementation;
using HotelManagementSystem.Service.Repositories.Implementation;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

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
        builder.Services.AddTransient<IRoomService, RoomService>();
        builder.Services.AddTransient<IBookingControlService, BookingControlService>();
        builder.Services.AddTransient<ICheckInAndCheckoutService, CheckInAndCheckoutService>();
        builder.Services.AddTransient<ISearchRoomRepository, SearchRoomRepository>();
        builder.Services.AddTransient<IBookingService, BookingService>();
        builder.Services.AddTransient<IRoomTypeService, RoomTypeService>();
        builder.Services.AddTransient<IGuestService, GuestService>();
        builder.Services.AddTransient<IFeatureRoomService, FeatureRoomService>();
        builder.Services.AddScoped<IInvoicePdfService, InvoicePdfService>();

        //repository
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IRoomRepository, RoomRepository>();
        builder.Services.AddTransient<ISearchRoomService, SearchRoomService>();
        builder.Services.AddTransient<IBookingRepository, BookingRepository>();
        builder.Services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
        builder.Services.AddTransient<IGuestRepository, GuestRepository>();
        builder.Services.AddTransient<IFeatureRoomRepository, FeatureRoomRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
      
        //License
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorFrontend", policy =>
            {
                policy.WithOrigins("https://localhost:7144")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        builder.Services.AddTransient<IBookingControlRepository, BookingControlRepository>();
        builder.Services.AddTransient<ICheckInAndCheckoutRepository, CheckInAndCheckoutRepository>();

        //helpers
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddTransient<ITokenProcessors,TokenProcessor>();
        builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<ISmtpService, SmtpService>();
        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
    }
}