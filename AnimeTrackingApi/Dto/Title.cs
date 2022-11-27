namespace AnimeTrackingApi.Dto;

public class Title
{
    // TODO усовершенствовать для поиска 
    public bool IsDesiredTitle(string titleName)
    {
        return titleName.Equals(english) || titleName.Equals(romaji) || titleName.Equals(native);
    }
    
    public string? english { get; set; }
    public string? romaji { get; set; }
    public string? native { get; set; }
}