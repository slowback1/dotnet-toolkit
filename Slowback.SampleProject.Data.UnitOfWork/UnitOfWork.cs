using Slowback.SampleProject.Data.Core;
using Slowback.SampleProject.Data.ToDo;
using Slowback.SampleProject.Data.User;

namespace Slowback.SampleProject.Data.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(SampleAppContext context)
    {
        ToDoCreator = new ToDoCreator(context);
        ToDoRetriever = new ToDoRetriever(context);
        ToDoUpdater = new ToDoUpdater(context);
        UserCreator = new UserCreator(context);
        UserRetriever = new UserRetriever(context);
        UserEditor = new UserEditor(context);
    }

    public IToDoCreator ToDoCreator { get; }
    public IToDoRetriever ToDoRetriever { get; }
    public IToDoUpdater ToDoUpdater { get; }
    public IUserCreator UserCreator { get; }
    public IUserRetriever UserRetriever { get; }
    public IUserEditor UserEditor { get; }
}