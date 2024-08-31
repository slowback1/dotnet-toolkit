using Microsoft.EntityFrameworkCore;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.User;

public interface IUserRetriever
{
    Task<GetUser> GetUserById(string userId);
}

public class UserRetriever : BaseDatabaseAction, IUserRetriever
{
    public UserRetriever(SampleAppContext context) : base(context)
    {
    }

    public async Task<GetUser> GetUserById(string userId)
    {
        var user = await GetUserModelById(userId);

        if (user == null) return null;

        return user.ConvertToDto();
    }

    internal async Task<Core.Models.User?> GetUserModelById(string userId)
    {
        return await _context.Users
            .Where(u => u.Id.ToString() == userId)
            .FirstOrDefaultAsync();
    }
}