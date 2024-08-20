using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Shared;

internal class MigrationInserter : MigrationActionBase
{
    public MigrationInserter(MigrationContext context) : base(context)
    {
    }

    public void InsertMigration(DataMigration migration)
    {
        var formattedSql = _context.QuerySet.InsertMigration
            .Replace("@Name", migration.Name)
            .Replace("@CreatedTime", migration.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"))
            .Replace("@UpFileName", migration.UpFileName)
            .Replace("@DownFileName", migration.DownFileName);

        _context.ExecuteSql(formattedSql);
    }
}