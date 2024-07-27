using Slowback.Common;
using Slowback.Common.Dtos;
using Slowback.Data.Core.EF;
using Slowback.Messaging;
using Slowback.SampleProject.Data.Core;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoUpdater : BaseDatabaseAction
{
    public ToDoUpdater(ConnectionOptions options) : base(options)
    {
    }

    public ToDoUpdater(SampleAppContext context) : base(context)
    {
    }

    public async Task<int> UpdateToDo(EditToDo dto)
    {
        var toDo = await _context.ToDos.FindAsync(dto.Id);

        if (toDo == null) return 0;

        toDo.Description = dto.Description;
        toDo.IsComplete = dto.IsComplete;

        if (toDo.IsComplete)
            toDo.CompletedAt = TimeEnvironment.Provider.Today();

        await _context.SaveChangesAsync();

        await MessageBus.PublishAsync(Messages.ToDoUpdated, toDo.ToDoId);

        return toDo.ToDoId;
    }
}