using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo.Tests;

public class ToDoCreatorTests : BaseDbTest
{
    private ToDoCreator _creator { get; set; }
    private string UserId { get; set; }

    [SetUp]
    public void Setup()
    {
        _creator = new ToDoCreator(_context);
        UserId = Guid.NewGuid().ToString();
    }

    [Test]
    public async Task CreateToDoReturnsAnId()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto, UserId);

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public async Task CreateToDoSavesToDatabase()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto, UserId);

        var toDo = await _lookupContext.ToDos.FindAsync(result);

        Assert.That(toDo, Is.Not.Null);
        Assert.That(toDo.Description, Is.EqualTo(dto.Description));
        Assert.That(toDo.UserId, Is.EqualTo(new Guid(UserId)));
    }

    [Test]
    public async Task CreateSendsAToDoCreatedMessageToTheMessageBus()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = await _creator.CreateToDo(dto, UserId);

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

        var result = await _creator.CreateToDo(dto, UserId);

        var toDo = await _lookupContext.ToDos.FindAsync(result);

        Assert.That(toDo.CreatedAt, Is.EqualTo(today).Within(1).Seconds);
    }
}