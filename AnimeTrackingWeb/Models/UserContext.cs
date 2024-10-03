using Microsoft.EntityFrameworkCore;

namespace AnimeTrackingWeb.Models;

/// <inheritdoc />
public class UserContext : DbContext
{
    /// <summary>
    /// Таблица "Пользователь".
    /// </summary>
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        
    }
}