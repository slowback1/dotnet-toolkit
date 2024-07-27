using Microsoft.EntityFrameworkCore;
using Slowback.Common.Dtos;
using Slowback.Data.Core.EF;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoRetriever : BaseDatabaseAction
{
    public ToDoRetriever(ConnectionOptions options) : base(options)
    {
    }

    public ToDoRetriever(SampleAppContext context) : base(context)
    {
    }

    private GetToDo GetToDoFromEntity(Core.Models.ToDo toDo)
    {
        return new GetToDo
        {
            Id = toDo.ToDoId,
            Description = toDo.Description,
            IsComplete = toDo.IsComplete,
            CreatedAt = toDo.CreatedAt,
            CompletedAt = toDo.CompletedAt
        };
    }

    public async Task<GetToDo> GetToDoById(int toDoId)
    {
        var toDo = await _context.ToDos.FindAsync(toDoId);

        if (toDo == null) return null;

        return GetToDoFromEntity(toDo);
    }

    public async Task<IEnumerable<GetToDo>> GetToDos()
    {
        var toDos = await _context.ToDos.ToListAsync();

        return toDos.Select(GetToDoFromEntity);
    }
}