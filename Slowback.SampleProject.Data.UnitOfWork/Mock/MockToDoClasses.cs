using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.ToDo;

namespace Slowback.SampleProject.Data.UnitOfWork.Mock;

public class MockToDoCreator : IToDoCreator
{
    public int CreatedToDoId { get; set; } = 1;

    public Task<int> CreateToDo(CreateToDo dto, string userId)
    {
        return Task.FromResult(CreatedToDoId);
    }
}

public class MockToDoRetriever : IToDoRetriever
{
    public Task<GetToDo> GetToDoById(int toDoId)
    {
        return Task.FromResult(GetExampleToDo());
    }

    public async Task<IEnumerable<GetToDo>> GetToDos()
    {
        var todo = GetExampleToDo();
        return new List<GetToDo> { todo };
    }

    public async Task<IEnumerable<GetToDo>> GetToDosForUser(string userId)
    {
        var todo = GetExampleToDo();
        return new List<GetToDo> { todo };
    }

    private GetToDo GetExampleToDo()
    {
        return new GetToDo
        {
            Description = "This is an example ToDo",
            CompletedAt = new DateTime(),
            CreatedAt = new DateTime(),
            IsComplete = true,
            Id = 1
        };
    }
}

public class MockToDoUpdater : IToDoUpdater
{
    public int UpdatedToDoId { get; set; } = 1;

    public Task<int> UpdateToDo(EditToDo dto)
    {
        return Task.FromResult(UpdatedToDoId);
    }
}