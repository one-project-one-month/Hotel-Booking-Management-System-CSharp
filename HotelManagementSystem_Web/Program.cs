//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//namespace HotelManagementSystem_Web;

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var builder = WebAssemblyHostBuilder.CreateDefault(args);
//        builder.RootComponents.Add<App>("#app");
//        builder.RootComponents.Add<HeadOutlet>("head::after");

//        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//        await builder.Build().RunAsync();
//    }
//}


using HotelManagementSystem_Web;
using HotelManagementSystem_Web.DevCode;
using HotelManagementSystem_Web.Models.Room;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7161") });

builder.Services.AddScoped<SearchRoomState>();

builder.Services.AddScoped<GetRoomTypeNamesService>();
await builder.Build().RunAsync();
