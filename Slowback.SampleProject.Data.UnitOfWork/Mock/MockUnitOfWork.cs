using Slowback.SampleProject.Data.ToDo;
using Slowback.SampleProject.Data.User;

namespace Slowback.SampleProject.Data.UnitOfWork.Mock;

internal class MockUnitOfWork : IUnitOfWork
{
    public IToDoCreator ToDoCreator { get; } = new MockToDoCreator();
    public IToDoRetriever ToDoRetriever { get; } = new MockToDoRetriever();
    public IToDoUpdater ToDoUpdater { get; } = new MockToDoUpdater();
    public IUserCreator UserCreator { get; } = new MockUserCreator();
    public IUserRetriever UserRetriever { get; } = new MockUserRetriever();
    public IUserEditor UserEditor { get; } = new MockUserEditor();
}