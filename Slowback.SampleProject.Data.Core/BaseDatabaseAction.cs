using Slowback.Data.Core.EF;

namespace Slowback.SampleProject.Data.Core;

public abstract class BaseDatabaseAction
{
    protected readonly SampleAppContext _context;

    public BaseDatabaseAction(ConnectionOptions options)
    {
        _context = new SampleAppContext(options);
    }

    public BaseDatabaseAction(SampleAppContext context)
    {
        _context = context;
    }
}