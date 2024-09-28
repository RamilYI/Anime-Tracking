namespace AnimeTrackingApi.Dto;

public record Schedule
{
    public List<EpisodeSchedule> edges { get; set; } = new List<EpisodeSchedule>();
}