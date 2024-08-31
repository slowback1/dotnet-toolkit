using Slowback.Common;
using Slowback.Mapper;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.User;

public interface IUserEditor
{
    Task UpdateUser(EditUser dto);
}

public class UserEditor : BaseDatabaseAction, IUserEditor
{
    public UserEditor(SampleAppContext context) : base(context)
    {
    }

    public async Task UpdateUser(EditUser dto)
    {
        var user = await GetUserById(dto.Id);

        if (user == null) return;

        dto.ConvertToModel().Map(user);

        await _context.SaveChangesAsync();

        BroadcastMessage(user.Id);
    }

    private void BroadcastMessage(Guid id)
    {
        MessageBus.Publish(Messages.UserUpdated, id.ToString());
    }

    private async Task<Core.Models.User?> GetUserById(string userId)
    {
        return await new UserRetriever(_context)
            .GetUserModelById(userId);
    }
}