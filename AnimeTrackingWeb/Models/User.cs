using System.ComponentModel.DataAnnotations;

namespace AnimeTrackingWeb.Models;

/// <summary>
/// Таблица "Пользователь".
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Идентификатор чата пользователя.
    /// </summary>
    public long ChatId { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов выбранных тайтлов.
    /// </summary>
    public List<int> TitleIds { get; set; }
}