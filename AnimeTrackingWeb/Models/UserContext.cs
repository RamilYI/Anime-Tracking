using Microsoft.EntityFrameworkCore;

namespace AnimeTrackingWeb.Models;

/// <inheritdoc />
public class UserContext : DbContext
{
    /// <summary>
    /// Таблица "Пользователь".
    /// </summary>
    public DbSet<User> Users { get; set; }

    // /// <inheritdoc />
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         @"Server=(localdb)\mssqllocaldb;Database=AnimeTrackingUsers;Trusted_Connection=True;ConnectRetryCount=0");
    // }

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        
    }
}