using Slowback.SampleProject.Data.ToDo;
using Slowback.SampleProject.Data.User;

namespace Slowback.SampleProject.Data.UnitOfWork;

public interface IUnitOfWork
{
    IToDoCreator ToDoCreator { get; }
    IToDoRetriever ToDoRetriever { get; }
    IToDoUpdater ToDoUpdater { get; }
    IUserCreator UserCreator { get; }
    IUserRetriever UserRetriever { get; }
    IUserEditor UserEditor { get; }
}