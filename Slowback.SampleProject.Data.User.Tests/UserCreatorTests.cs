using Microsoft.EntityFrameworkCore;
using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.User.Tests;

public class UserCreatorTests : BaseDbTest
{
    [Test]
    public async Task CanCreateAUser()
    {
        var creator = new UserCreator(_context);

        var dto = new CreateUser
        {
            Name = "Test User"
        };

        var result = await creator.CreateUser(dto);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task ReturnsAValidGuid()
    {
        var creator = new UserCreator(_context);

        var dto = new CreateUser
        {
            Name = "Test User"
        };

        var result = await creator.CreateUser(dto);

        Assert.That(Guid.TryParse(result, out _), Is.True);
    }

    [Test]
    [Repeat(100)]
    public async Task ReturnsAUniqueGuid()
    {
        var creator = new UserCreator(_context);

        var dto = new CreateUser
        {
            Name = "Test User"
        };

        var result1 = await creator.CreateUser(dto);
        var result2 = await creator.CreateUser(dto);

        Assert.That(result1, Is.Not.EqualTo(result2));
    }

    [Test]
    public async Task StoresTheCreatedUserInTheDatabase()
    {
        var creator = new UserCreator(_context);

        var dto = new CreateUser
        {
            Name = "Test User"
        };

        var result = await creator.CreateUser(dto);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == result);

        Assert.That(user, Is.Not.Null);
        Assert.That(user.Name, Is.EqualTo(dto.Name));
    }

    [Test]
    public async Task CreatingAUserSendsTheNewIdToTheMessageBus()
    {
        var creator = new UserCreator(_context);

        var dto = new CreateUser
        {
            Name = "Test User"
        };

        var userId = await creator.CreateUser(dto);

        var lastMessage = MessageBus.GetLastMessage<string>(Messages.UserCreated);

        Assert.That(lastMessage, Is.EqualTo(userId));
    }
}