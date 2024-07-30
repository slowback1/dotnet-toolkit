namespace Slowback.SampleProject.Data.Core;

public abstract class BaseDatabaseAction
{
    protected readonly SampleAppContext _context;

    protected BaseDatabaseAction(SampleAppContext context)
    {
        _context = context;
    }
}