namespace AnimeTrackingWeb;

/// <summary>
/// Когнфигурация бота.
/// </summary>
public class BotConfiguration
{
    /// <summary>
    /// Токен.
    /// </summary>
    public string BotToken { get; init; } = default!;
    
    /// <summary>
    /// Адрес бота.
    /// </summary>
    public Uri BotWebAppUrl { get; init; } = default!;
    
    /// <summary>
    /// Адрес миниприложения.
    /// </summary>
    public Uri MiniAppUrl { get; init; } = default!;
    
    /// <summary>
    /// Строка подключения к БД джобов.
    /// </summary>
    public string HangfireConnection { get; init; } = default!;
    
    /// <summary>
    /// Строка подключения к БД пользователей.
    /// </summary>
    public string UserTableConnection { get; init; } = default!;
}