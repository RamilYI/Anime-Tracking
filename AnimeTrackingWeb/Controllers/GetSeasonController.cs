using AnimeTrackingApi;
using AnimeTrackingApi.Dto;
using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTrackingWeb.Controllers;

/// <summary>
/// Контроллер получения текущего сезона.
/// </summary>
[ApiController]
[Route("/api/bot/getSeason")]
public class GetSeasonController : ControllerBase
{
    #region Методы

    /// <summary>
    /// Обработать GET запрос.
    /// </summary>
    /// <param name="animeTracking">Сервис уведомлятора.</param>
    /// <returns>Коллекция тайтлов сезона.</returns>
    [HttpGet]
    public async Task<IEnumerable<TitleInformation>> Get([FromServices]IAnimeTracking animeTracking)
    {
        var season = await animeTracking.GetSeason();
        return season.media;
    }

    #endregion
}