namespace AnimeTrackingApi.Dto;

public record ScheduleDto
{
    public int id { get; set; }
    public Schedule airingSchedule { get; set; }
    public Title title { get; set; }
}