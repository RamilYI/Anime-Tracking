using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimeTrackingWeb.Models;

/// <inheritdoc />
public class UserContext : DbContext
{
    /// <summary>
    /// Конфигурация бота.
    /// </summary>
    private readonly IOptions<BotConfiguration> configuration;

    /// <summary>
    /// Таблица "Пользователь".
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options, IOptions<BotConfiguration> configuration)
        : base(options)
    {
        this.configuration = configuration;
    }
}