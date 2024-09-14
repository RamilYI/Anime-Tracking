using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnimeTrackingApi;
using AnimeTrackingApi.Dto;
using AnimeTrackingWeb.Services;
using Microsoft.AspNetCore.Cors;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnimeTrackingWeb.Controllers;

[ApiController]
[Route("/api/bot")]
public class BotController : ControllerBase
{
    [HttpPost]
    // [ValidateTelegramBot]
    public async Task<IActionResult> Post([FromBody] Update update,
        [FromServices] UpdateHandlersService handleUpdateService,
        [FromServices] ISchedulerFactory schedulerFactory,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, schedulerFactory, cancellationToken);
        return Ok();
    }
    
    [HttpGet]
    public IEnumerable<Test> Get([FromBody] Update update,
        [FromServices] UpdateHandlersService handleUpdateService,
        [FromServices] ISchedulerFactory schedulerFactory)
    {
        return Enumerable.Range(1, 5).Select(index => new Test
            {
                TestVal = Random.Shared.Next(-20, 55),
            })
            .ToArray();
    }
}

public class Test
{
    public double TestVal { get; set; }
}

[ApiController]
[Route("/api/bot/test")]
public class GetBotController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TitleInformation>> Get([FromServices]AnimeTracking animeTracking)
    {
        var season = await animeTracking.GetSeason();
        return season.media;
        
        
    }
}
