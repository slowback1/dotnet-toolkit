using Slowback.SampleProject.Data.ToDo;

namespace Slowback.SampleProject.Data.UnitOfWork;

public interface IUnitOfWork
{
    IToDoCreator ToDoCreator { get; }
    IToDoRetriever ToDoRetriever { get; }
    IToDoUpdater ToDoUpdater { get; }
}