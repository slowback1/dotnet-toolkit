using Slowback.SampleProject.Common.Dtos;

namespace Slowback.SampleProject.Data.ToDo;

internal static class ToDoConverters
{
    public static Core.Models.ToDo ConvertToEntity(this CreateToDo dto, string userId)
    {
        return new Core.Models.ToDo
        {
            Description = dto.Description,
            UserId = new Guid(userId)
        };
    }

    public static GetToDo ConvertToDto(this Core.Models.ToDo entity)
    {
        return new GetToDo
        {
            Id = entity.ToDoId,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            CompletedAt = entity.CompletedAt,
            IsComplete = entity.IsComplete
        };
    }

    public static Core.Models.ToDo ConvertToEntity(this EditToDo dto)
    {
        return new Core.Models.ToDo
        {
            ToDoId = dto.Id,
            Description = dto.Description,
            IsComplete = dto.IsComplete
        };
    }
}