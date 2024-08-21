using Microsoft.EntityFrameworkCore;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoRetriever : BaseDatabaseAction
{
    public ToDoRetriever(SampleAppContext context) : base(context)
    {
    }

    private GetToDo GetToDoFromEntity(Core.Models.ToDo toDo)
    {
        return toDo.ConvertToDto();
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

        return toDos.Select(GetToDoFromEntity).ToList();
    }

    public async Task<IEnumerable<GetToDo>> GetToDosForUser(string userId)
    {
        var toDos = await _context.ToDos.Where(x => x.UserId == new Guid(userId)).ToListAsync();

        return toDos.Select(GetToDoFromEntity).ToList();
    }
}