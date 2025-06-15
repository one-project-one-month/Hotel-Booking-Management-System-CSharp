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


using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HotelManagementSystem_Web;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7161") });

await builder.Build().RunAsync();
