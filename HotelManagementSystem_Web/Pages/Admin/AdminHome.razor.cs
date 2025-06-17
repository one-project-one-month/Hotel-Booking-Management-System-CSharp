using System.Globalization;
using HotelManagementSystem_Web.Models.Booking;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace HotelManagementSystem_Web.Pages.Admin;

public partial class AdminHome : ComponentBase
{
    private List<BookingModel> _bookingLst = new List<BookingModel>();
    private int[] MonthlySale;
    private string[] Month;
    public class MonthlySaleData
    {
        public int Month { get; set; }
        public decimal? Total { get; set; }
    }
    protected override async Task OnInitializedAsync()
    {
        await GetBookingList();
      await  GetMonthlySales();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setColumnChartRoomType");
            
            await JSRuntime.InvokeVoidAsync("setPieChartSourcesBooking");
            await JSRuntime.InvokeVoidAsync("setBarChartCount");
        }
    }

    public async Task GetBookingList()
    {
        var res = await _httpClient.GetAsync("admin/Bookings");
        if (res.IsSuccessStatusCode)
        {
            var jsonStr = await res.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr);
            var lst = JsonConvert.DeserializeObject<BookingListResponseModel>(jsonStr)!;
            _bookingLst = lst.Bookings;
        }
        else
        {
            Console.WriteLine(res.Content.ReadAsStringAsync());
        }
    }

    public async Task GetMonthlySales()
    {
        // Group and summarize sales by month number (1 = Jan, 12 = Dec)
        var monthlySales = _bookingLst
            .GroupBy(b => b.CreatedAt!.Value.Month)
            .OrderBy(g => g.Key)
            .Select(g => new MonthlySaleData
            {
                Month = g.Key,
                Total = g.Sum(b => b.TotalAmount)
            })
            .ToList();

        // Convert month number to short name (e.g., 1 => "Jan")
        Month = monthlySales
            .Select(m => CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(m.Month))
            .ToArray();

        // Get totals
        MonthlySale = monthlySales
            .Select(m => Convert.ToInt32(m.Total))
            .ToArray();
        await JSRuntime.InvokeVoidAsync("setLineChartSale", MonthlySale,Month);
    }
}