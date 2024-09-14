using System.Text;
using System.Web;
using AnimeTrackingApi;
using AnimeTrackingApi.Dto;
using AnimeTrackingWeb.Jobs;
using Quartz;
using Quartz.Impl;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace AnimeTrackingWeb.Services;

public class AnimeSeasonWebAppDto
{
    public int Id { get; set; }
    
    public string englishTitle { get; set; }
    
    public string nativeTitle { get; set; }
    
    public string romajiTitle { get; set; }
    
    public bool isEnabled { get; set; }
}

/// <summary>
/// Сервис обновления запросов клиента.
/// </summary>
public class UpdateHandlersService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlersService> _logger;
    private readonly AnimeTracking _animeTracking = new(); // переиграем как у того шведа (автор DI)
    
    public UpdateHandlersService(ITelegramBotClient botClient, ILogger<UpdateHandlersService> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(Update update, ISchedulerFactory schedulerFactory,
        CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            { Message: { } message }                       => BotOnMessageReceived(message,schedulerFactory, cancellationToken),
            // { EditedMessage: { } message }                 => BotOnMessageReceived(message, cancellationToken),
            // { CallbackQuery: { } callbackQuery }           => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            // { InlineQuery: { } inlineQuery }               => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            // { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task BotOnMessageReceived(Message message, ISchedulerFactory schedulerFactory,
        CancellationToken cancellationToken)
    {
        try
        {
            if (message.WebAppData?.Data is string idText)
            {
                var ids = idText.Split(",");
            }
            
            if (message.Text is not { } messageText)
                return;
            var words = messageText.Split(' ');
            if (words.ElementAtOrDefault(0) == null || words.ElementAtOrDefault(1) == null)
                return;
        
            var action = words[0] switch
            {
                "/findongoing" => FindOngoing(_botClient, message, cancellationToken, schedulerFactory, string.Join(' ', words.Skip(1)), _animeTracking),
                "/findongoing2" => CheckButtons(_botClient, message, _animeTracking),
                "/checkButtons" => CheckButtons(_botClient, message, _animeTracking),
            };

            static async Task<Message> FindOngoing(ITelegramBotClient telegramBotClient, Message message,
                CancellationToken cancellationToken, ISchedulerFactory factory, string titleName,
                AnimeTracking _animetracking)
            {
                var result = _animetracking.GetTitleSchedule(titleName).Result;
                if (result == null)
                {
                    return await telegramBotClient.SendTextMessageAsync(message.Chat.Id, text: $"doesnt find anything.", cancellationToken: cancellationToken);

                }
                var dates = result?.airingSchedule.edges.Select(x => x.node.getAiringAtUtc());
            
                var scheduler = await factory.GetScheduler();

                foreach (var date in dates)
                {
                    await scheduler.Start();
                    var job = JobBuilder.Create<OngoingJob>().Build();
                    job.JobDataMap["TelegramBot"] = telegramBotClient;
                    job.JobDataMap["TitleName"] = titleName;
                    job.JobDataMap["MessageChatId"] = message.Chat.Id;
                    job.JobDataMap["CancellationToken"] = cancellationToken;
                    var trigger = TriggerBuilder.Create().StartAt(date).Build();
                    await scheduler.ScheduleJob(job, trigger);
                }
            
                // just testing quartz
                /*var testJob = JobBuilder.Create<OngoingJob>().Build();
            var testTrigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(1)
                .RepeatForever()).Build();
            testTrigger.JobDataMap["TelegramBot"] = telegramBotClient;
            testTrigger.JobDataMap["TitleName"] = titleName;
            testTrigger.JobDataMap["MessageChatId"] = message.Chat.Id;
            testTrigger.JobDataMap["CancellationToken"] = cancellationToken;
            await scheduler.ScheduleJob(testJob, testTrigger);*/
            
                return await telegramBotClient.SendTextMessageAsync(message.Chat.Id, text: $"Excellent! Now you're subscribed to notifications of new {titleName} episodes.", cancellationToken: cancellationToken);
            }
        
            static async Task<Message> CheckButtons(ITelegramBotClient botClient, Message message,
                AnimeTracking animeTracking)
            {
                var keyBoardButton = new KeyboardButton("text")
                {
                    WebApp = new WebAppInfo()
                    {
                        Url = $"https://192.168.0.104:5173",
                    }
                };
                var inlineMarkup = new ReplyKeyboardMarkup(keyBoardButton);
                return await botClient.SendTextMessageAsync(message.Chat.Id, "check buttons", replyMarkup: inlineMarkup);
            }

            var sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static IList<AnimeSeasonWebAppDto> ConvertSeasonToWebAppDtos(SeasonDto season)
    {
        var webAppDtos = new List<AnimeSeasonWebAppDto>();

        foreach (var titleInformation in season.media)
        {
            webAppDtos.Add(new AnimeSeasonWebAppDto()
            {
                englishTitle = titleInformation.title.english,
                nativeTitle = titleInformation.title.native,
                romajiTitle = titleInformation.title.romaji,
                Id = titleInformation.id.HasValue ? titleInformation.id.Value : 0,
            });
        }

        return webAppDtos;
    }
}