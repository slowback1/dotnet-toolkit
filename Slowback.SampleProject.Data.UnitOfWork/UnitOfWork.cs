using Slowback.SampleProject.Data.Core;
using Slowback.SampleProject.Data.ToDo;

namespace Slowback.SampleProject.Data.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(SampleAppContext context)
    {
        ToDoCreator = new ToDoCreator(context);
        ToDoRetriever = new ToDoRetriever(context);
        ToDoUpdater = new ToDoUpdater(context);
    }

    public IToDoCreator ToDoCreator { get; }
    public IToDoRetriever ToDoRetriever { get; }
    public IToDoUpdater ToDoUpdater { get; }
}