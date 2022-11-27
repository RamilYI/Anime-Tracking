namespace AnimeTrackingApi.Dto;

public record ScheduleDto
{
    public int id { get; set; }
    public Schedule airingSchedule { get; set; }
    public Title title { get; set; }
}

public record Schedule
{
    public List<EpisodeSchedule> edges { get; set; } = new List<EpisodeSchedule>();
}

public record EpisodeSchedule
{
    public EpisodeInformation node { get; set; }
}

public record EpisodeInformation
{
    public long airingAt { get; set; }
    public long timeUntilAiring { get; set; }
    public int episode { get; set; }

    public DateTime getAiringAtUtc()

    
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( airingAt ).ToUniversalTime();
        return dateTime;
    }
}