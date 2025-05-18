using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem;

public class ServiceInjectionFactory
{
    public static void ServiceInject(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        //service
        builder.Services.AddTransient<IUserService, UserService>();
        
        //repository
        
        
        //helpers
        //builder.Services.AddTransient<IAuthorizationHelper, AuthorizationHelper>();
    }
}