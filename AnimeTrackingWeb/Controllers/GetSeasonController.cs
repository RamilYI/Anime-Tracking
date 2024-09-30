using AnimeTrackingApi;
using AnimeTrackingApi.Dto;
using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeTrackingWeb.Controllers;

[ApiController]
[Route("/api/bot/getSeason")]
public class GetSeasonController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TitleInformation>> Get([FromServices]IAnimeTracking animeTracking)
    {
        var season = await animeTracking.GetSeason();
        return season.media;
    }
}