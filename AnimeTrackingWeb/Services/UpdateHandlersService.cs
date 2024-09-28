using AnimeTrackingApi;
using Hangfire;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace AnimeTrackingWeb.Services;

/// <summary>
/// Сервис обновления запросов клиента.
/// </summary>
public class UpdateHandlersService
{
    #region Поля и свойства

    /// <summary>
    /// Клиент телеграм-бота.
    /// </summary>
    private readonly ITelegramBotClient botClient;
    
    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<UpdateHandlersService> logger;

    /// <summary>
    /// Аниме-трекер.
    /// </summary>
    private IAnimeTracking animeTracking;

    #endregion

    #region Методы

    /// <summary>
    /// Обработать запрос клиента.
    /// </summary>
    /// <param name="update">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message }                       => BotOnMessageReceived(message, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(update)
        };

        await handler;
    }

    /// <summary>
    /// Обработать неизвестный запрос.
    /// </summary>
    /// <param name="update">Запрос.</param>
    /// <returns>Результат.</returns>
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        this.logger.LogInformation("Неизвестный тип: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Обработать запрос сообщения.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        try
        {
            // при наличии данных в WebAppData это запрос на создание джобов
            if (message.WebAppData?.Data is { } titleIdValues)
            {
                await this.CreateJobs(message, cancellationToken, titleIdValues);
            }

            var sentMessage = await (message.Text?.Split(' ')[0] switch
            {
                "/start" => this.CreateMiniApp(message),
                _ => this.Usage(message),
            });
            
            this.logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    /// <summary>
    /// Создать миниприложение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Созданное сообщение с миниприложением.</returns>
    private async Task<Message> CreateMiniApp(Message message)
    {
        var keyBoardButton = new KeyboardButton("Выбрать тайтлы")
        {
            WebApp = new WebAppInfo()
            {
                Url = $"https://192.168.0.103:5173/",
            }
        };
        var inlineMarkup = new ReplyKeyboardMarkup(keyBoardButton);
        return await this.botClient.SendTextMessageAsync(message.Chat.Id, @"Для выбора отслеживаемых тайтлов нажмите на кнопку «Выбрать тайтлы»", replyMarkup: inlineMarkup);
    }

    /// <summary>
    /// Создать джобы.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="titleIdValues">Идентификаторы выбранных тайтлов.</param>
    private async Task CreateJobs(Message message, CancellationToken cancellationToken, string titleIdValues)
    {
        var titleIds = titleIdValues.Split(",").Select(x => x.ParseInt());
        foreach (var id in titleIds)
        {
            var schedule = await this.animeTracking.GetSchedule(id);
            if (schedule?.airingSchedule?.edges == null)
            {
                continue;
            }
            
            foreach (var episodeInformation in schedule.airingSchedule.edges.Select(x => x.node))
            {
                var date = episodeInformation.getAiringAtUtc();
                var title = schedule.title.english ?? schedule.title.romaji ?? schedule.title.native;
                BackgroundJob.Schedule(
                    () => SendNotifications(message.Chat.Id, title,
                        episodeInformation.episode, cancellationToken), date);
            }
        }
    }
    
    /// <summary>
    /// Отправить уведомление.
    /// </summary>
    /// <param name="chatId">Идентификатор чата клиента.</param>
    /// <param name="title">Название тайтла.</param>
    /// <param name="episodeNum">Номер вышедшего эпизода.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public void SendNotifications(long chatId, string? title, int episodeNum, CancellationToken cancellationToken)
    {
        botClient.SendTextMessageAsync(chatId, text: $"{episodeNum} эпизод {title} вышел!", cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Обработать некорректное сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Созданное сообщение.</returns>
    private async Task<Message> Usage(Message message)
    {
        var usage = "<b><u>Меню бота</u></b>:\n" +
                    "/start             -   запустить бота";
        return await this.botClient.SendTextMessageAsync(message.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    #endregion

    #region Конструктор

    /// <summary>
    /// Сервис обновления запросов клиента.
    /// </summary>
    public UpdateHandlersService(ITelegramBotClient botClient, ILogger<UpdateHandlersService> logger, IAnimeTracking animeTracking)
    {
        this.botClient = botClient;
        this.logger = logger;
        this.animeTracking = animeTracking;
    }

    #endregion
}