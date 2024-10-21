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

            foreach (var titleId in titleIds)
            {
                var userTitle = new Usertitle()
                {
                    userid = user.Id,
                    titleid = titleId,
                };
                
                userTitle.jobids = new List<string>();
                userTitle.jobids.AddRange(jobIds[titleId]);
                context.Usertitles.Add(userTitle);
            }
            
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex.Message);
        }
    }

    public ICollection<string> GetDeletedUserTitles(long chatId)
    {
        var user = this.GetUser(chatId);
        if (user == null)
        {
            return Enumerable.Empty<string>().ToList();
        }
        
        var titleIds = user.TitleIds;
        var deletedUserTitles = context.Usertitles.Where(ut => ut.userid == user.Id && !titleIds.Contains(ut.titleid)).ToList();
        context.Usertitles.RemoveRange(deletedUserTitles);
        context.SaveChanges();
        return deletedUserTitles.SelectMany(t => t.jobids).ToList();
    }

    public UserService(UserContext context, ILogger<IUserService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    private ILogger<IUserService> logger { get; set; }

    private UserContext context { get; set; }
}