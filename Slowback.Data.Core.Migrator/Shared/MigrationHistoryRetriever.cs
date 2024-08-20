using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Shared;

internal class MigrationHistoryRetriever : MigrationActionBase
{
    public MigrationHistoryRetriever(MigrationContext context) : base(context)
    {
    }


    public IEnumerable<DataMigration> GetAlreadyDoneMigrations()
    {
        var migrations = _context.Sql<DataMigration>(_context.QuerySet.GetMigrations);

        return migrations.ToList();
    }
}