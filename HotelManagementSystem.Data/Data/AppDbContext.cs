using HotelManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Data.Data;

public class AppDbContext : DbContext // THIS IS ESSENTIAL
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
}