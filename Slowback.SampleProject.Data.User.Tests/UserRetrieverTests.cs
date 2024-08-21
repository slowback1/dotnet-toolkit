using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.User.Tests;

public class UserRetrieverTests : BaseDbTest
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
    public async Task CanRetrieveAUser()
    {
        var retriever = new UserRetriever(_context);

        var result = await retriever.GetUserById(UserId);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task ReturnsTheCorrectUser()
    {
        var retriever = new UserRetriever(_context);

        var result = await retriever.GetUserById(UserId);

        Assert.That(result.Name, Is.EqualTo("Test User"));
        Assert.That(result.Id, Is.EqualTo(UserId));
    }

    [Test]
    public async Task ReturnsNullForNonExistentUser()
    {
        var retriever = new UserRetriever(_context);

        var result = await retriever.GetUserById(Guid.NewGuid().ToString());

        Assert.That(result, Is.Null);
    }
}