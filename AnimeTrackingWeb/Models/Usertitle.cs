using System.ComponentModel.DataAnnotations;

namespace AnimeTrackingWeb.Models;

/// <summary>
/// Таблица "Название тайтла, выбранного пользователем".
/// </summary>
public class Usertitle
{
    [Key]
    public int id {get; set;}
    
    public int userid { get; set; }
    
    public int titleid { get; set; }
    
    public List<string> jobids { get; set; }
}