using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.User;

public class UserCreator : BaseDatabaseAction
{
    public UserCreator(SampleAppContext context) : base(context)
    {
    }

    public async Task<string> CreateUser(CreateUser dto)
    {
        var user = BuildUserModel(dto);
        await AddAndSaveUser(user);
        BroadcastUserCreatedEvent(user.Id);

        return user.Id.ToString();
    }

    private Core.Models.User BuildUserModel(CreateUser dto)
    {
        var id = GenerateGuid();

        var user = dto.ConvertToModel(id);
        return user;
    }

    private async Task AddAndSaveUser(Core.Models.User user)
    {
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    private Guid GenerateGuid()
    {
        return Guid.NewGuid();
    }

    private Guid BroadcastUserCreatedEvent(Guid id)
    {
        MessageBus.Publish(Messages.UserCreated, id.ToString());

        return id;
    }
}