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
        var id = GenerateGuid();

        var user = new Core.Models.User
        {
            Id = id,
            Name = dto.Name
        };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return id.ToString();
    }

    private Guid GenerateGuid()
    {
        return Guid.NewGuid();
    }
}