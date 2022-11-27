using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApp.Controllers;

[ApiController]
[Route("api/bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        var client = new TelegramBotClient("5783182392:AAH9leHkrp7jbk2SM6pY40aOUHkgkEUDxuA");

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            await client.SendTextMessageAsync(update.Message.From.Id, "answer");
        }

        return Ok();
    }
}