namespace AnimeTrackingApi.Dto;

public record SeasonDto
{
    public List<TitleInformation> media { get; set; } = new List<TitleInformation>();
}