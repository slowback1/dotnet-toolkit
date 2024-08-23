using Slowback.Common;
using Slowback.Messaging;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoCreator : BaseDatabaseAction, IToDoCreator
{
    public ToDoCreator(SampleAppContext context) : base(context)
    {
    }

    public async Task<int> CreateToDo(CreateToDo dto, string userId)
    {
        var toDo = dto.ConvertToEntity(userId);

        toDo.CreatedAt = TimeEnvironment.Provider.Today();

        await _context.ToDos.AddAsync(toDo);
        await _context.SaveChangesAsync();

        await MessageBus.PublishAsync(Messages.ToDoCreated, toDo.ToDoId);

        return toDo.ToDoId;
    }
}

public interface IToDoCreator
{
    Task<int> CreateToDo(CreateToDo dto, string userId);
}