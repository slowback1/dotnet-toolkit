using Slowback.Common;
using Slowback.Common.Dtos;
using Slowback.Messaging;
using Slowback.SampleProject.Data.Core;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoCreator : BaseDatabaseAction
{
    public ToDoCreator(SampleAppContext context) : base(context)
    {
    }

    public async Task<int> CreateToDo(CreateToDo dto)
    {
        var toDo = dto.ConvertToEntity();

        toDo.CreatedAt = TimeEnvironment.Provider.Today();

        await _context.ToDos.AddAsync(toDo);
        await _context.SaveChangesAsync();

        await MessageBus.PublishAsync(Messages.ToDoCreated, toDo.ToDoId);

        return toDo.ToDoId;
    }
}