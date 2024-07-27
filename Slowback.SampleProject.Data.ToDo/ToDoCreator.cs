using Slowback.Common;
using Slowback.Common.Dtos;
using Slowback.Data.Core.EF;
using Slowback.Messaging;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.ToDo;

public class ToDoCreator : BaseDatabaseAction
{
    public ToDoCreator(ConnectionOptions options) : base(options)
    {
    }

    public ToDoCreator(SampleAppContext context) : base(context)
    {
    }

    public async Task<int> CreateToDo(CreateToDo dto)
    {
        var toDo = new Core.Models.ToDo
        {
            Description = dto.Description,
            IsComplete = false
        };

        await _context.ToDos.AddAsync(toDo);
        await _context.SaveChangesAsync();

        await MessageBus.PublishAsync(Messages.ToDoCreated, toDo.ToDoId);

        return toDo.ToDoId;
    }
}