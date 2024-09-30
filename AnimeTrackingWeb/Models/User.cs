using System.ComponentModel.DataAnnotations;

namespace AnimeTrackingWeb.Models;

/// <summary>
/// Таблица "Пользователь".
/// </summary>
public class User
{
    [Key]
    public int Id { get; set; }
    
    public long ChatId { get; set; }
    
    public List<int> TitleIds { get; set; }
    
}