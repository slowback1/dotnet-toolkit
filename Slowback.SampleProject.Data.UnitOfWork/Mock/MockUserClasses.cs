using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.User;

namespace Slowback.SampleProject.Data.UnitOfWork.Mock;

public class MockUserCreator : IUserCreator
{
    public Task<string> CreateUser(CreateUser dto)
    {
        return Task.FromResult("mock-user-id");
    }
}

public class MockUserEditor : IUserEditor
{
    public Task UpdateUser(EditUser dto)
    {
        return Task.CompletedTask;
    }
}

public class MockUserRetriever : IUserRetriever
{
    public Task<GetUser> GetUserById(string userId)
    {
        return Task.FromResult(new GetUser
        {
            Id = "mock-user-id",
            Name = "mock-user-name"
        });
    }
}