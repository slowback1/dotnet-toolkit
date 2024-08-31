using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.UnitOfWork.Mock;

namespace Slowback.SampleProject.Data.UnitOfWork.Tests.Mock;

public class MockUserCreatorTests
{
    [Test]
    public async Task CreateUser_ReturnsMockUserId()
    {
        var mockUserCreator = new MockUserCreator();
        var result = await mockUserCreator.CreateUser(new CreateUser());

        Assert.That(result, Is.EqualTo("mock-user-id"));
    }
}

public class MockUserEditorTests
{
    [Test]
    public async Task UpdateUser_CompletesSuccessfully()
    {
        var mockUserEditor = new MockUserEditor();
        await mockUserEditor.UpdateUser(new EditUser());

        Assert.Pass(); // If no exception is thrown, the test passes
    }
}

public class MockUserRetrieverTests
{
    [Test]
    public async Task GetUserById_ReturnsMockUser()
    {
        var mockUserRetriever = new MockUserRetriever();
        var result = await mockUserRetriever.GetUserById("mock-user-id");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("mock-user-id"));
        Assert.That(result.Name, Is.EqualTo("mock-user-name"));
    }
}