using AnimeTrackingApi.Dto;

namespace AnimeTrackingApi;

public interface IAnimeTracking
{
    Task<ScheduleDto?> GetTitleSchedule(string title);
}