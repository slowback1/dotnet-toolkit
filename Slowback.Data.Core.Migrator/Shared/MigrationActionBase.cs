namespace Slowback.Data.Core.Migrator.Shared;

internal abstract class MigrationActionBase
{
    protected readonly MigrationContext _context;

    public MigrationActionBase(MigrationContext context)
    {
        _context = context;
    }
}