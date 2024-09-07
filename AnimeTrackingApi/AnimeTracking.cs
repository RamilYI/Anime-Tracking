using System.Text;
using Newtonsoft.Json;
using AnimeTrackingApi.Dto;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AnimeTrackingApi;

public class AnimeTracking : IAnimeTracking
{
    public async Task<ScheduleDto?> GetTitleSchedule(string title)
    {
        var season = await this.GetSeason();
        var findTitle = season.media.FirstOrDefault(x => x.title.IsDesiredTitle(title));
        if (findTitle != null)
        {
            var schedule = await this.GetSchedule(findTitle.id.Value);
            return schedule;
        }

        findTitle = season.media.FirstOrDefault(x => x.title.SearchTitleWithSimilarName(title));
        if (findTitle != null)
        {
            var schedule = await this.GetSchedule(findTitle.id.Value);
            return schedule;
        }

        return null;
    }

    private async Task<ScheduleDto?> GetSchedule(int findTitleId)
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
            if (response != null)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var formattedResult = JObject.Parse(jsonResult)["data"]?["Media"]?.ToString();
                var scheduleDto = JsonSerializer.Deserialize<ScheduleDto>(formattedResult);
                return scheduleDto;
            }
        }
        catch (Exception ex)
        {
            
        }

        return new ScheduleDto();
    }

    private async Task<SeasonDto> GetSeason()
    {
        var client = new HttpClient();
        var season = new SeasonDto();
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
                if (response != null)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    hasPage = Convert.ToBoolean(JObject.Parse(jsonResult)["data"]?["Page"]?["pageInfo"]?["hasNextPage"].ToString());
                    var formattedResult = JObject.Parse(jsonResult)["data"]?["Page"]?.ToString();
                    var seasonDto = JsonSerializer.Deserialize<SeasonDto>(formattedResult);
                    season.media.AddRange(seasonDto.media);
                }
            }
            catch (Exception ex)
            {
            
            }
        }

        return season;
    }
}