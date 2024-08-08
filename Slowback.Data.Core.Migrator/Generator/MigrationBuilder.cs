using Slowback.Data.Core.Migrator.Models;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Generator;

internal static class MigrationBuilder
{
    public static DataMigration BuildMigration(string name)
    {
        var migrationNumber = BuildMigrationTimestamp(TimeEnvironment.Provider.Now());

        return new DataMigration
        {
            Name = name,
            CreatedTime = TimeEnvironment.Provider.Now(),
            DownFileName = $"{migrationNumber}_{name}_Down.sql",
            UpFileName = $"{migrationNumber}_{name}_Up.sql"
        };
    }

    private static string BuildMigrationTimestamp(DateTime date)
    {
        return date.ToString("yyyyMMddHHmmss");
    }
}