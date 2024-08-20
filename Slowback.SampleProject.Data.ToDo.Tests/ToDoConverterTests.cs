using Slowback.SampleProject.Common.Dtos;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.ToDo.Tests;

[TestFixture]
public class ToDoConvertersTests
{
    [Test]
    public void ConvertToEntity_WithCreateToDo_ReturnsToDoEntity()
    {
        var dto = new CreateToDo
        {
            Description = "Test Description"
        };

        var result = dto.ConvertToEntity();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Description, Is.EqualTo("Test Description"));
    }

    [Test]
    public void ConvertToDto_WithToDoEntity_ReturnsGetToDoDto()
    {
        var entity = new Core.Models.ToDo
        {
            ToDoId = 1,
            Description = "Test Description",
            CreatedAt = new DateTime(2024, 6, 7),
            CompletedAt = new DateTime(2024, 6, 8),
            IsComplete = true
        };

        var result = entity.ConvertToDto();

        result.HasNoNulls();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Description, Is.EqualTo("Test Description"));
        Assert.That(result.CreatedAt, Is.EqualTo(new DateTime(2024, 6, 7)));
        Assert.That(result.CompletedAt, Is.EqualTo(new DateTime(2024, 6, 8)));
        Assert.That(result.IsComplete, Is.True);
    }

    [Test]
    public void ConvertToEntity_WithEditToDo_ReturnsToDoEntity()
    {
        var dto = new EditToDo
        {
            Id = 1,
            Description = "Updated Description",
            IsComplete = true
        };

        var result = dto.ConvertToEntity();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToDoId, Is.EqualTo(1));
        Assert.That(result.Description, Is.EqualTo("Updated Description"));
        Assert.That(result.IsComplete, Is.True);
    }
}