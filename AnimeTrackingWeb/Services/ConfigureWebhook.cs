using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AnimeTrackingWeb.Services;

/// <summary>
/// Класс конфигурации вебхука.
/// </summary>
public class ConfigureWebhook : IHostedService
{
    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<ConfigureWebhook> logger;
    
    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    private readonly IServiceProvider serviceProvider;
    
    /// <summary>
    /// Конфигурация бота.
    /// </summary>
    private readonly IOptions<BotConfiguration> configs;

    #endregion

    #region Методы

    /// <summary>
    /// Запустить бота.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        logger.LogInformation("Setting webhook: {WebhookAddress}", this.configs.Value.BotWebAppUrl);
        await botClient.SetWebhookAsync(
            url: $"{this.configs.Value.BotWebAppUrl.ToString()}/api/bot",
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Остановить бота.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook on app shutdown
        logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }

    #endregion

    #region Конструктор

    /// <summary>
    /// Класс конфигурации вебхука.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    /// <param name="serviceProvider">Провайдер сервисов.</param>
    /// <param name="configs">Конфигурация бота.</param>
    public ConfigureWebhook(
        ILogger<ConfigureWebhook> logger,
        IServiceProvider serviceProvider,
        IOptions<BotConfiguration> configs)
    {
        this.configs = configs;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    #endregion
}