using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo.Tests;

public class ToDoUpdaterTests : BaseDbTest
{
    private readonly int _nonExistentToDoId = 999999;
    private readonly DateTime Today = new(2024, 7, 7);
    private ToDoUpdater _updater { get; set; }
    private int _toDoId { get; set; }
    private string UserId { get; set; }


    [SetUp]
    public async Task SetUp()
    {
        UserId = Guid.NewGuid().ToString();

        TimeEnvironment.SetProvider(new TestTimeProvider(Today));

        _updater = new ToDoUpdater(_context);

        var creator = new ToDoCreator(_context);

        _toDoId = await creator.CreateToDo(new CreateToDo { Description = "test" }, UserId);
    }

    [Test]
    public async Task UpdateToDoReturnsTrue()
    {
        var dto = new EditToDo
        {
            Id = 1,
            Description = "Test Description"
        };

        var result = await _updater.UpdateToDo(dto);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateToDoSavesToDatabase()
    {
        var dto = new EditToDo
        {
            Id = _toDoId,
            Description = "Test Description"
        };

        var result = await _updater.UpdateToDo(dto);

        var toDo = await _lookupContext.ToDos.FindAsync(_toDoId);

        Assert.That(toDo, Is.Not.Null);
        Assert.That(toDo.Description, Is.EqualTo(dto.Description));
    }

    [Test]
    public async Task UpdateToDoReturnsZeroForNonExistentToDo()
    {
        var dto = new EditToDo
        {
            Id = _nonExistentToDoId,
            Description = "Test Description"
        };

        var result = await _updater.UpdateToDo(dto);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateToDoSendsAToDoUpdatedMessageToTheMessageBus()
    {
        var dto = new EditToDo
        {
            Id = _toDoId,
            Description = "Test Description"
        };

        var result = await _updater.UpdateToDo(dto);

        var message = MessageBus.GetLastMessage<int>(Messages.ToDoUpdated);

        Assert.That(message, Is.EqualTo(_toDoId));
    }

    [Test]
    public async Task UpdateToDoSettingTheCompletedAtDateToToday()
    {
        var dto = new EditToDo
        {
            Id = _toDoId,
            Description = "Test Description",
            IsComplete = true
        };

        var result = await _updater.UpdateToDo(dto);

        var stored = await _lookupContext.ToDos.FindAsync(_toDoId);

        Assert.That(stored.CompletedAt, Is.EqualTo(Today).Within(1).Seconds);
    }
}