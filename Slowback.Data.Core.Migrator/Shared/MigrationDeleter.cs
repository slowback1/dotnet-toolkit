using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Shared;

internal class MigrationDeleter : MigrationActionBase
{
    public MigrationDeleter(MigrationContext context) : base(context)
    {
    }

    public void DeleteMigration(DataMigration migration)
    {
        var formattedSql = _context.QuerySet.DeleteMigration
            .Replace("@Name", migration.Name);

        _context.ExecuteSql(formattedSql);
    }
}