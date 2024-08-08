using System.Text;
using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Generator;

internal static class MigrationFileWriter
{
    public static void Write(DataMigration migration)
    {
        var directory = MigrationDirectoryProvider.MigrationOutputDirectory;

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var existingMigrations = MigrationFileReader.Read().ToList();

        existingMigrations.Add(migration);

        var fileContents = GenerateCsv(existingMigrations);

        File.WriteAllText($"{directory}/MigrationHistory.csv", fileContents);
    }

    private static string GenerateCsv(IEnumerable<DataMigration> migrations)
    {
        var csv = new StringBuilder();
        csv.Append(WriteCsvHeader() + "\n");
        foreach (var migration in migrations) csv.Append(WriteCsvRow(migration) + "\n");
        return csv.ToString();
    }

    private static string WriteCsvHeader()
    {
        return "Name,CreatedTime,UpFileName,DownFileName";
    }

    private static string WriteCsvRow(DataMigration migration)
    {
        return $"{migration.Name},{migration.CreatedTime},{migration.UpFileName},{migration.DownFileName}";
    }
}