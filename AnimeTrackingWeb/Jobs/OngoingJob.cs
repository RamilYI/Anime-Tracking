using Quartz;
using Telegram.Bot;

namespace AnimeTrackingWeb.Jobs;

[DisallowConcurrentExecution]
public class OngoingJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var telegramBotClient = (ITelegramBotClient)context.Trigger.JobDataMap["TelegramBot"];
        var titleName = context.Trigger.JobDataMap.GetString("TitleName");
        var messageChatId = context.Trigger.JobDataMap.GetLongValue("MessageChatId");
        // var cancellationToken = (CancellationToken)context.JobDetail.JobDataMap["CancellationToken"];
        if (messageChatId != default)
            await telegramBotClient.SendTextMessageAsync(messageChatId, text: $"The new episode of {titleName} is out!");
    }
}