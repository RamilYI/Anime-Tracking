namespace AnimeTrackingApi.Dto;

public class CoverImage
{
    public string? large { get; set; }
}

public class Title
{
    // TODO усовершенствовать для поиска
    
    /// <summary>
    /// Найти тайтл по имени.
    /// </summary>
    /// <param name="titleName">Имя тайтла.</param>
    /// <returns>Признак наличия тайтла.</returns>
    public bool IsDesiredTitle(string titleName)
    {
        return this.CheckTitleByName(s => s.Equals(titleName));
    }

    /// <summary>
    /// Найти тайтл с одинаковым именем.
    /// </summary>
    /// <param name="titleName">Имя тайтла.</param>
    /// <returns>Признак наличия тайтла.</returns>
    public bool SearchTitleWithSimilarName(string titleName)
    {
        return this.CheckTitleByName(s => s.Contains(titleName, StringComparison.CurrentCulture)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.CurrentCultureIgnoreCase)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.InvariantCulture)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.InvariantCultureIgnoreCase)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.Ordinal)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Проверка тайтла по имени.
    /// </summary>
    /// <param name="pred">Предикат проверки.</param>
    /// <returns>Результат проверки.</returns>
    public bool CheckTitleByName(Predicate<string> pred)
    {
        return pred(english ?? string.Empty) || pred(romaji ?? string.Empty) || pred(native ?? string.Empty);
    }
    
    public string? english { get; set; }
    public string? romaji { get; set; }
    public string? native { get; set; }
}