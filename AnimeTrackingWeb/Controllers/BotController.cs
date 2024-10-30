using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Services;
using Microsoft.AspNetCore.Cors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnimeTrackingWeb.Controllers;

/// <summary>
/// Контроллер бота.
/// </summary>
[ApiController]
[Route("/api/bot")]
public class BotController : ControllerBase
{
    #region Методы

    /// <summary>
    /// Обработать POST запрос.
    /// </summary>
    /// <param name="update">Запрос.</param>
    /// <param name="handleUpdateService">Сервис обработки запросов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат.</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update,
        [FromServices] UpdateHandlersService handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }

    #endregion
}