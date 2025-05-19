using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Reposities.Implementation;
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

        //service
        builder.Services.AddTransient<IUserService, UserService>();

        //repository
        builder.Services.AddTransient<IUserRepository, UserRepository>();

        //helpers
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}