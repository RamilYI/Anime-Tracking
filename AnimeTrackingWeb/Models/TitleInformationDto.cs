using AnimeTrackingApi.Dto;

namespace AnimeTrackingWeb.Models;

public record TitleInformationDto
{
    public int? id { get; set; }
    public Title? title { get; set; }
    public CustomDate? startDate { get; set; }
    
    public CoverImage? coverImage { get; set; }
    
    public bool isEnabled { get; set; }
}