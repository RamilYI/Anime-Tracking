using AnimeTrackingApi.Dto;

namespace AnimeTrackingApi;

public interface IAnimeTracking
{
    /// <summary>
    /// Получить расписание тайтла.
    /// </summary>
    /// <param name="title">Имя тайтла.</param>
    /// <returns>Расписание.</returns>
    Task<ScheduleDto?> GetTitleSchedule(string title);

    /// <summary>
    /// Получить сезон.
    /// </summary>
    /// <returns>Сезон.</returns>
    Task<SeasonDto> GetSeason();

    /// <summary>
    /// Получить расписание тайтла.
    /// </summary>
    /// <param name="findTitleId">Идентификатор тайтла.</param>
    /// <returns>Расписание.</returns>
    Task<ScheduleDto?> GetSchedule(int findTitleId);
}