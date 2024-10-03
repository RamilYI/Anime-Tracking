namespace AnimeTrackingWeb;

/// <summary>
/// Когнфигурация бота.
/// </summary>
public class BotConfiguration
{
    public string BotToken { get; init; } = default!;
    
    public Uri BotWebAppUrl { get; init; } = default!;
    
    public Uri MiniAppUrl { get; init; } = default!;
    
    public string HangfireConnection { get; init; } = default!;
    
    public string UserTableConnection { get; init; } = default!;
}