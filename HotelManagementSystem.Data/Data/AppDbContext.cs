using HotelManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<RoomBookingEntity> RoomBookings { get; set; }
    public DbSet<GuestEntity> Guests { get; set; }
    public DbSet<CouponEntity> Coupons { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<InvoiceEntity> Invoice { get; set; }
    public DbSet<RoomTypeEntity> RoomTypes { get; set; }
    public DbSet<CheckInOutEntity> CheckInOut { get; set; }
}