using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelManagementSystem.Helpers
{
    public sealed class AutoCancelNoShowService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<AutoCancelNoShowService> _log;

        private readonly TimeSpan _cutoff = new(18, 0, 0);     
        private readonly TimeSpan _interval = TimeSpan.FromHours(1); 

        public AutoCancelNoShowService(IServiceProvider sp, ILogger<AutoCancelNoShowService> log)
        {
            _sp = sp;
            _log = log;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nowLocal = DateTime.Now;              
                if (nowLocal.TimeOfDay >= _cutoff)
                {
                    await ProcessTodaysNoShowsAsync(
                        DateOnly.FromDateTime(nowLocal),
                        stoppingToken);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ProcessTodaysNoShowsAsync(DateOnly today, CancellationToken stop)
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
            var noShows = await db.TblBookings
                                  .Include(b => b.TblRoomBookings)
                                      .ThenInclude(rb => rb.Room)
                                  .Where(b => b.BookingStatus == "Booked"
                                              && b.CheckInTime == today)
                                  .ToListAsync(stop);

            if (noShows.Count == 0) return;

            foreach (var booking in noShows)
            {
                booking.BookingStatus = "Cancelled";

                foreach (var rb in booking.TblRoomBookings)
                {
                    if (rb.Room.RoomStatus == "Occupied")
                        rb.Room.RoomStatus = "Available";
                }
            }

            await db.SaveChangesAsync(stop);

            _log.LogInformation(
                "Auto‑cancelled {Count} bookings and freed {RoomCount} rooms on {Date:d}.",
                noShows.Count,
                noShows.Sum(b => b.TblRoomBookings.Count),
                today);
        }
    }
}
