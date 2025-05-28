using HotelManagementSystem;
using HotelManagementSystem.Service.Helpers.Auth.MiddleWare;
using HotelManagementSystem.Service.Reposities.Implementation;
using HotelManagementSystem.Service.Repositories.Implementation;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.OpenApi.Models;
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("https://localhost:7144",
                "http://localhost:5053").AllowAnyMethod().AllowAnyHeader();
        });
});
// Add services to the container.
ServiceInjectionFactory.ServiceInject(builder);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<ISearchRoomService, SearchRoomService>();
builder.Services.AddTransient<ISearchRoomRepository, SearchRoomRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseMiddleware<JwtAutoRefreshMiddleware>();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();