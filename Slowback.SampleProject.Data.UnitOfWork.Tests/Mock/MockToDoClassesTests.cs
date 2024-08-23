using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.UnitOfWork.Mock;

namespace Slowback.SampleProject.Data.UnitOfWork.Tests.Mock;

public class MockToDoCreaterTests
{
    [Test]
    public async Task ByDefaultReturns1()
    {
        var mockToDoCreator = new MockToDoCreator();

        var result = await mockToDoCreator.CreateToDo(new CreateToDo(), "");

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task CanSetCreatedToDoId()
    {
        var mockToDoCreator = new MockToDoCreator();
        mockToDoCreator.CreatedToDoId = 2;

        var result = await mockToDoCreator.CreateToDo(new CreateToDo(), "");

        Assert.That(result, Is.EqualTo(2));
    }
}

public class MockToDoRetrieverTests
{
    [Test]
    public async Task GetToDoByIdReturnsGetToDo()
    {
        var mockToDoRetriever = new MockToDoRetriever();

        var result = await mockToDoRetriever.GetToDoById(1);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Description, Is.EqualTo("This is an example ToDo"));

        Assert.That(result.CompletedAt, Is.EqualTo(new DateTime()));
        Assert.That(result.CreatedAt, Is.EqualTo(new DateTime()));
        Assert.That(result.IsComplete, Is.True);
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetToDosReturnsListOfGetToDos()
    {
        var mockToDoRetriever = new MockToDoRetriever();

        var result = await mockToDoRetriever.GetToDos();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToList(), Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetToDosForUserReturnsListOfGetToDos()
    {
        var mockToDoRetriever = new MockToDoRetriever();

        var result = await mockToDoRetriever.GetToDosForUser("");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToList(), Has.Count.EqualTo(1));
    }
}

public class MockToDoUpdaterTests
{
    [Test]
    public async Task ByDefaultReturns1()
    {
        var mockToDoUpdater = new MockToDoUpdater();

        var result = await mockToDoUpdater.UpdateToDo(new EditToDo());

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task CanSetUpdatedToDoId()
    {
        var mockToDoUpdater = new MockToDoUpdater();
        mockToDoUpdater.UpdatedToDoId = 2;

        var result = await mockToDoUpdater.UpdateToDo(new EditToDo());

        Assert.That(result, Is.EqualTo(2));
    }
}