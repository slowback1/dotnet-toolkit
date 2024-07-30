using Slowback.Common;
using Slowback.Common.Dtos;
using Slowback.Mapper;
using Slowback.Messaging;
using Slowback.SampleProject.Data.Core;
using Slowback.Time;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoUpdater : BaseDatabaseAction
{
    public ToDoUpdater(SampleAppContext context) : base(context)
    {
    }

    public async Task<int> UpdateToDo(EditToDo dto)
    {
        var toDo = await _context.ToDos.FindAsync(dto.Id);

        if (toDo == null) return 0;

        MapUpdatedTodo(dto, toDo);

        await _context.SaveChangesAsync();

        await MessageBus.PublishAsync(Messages.ToDoUpdated, toDo.ToDoId);

        return toDo.ToDoId;
    }

    private static void MapUpdatedTodo(EditToDo dto, Core.Models.ToDo toDo)
    {
        var convertedToDo = dto.ConvertToEntity();

        convertedToDo.Map(toDo);

        if (toDo.IsComplete)
            toDo.CompletedAt = TimeEnvironment.Provider.Today();
    }
}