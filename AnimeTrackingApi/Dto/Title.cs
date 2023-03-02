namespace AnimeTrackingApi.Dto;

public class Title
{
    // TODO усовершенствовать для поиска 
    public bool IsDesiredTitle(string titleName)
    {
        return this.CheckTitleByName(s => s.Equals(titleName));
    }

    public bool SearchTitleWithSimilarName(string titleName)
    {
        return this.CheckTitleByName(s => s.Contains(titleName, StringComparison.CurrentCulture)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.CurrentCultureIgnoreCase)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.InvariantCulture)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.InvariantCultureIgnoreCase)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.Ordinal)) ||
               this.CheckTitleByName(s => s.Contains(titleName, StringComparison.OrdinalIgnoreCase));
    }

    public bool CheckTitleByName(Predicate<string> pred)
    {
        return pred(english ?? string.Empty) || pred(romaji ?? string.Empty) || pred(native ?? string.Empty);
    }
    
    public string? english { get; set; }
    public string? romaji { get; set; }
    public string? native { get; set; }
}