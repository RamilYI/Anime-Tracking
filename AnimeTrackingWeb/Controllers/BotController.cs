using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnimeTrackingWeb.Services;
using Microsoft.AspNetCore.Cors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnimeTrackingWeb.Controllers;

[ApiController]
[Route("/api/bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update,
        [FromServices] UpdateHandlersService handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}