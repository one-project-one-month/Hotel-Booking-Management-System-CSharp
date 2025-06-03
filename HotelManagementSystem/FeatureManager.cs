namespace HotelManagementSystem;
public static class FeatureManager
{
    public static void AddHotelManagementService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        //service
        //builder.Services.AddTransient<IUserRepository, UserRepository>();
        //builder.Services.AddTransient<IUserService, UserService>();

        //repository


        //helpers
        //builder.Services.AddTransient<IAuthorizationHelper, AuthorizationHelper>();
    }
}
