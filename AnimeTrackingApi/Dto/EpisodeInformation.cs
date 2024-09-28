namespace AnimeTrackingApi.Dto;

/// <summary>
/// Информация об эпизоде.
/// </summary>
public record EpisodeInformation
{
    public long airingAt { get; set; }
    public long timeUntilAiring { get; set; }
    public int episode { get; set; }

    /// <summary>
    /// Получить дату выхода эпизода.
    /// </summary>
    /// <returns></returns>
    public DateTime getAiringAtUtc()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( airingAt ).ToUniversalTime();
        return dateTime;
    }
}