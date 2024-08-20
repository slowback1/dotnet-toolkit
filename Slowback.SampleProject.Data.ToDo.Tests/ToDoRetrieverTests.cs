using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo.Tests;

public class ToDoRetrieverTests : BaseDbTest
{
    private const int nonExistentToDoId = 999999;
    private readonly DateTime Today = new(2024, 6, 7);
    private int _toDoId { get; set; }

    [SetUp]
    public async Task CreateATestEntity()
    {
        TimeEnvironment.SetProvider(new TestTimeProvider(Today));

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
        Assert.That(result.CreatedAt, Is.EqualTo(Today).Within(1).Seconds);
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

    [Test]
    public async Task GetToDoById_IncludesCompletedAtDateWhenItsThere()
    {
        var stored = await _context.ToDos.FindAsync(_toDoId);
        stored.CompletedAt = new DateTime(2024, 5, 5);
        await _context.SaveChangesAsync();
        var retriever = new ToDoRetriever(_context);

        var result = await retriever.GetToDoById(_toDoId);

        Assert.That(result, Is.Not.Null);

        Assert.That(result.CompletedAt, Is.EqualTo(new DateTime(2024, 5, 5)).Within(1).Seconds);
    }
}