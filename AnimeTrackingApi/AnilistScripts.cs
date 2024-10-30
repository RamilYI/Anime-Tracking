namespace AnimeTrackingApi;

/// <summary>
/// Скрипты к АПИ anilist.
/// </summary>
internal static class AnilistScripts
{
    /// <summary>
    /// Урл анилиста.
    /// </summary>
    public static readonly string Url = "https://graphql.anilist.co";
    
    /// <summary>
    /// Скрипт получения расписания.
    /// </summary>
    public static readonly string GetScheduleScript = "query ($id: Int) {\n  Media (id: $id, type: ANIME) {\n    id\n    airingSchedule{\n        edges\n        {\n            node\n            {\n                airingAt\n                timeUntilAiring\n                episode\n            }\n        }\n    }\n    title {\n      romaji\n      english\n      native\n    }\n  }\n}";

    /// <summary>
    /// Скрипт получения тайтла.
    /// </summary>
    public static readonly string FindTitleScript = "query ($id: Int, $page: Int, $perPage: Int, $search: String) {" +
                                                    "\n    Page (page: $page, perPage: $perPage) {" +
                                                    "\n        pageInfo {" +
                                                    "\n            total" +
                                                    "\n            currentPage" +
                                                    "\n            lastPage" +
                                                    "\n            hasNextPage" +
                                                    "\n            perPage" +
                                                    "\n        }" +
                                                    "\n        media (id: $id, search: $search, type: ANIME) {" +
                                                    "\n            id" +
                                                    "\n            title {" +
                                                    "\n                romaji" +
                                                    "\n                english" +
                                                    "\n                native" +
                                                    "\n            }" +
                                                    "\n        }" +
                                                    "\n    }" +
                                                    "\n}";
    
    /// <summary>
    /// Скрипт получения сезона.
    /// </summary>
    public static readonly string GetSeasonScript = "query ($season: MediaSeason!, $seasonYear: Int!, $page: Int) {" +
                                                    "\n  Page(page: $page) {" +
                                                    "\n    pageInfo {" +
                                                    "\n      total" +
                                                    "\n      perPage" +
                                                    "\n      currentPage" +
                                                    "\n      lastPage" +
                                                    "\n      hasNextPage" +
                                                    "\n    }" +
                                                    "\n    media(season: $season, seasonYear: $seasonYear, type: ANIME, sort: START_DATE) {" +
                                                    "\n      id" +
                                                    "\n      title {" +
                                                    "\n        romaji" +
                                                    "\n        english" +
                                                    "\n        native" +
                                                    "\n      }" +
                                                    "\n      startDate {" +
                                                    "\n        year" +
                                                    "\n        month" +
                                                    "\n        day" +
                                                    "\n      }" +
                                                    "\n      coverImage{\n        large\n      }" +
                                                    "\n    }" +
                                                    "\n  }" +
                                                    "\n}";

    /// <summary>
    /// Получить переменные скрипта поиска тайтла.
    /// </summary>
    /// <param name="title">Тайтл.</param>
    /// <param name="perPage">Количество записей страницы.</param>
    /// <returns>Переменные скрипта.</returns>
    public static string GetFindTitleScriptVariables(this string title, int perPage = 10)
    {
        return "{" +
               $"\n    'search': '{title}'," +
               "\n    'page': 1," +
               $"\n    'perPage': {perPage}" +
               "\n}";
    }

    public static string GetScheduleScriptVariables(int titleId)
    {
        var id = "{" +
               $"\n    \"id\": {titleId}" +
               "\n}";
        return id;
    }


    /// <summary>
    /// Получить сезон.
    /// </summary>
    /// <param name="currentDate">Дата.</param>
    /// <returns>Сезон.</returns>
    public static AnimeSeason GetSeason(DateTime currentDate)
    {
        if (currentDate.Month is 1 or < 3)
        {
            return AnimeSeason.WINTER;
        }

        if (currentDate.Month < 6)
        {
            return AnimeSeason.SPRING;
        }

        if (currentDate.Month < 9)
        {
            return AnimeSeason.SUMMER;
        }

        return AnimeSeason.FALL;
    }
    
    /// <summary>
    /// Получить переменные скрипта поиска сезона.
    /// </summary>
    /// <returns>Переменные скрипта.</returns>
    public static string GetSeasonScriptVariables(int pageNum)
    {
        var currentDate = DateTime.Now;
        return "{" +
               $"\n  \"season\": \"{GetSeason(currentDate).ToString()}\"," +
               $"\n  \"seasonYear\": {currentDate.Year}," +
               $"\n  \"page\": {pageNum}" +
               "\n}";
    }
}