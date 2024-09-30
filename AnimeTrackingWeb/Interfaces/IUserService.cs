using AnimeTrackingWeb.Models;

namespace AnimeTrackingWeb.Interfaces;

/// <summary>
/// Сервис взаимодействия с БД.
/// </summary>
public interface IUserService
{
    ICollection<int> GetUserTitleIds(long chatId);
    
    bool CheckUserTitleId(long chatId, int titleId);

    void AddUserTitleIds(long chatId, ICollection<int> titleIds);
}