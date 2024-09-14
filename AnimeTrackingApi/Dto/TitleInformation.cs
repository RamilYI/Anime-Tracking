namespace AnimeTrackingApi.Dto;

public record TitleInformation
{
    public int? id { get; set; }
    public Title? title { get; set; }
    public CustomDate? startDate { get; set; }
    
    public CoverImage? coverImage { get; set; }
}