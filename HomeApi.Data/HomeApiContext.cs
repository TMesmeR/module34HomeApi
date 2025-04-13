using System.Collections.Immutable;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data;

public class HomeApiContext: DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Device> Devices { get; set; }

    public HomeApiContext(DbContextOptions<HomeApiContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().ToTable("Rooms");
        modelBuilder.Entity<Device>().ToTable("Devices");
    }
}