using Slowback.Common.Dtos;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.ToDo.Tests;

public class ToDoRetrieverTests : BaseDbTest
{
    private const int nonExistentToDoId = 999999;
    private int _toDoId { get; set; }

    [SetUp]
    public async Task CreateATestEntity()
    {
        var creator = new ToDoCreator(_context);

        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        _toDoId = await creator.CreateToDo(dto);
    }

    [Test]
    public async Task GetToDos_ReturnsToDoById()
    {
        var retriever = new ToDoRetriever(_context);

        var result = await retriever.GetToDoById(_toDoId);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Id, Is.EqualTo(_toDoId));
        Assert.That(result.Description, Is.EqualTo("Test Description"));
    }

    [Test]
    public async Task GetToDos_ReturnsNullForNonExistentToDo()
    {
        var retriever = new ToDoRetriever(_context);

        var result = await retriever.GetToDoById(nonExistentToDoId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetToDos_ReturnsAllToDos()
    {
        var retriever = new ToDoRetriever(_context);

        var result = await retriever.GetToDos();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThan(0));
    }
}