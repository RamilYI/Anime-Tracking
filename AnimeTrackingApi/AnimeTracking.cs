using System.Text;
using Newtonsoft.Json;
using AnimeTrackingApi.Dto;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AnimeTrackingApi;

/// <inheritdoc />
public class AnimeTracking : IAnimeTracking
{
    #region IAnimeTracking

    /// <summary>
    /// Текущий сезон.
    /// </summary>
    private (AnimeSeason season, string date) currentSeason;
    private SeasonDto season;

    /// <inheritdoc />
    public async Task<ScheduleDto?> GetTitleSchedule(string title)
    {
        var season = await this.GetSeason();
        var findTitle = season.media.FirstOrDefault(x => x.title != null && x.title.IsDesiredTitle(title));
        if (findTitle is {id: not null})
        {
            var schedule = await this.GetSchedule(findTitle.id.Value);
            return schedule;
        }

        findTitle = season.media.FirstOrDefault(x => x.title != null && x.title.SearchTitleWithSimilarName(title));
        if (findTitle is {id: not null})
        {
            var schedule = await this.GetSchedule(findTitle.id.Value);
            return schedule;
        }

        return null;
    }
    
    /// <inheritdoc />
    public async Task<ScheduleDto?> GetSchedule(int findTitleId)
    {
        var client = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AnilistObject
        {
            query = AnilistScripts.GetScheduleScript,
            variables = AnilistScripts.GetScheduleScriptVariables(findTitleId)
        }), Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync(AnilistScripts.Url, content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var formattedResult = JObject.Parse(jsonResult)?["data"]?["Media"]?.ToString();
            if (formattedResult != null)
            {
                var scheduleDto = JsonSerializer.Deserialize<ScheduleDto>(formattedResult);
                return scheduleDto;
            }
        }
        catch (Exception ex)
        {
            // ignored
        }

        return new ScheduleDto();
    }

    /// <inheritdoc />
    public async Task<SeasonDto> GetSeason()
    {
        // возможно будет
        var newCurrentSeason = this.GetCurrentSeason(DateTime.Now);
        if (newCurrentSeason == this.currentSeason && this.season != null)
        {
            return this.season;
        }
        else
        {
            this.currentSeason = newCurrentSeason;
        }

        var client = new HttpClient();
        this.season = new SeasonDto();
        var hasPage = true;
        var pageNum = 1;
        while (hasPage)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new AnilistObject
            {
                query = AnilistScripts.GetSeasonScript,
                variables = AnilistScripts.GetSeasonScriptVariables(pageNum++)
            }), Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(AnilistScripts.Url, content);
                var jsonResult = await response.Content.ReadAsStringAsync();
                var hasPageStr = JObject.Parse(jsonResult)["data"]?["Page"]?["pageInfo"]?["hasNextPage"]?.ToString();
                if (string.IsNullOrEmpty(hasPageStr))
                {
                    continue;
                }

                hasPage = Convert.ToBoolean(hasPageStr);
                var formattedResult = JObject.Parse(jsonResult)["data"]?["Page"]?.ToString();
                if (formattedResult != null)
                {
                    var seasonDto = JsonSerializer.Deserialize<SeasonDto>(formattedResult);
                    if (seasonDto?.media != null)
                    {
                        this.season.media.AddRange(seasonDto.media);
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        return this.season;
    }

    /// <summary>
    /// Получить текущий сезон.
    /// </summary>
    /// <param name="currentDate">Текущая дата.</param>
    /// <returns>Текущий сезон.</returns>
    private (AnimeSeason season, string date) GetCurrentSeason(DateTime currentDate)
    {
        var currentDateText = currentDate.ToString("MMMM dd, yyyy");
        return (AnilistScripts.GetSeason(currentDate), currentDateText);
    }

    #endregion

    #region Конструктор

    /// <summary>
    /// Аниме-трекер.
    /// </summary>
    public AnimeTracking()
    {
        this.currentSeason = this.GetCurrentSeason(DateTime.Now);
    }

    #endregion
}