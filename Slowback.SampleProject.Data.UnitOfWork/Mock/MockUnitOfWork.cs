using Slowback.SampleProject.Data.ToDo;

namespace Slowback.SampleProject.Data.UnitOfWork.Mock;

internal class MockUnitOfWork : IUnitOfWork
{
    public IToDoCreator ToDoCreator { get; } = new MockToDoCreator();
    public IToDoRetriever ToDoRetriever { get; } = new MockToDoRetriever();
    public IToDoUpdater ToDoUpdater { get; } = new MockToDoUpdater();
}