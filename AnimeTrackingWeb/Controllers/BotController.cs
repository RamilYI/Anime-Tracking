using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnimeTrackingWeb.Controllers;

[ApiController]
[Route("/api/bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        var client = new TelegramBotClient("{TOKEN}");

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            await client.SendTextMessageAsync(update.Message.From.Id, "answer");
        }

        return Ok();
    }
}