using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Models;

namespace AnimeTrackingWeb.Services;

/// <inheritdoc />
public class UserService : IUserService
{
    /// <summary>
    /// Логгер.
    /// </summary>
    private ILogger<IUserService> logger { get; set; }

    /// <summary>
    /// Контекст взаимодействия с БД.
    /// </summary>
    private UserContext context { get; set; }

    #region IUserService

    /// <summary>
    /// Получить юзера.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    private User GetUser(long chatId)
    {
        try
        {
            return this.context?.Users?.FirstOrDefault(u => u.ChatId == chatId);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
            return null;
        }
    }

    /// <inheritdoc />
    public ICollection<int> GetUserTitleIds(long chatId)
    {
        try
        {
            var user = this.GetUser(chatId);
            if (user == null)
            {
                return Enumerable.Empty<int>().ToList();
            }

            return user.TitleIds;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
            return Enumerable.Empty<int>().ToList();
        }
    }

    /// <inheritdoc />
    public bool CheckUserTitleId(long chatId, int titleId)
    {
        try
        {
            var user = this.GetUser(chatId);
            if (user == null)
            {
                return false;
            }

            return user.TitleIds.Contains(titleId);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
            return false;
        }
    }

    /// <inheritdoc />
    public void AddUserTitleIds(long chatId, ICollection<int> titleIds, IDictionary<int, ICollection<string>> jobIds)
    {
        try
        {
            var user = this.GetUser(chatId);
            if (user == null)
            {
                user = new User()
                {
                    ChatId = chatId,
                };
            
                context.Users.Add(user);
                user.TitleIds = new List<int>();
            }

            user.TitleIds.Clear();
            user.TitleIds.AddRange(titleIds);
            this.FillUserTitles(titleIds, jobIds, user);
            
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
        }
    }

    /// <summary>
    /// Создать строки таблицы UserTitle. 
    /// </summary>
    /// <param name="titleIds">Коллекция выбранных тайтлов.</param>
    /// <param name="jobIds">Коллекция созданных джобов.</param>
    /// <param name="user">Пользователь.</param>
    private void FillUserTitles(ICollection<int> titleIds, IDictionary<int, ICollection<string>> jobIds, User user)
    {
        foreach (var titleId in titleIds)
        {
            var userTitle = new Usertitle()
            {
                userid = user.Id,
                titleid = titleId,
            };
                
            userTitle.jobids = new List<string>();
            userTitle.jobids.AddRange(jobIds[titleId]);
            context?.usertitles?.Add(userTitle);
        }
    }

    /// <inheritdoc />
    public ICollection<string> GetDeletedUserTitles(long chatId)
    {
        var user = this.GetUser(chatId);
        if (user == null)
        {
            return Enumerable.Empty<string>().ToList();
        }
        
        var titleIds = user.TitleIds;
        var deletedUserTitles = context.usertitles.Where(ut => ut.userid == user.Id && !titleIds.Contains(ut.titleid)).ToList();
        context.usertitles.RemoveRange(deletedUserTitles);
        context.SaveChanges();
        return deletedUserTitles.SelectMany(t => t.jobids).ToList();
    }

    #endregion

    #region Конструктор

    /// <summary>
    /// Сервис взаимодействия с БД.
    /// </summary>
    /// <param name="context">Контекст данных для взаимодействия с БД.</param>
    /// <param name="logger">Логгер.</param>
    public UserService(UserContext context, ILogger<IUserService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    #endregion
}