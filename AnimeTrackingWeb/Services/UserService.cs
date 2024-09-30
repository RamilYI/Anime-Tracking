using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Models;

namespace AnimeTrackingWeb.Services;

/// <inheritdoc />
public class UserService : IUserService
{
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

    public void AddUserTitleIds(long chatId, ICollection<int> titleIds)
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
            }

            user.TitleIds.Clear();
            user.TitleIds.AddRange(titleIds);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
        }
    }

    public UserService(UserContext context, ILogger<IUserService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    private ILogger<IUserService> logger { get; set; }

    private UserContext context { get; set; }
}