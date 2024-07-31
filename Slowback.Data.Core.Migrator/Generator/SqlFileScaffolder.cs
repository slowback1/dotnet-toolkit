using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Generator;

internal class SqlFileScaffolder
{
    private readonly DataMigration _migration;

    public SqlFileScaffolder(DataMigration migration)
    {
        _migration = migration;
    }

    public void Scaffold()
    {
        var upFilePath = Path.Combine(MigrationDirectoryProvider.MigrationOutputDirectory, _migration.UpFileName);
        var downFilePath = Path.Combine(MigrationDirectoryProvider.MigrationOutputDirectory, _migration.DownFileName);

        EnsureDirectoryExists();

        var content = GetMigrationContent();

        File.WriteAllText(upFilePath, content);
        File.WriteAllText(downFilePath, content);
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(MigrationDirectoryProvider.MigrationOutputDirectory))
            Directory.CreateDirectory(MigrationDirectoryProvider.MigrationOutputDirectory);
    }

    private string GetMigrationContent()
    {
        return @"-- Write your SQL here
THROW 51000, 'This is a placeholder for your SQL.', 1;
";
    }
}