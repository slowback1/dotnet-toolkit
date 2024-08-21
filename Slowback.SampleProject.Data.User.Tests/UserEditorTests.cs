using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.User.Tests;

public class UserEditorTests : BaseDbTest
{
    private string UserId { get; set; }

    [SetUp]
    public async Task Setup()
    {
        var user = new CreateUser
        {
            Name = "Test User"
        };

        var creator = new UserCreator(_context);
        UserId = await creator.CreateUser(user);
    }

    [Test]
    public async Task CanEditAUser()
    {
        var editor = new UserEditor(_context);

        var dto = new EditUser
        {
            Id = UserId,
            Name = "Edited User"
        };

        await editor.UpdateUser(dto);

        var retriever = new UserRetriever(_context);
        var result = await retriever.GetUserById(UserId);

        Assert.That(result.Name, Is.EqualTo("Edited User"));
    }

    [Test]
    public async Task EditingAUserSendsAnEventThroughTheMessageBus()
    {
        var editor = new UserEditor(_context);

        var dto = new EditUser
        {
            Id = UserId,
            Name = "Edited User"
        };

        await editor.UpdateUser(dto);

        var message = MessageBus.GetLastMessage<string>(Messages.UserUpdated);

        Assert.That(message, Is.Not.Null);
        Assert.That(message, Is.EqualTo(UserId));
    }
}