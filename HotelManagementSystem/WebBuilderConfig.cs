using HotelManagementSystem.Data.Data;
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
        //builder.Services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        //service
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();

        builder.Services.AddTransient<ISearchRoomService, SearchRoomService>();
        builder.Services.AddTransient<ISearchRoomRepository, SearchRoomRepository>();
     

        //repository


        //helpers
        //builder.Services.AddTransient<IAuthorizationHelper, AuthorizationHelper>();
    }
}