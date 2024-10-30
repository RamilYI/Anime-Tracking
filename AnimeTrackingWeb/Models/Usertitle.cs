using System.ComponentModel.DataAnnotations;

namespace AnimeTrackingWeb.Models;

/// <summary>
/// Таблица "Название тайтла, выбранного пользователем".
/// </summary>
public class Usertitle
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Key]
    public int id {get; set;}
    
    /// <summary>
    /// Идентификатор таблицы пользователя.
    /// </summary>
    public int userid { get; set; }
    
    /// <summary>
    /// Идентификатор выбранного тайтла.
    /// </summary>
    public int titleid { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов джобов тайтла.
    /// </summary>
    public List<string> jobids { get; set; }
}