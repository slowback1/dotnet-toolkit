using Slowback.Common;
using Slowback.Common.Dtos;
using Slowback.Messaging;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo.Tests;

public class ToDoCreatorTests : BaseDbTest
{
    private ToDoCreator _creator { get; set; }

    [SetUp]
    public void Setup()
    {
        _creator = new ToDoCreator(_context);
    }

    [Test]
    public async Task CreateToDoReturnsAnId()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto);

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public async Task CreateToDoSavesToDatabase()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto);

        var toDo = await _lookupContext.ToDos.FindAsync(result);

        Assert.That(toDo, Is.Not.Null);
        Assert.That(toDo.Description, Is.EqualTo(dto.Description));
    }

    [Test]
    public async Task CreateSendsAToDoCreatedMessageToTheMessageBus()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto);

        var message = MessageBus.GetLastMessage<int>(Messages.ToDoCreated);

        Assert.That(message, Is.EqualTo(result));
    }

    [Test]
    public async Task CreateSetsTheCreatedAtDate()
    {
        var today = new DateTime(2024, 7, 7);

        TimeEnvironment.SetProvider(new TestTimeProvider(today));

        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto);

        var toDo = await _lookupContext.ToDos.FindAsync(result);

        Assert.That(toDo.CreatedAt, Is.EqualTo(today).Within(1).Seconds);
    }
}