using HotelManagement.DbService.Entities;
using Microsoft.EntityFrameworkCore;
namespace HotelManagement.DbService.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
}
