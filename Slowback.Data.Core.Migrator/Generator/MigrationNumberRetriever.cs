namespace Slowback.Data.Core.Migrator.Generator;

internal static class MigrationNumberRetriever
{
    public static int GetNextMigrationNumber()
    {
        var migrations = MigrationFileReader.Read().ToList();

        if (!migrations.Any()) return 1;

        var max = migrations.Max(m => m.Number);

        return max + 1;
    }
}