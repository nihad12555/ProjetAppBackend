using AppBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Salle> Salles { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}