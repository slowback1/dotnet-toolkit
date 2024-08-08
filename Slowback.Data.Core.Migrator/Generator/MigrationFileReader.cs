using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Generator;

internal static class MigrationFileReader
{
    public static IEnumerable<DataMigration> Read()
    {
        var fileContents = ReadFromMigrationFile();

        if (fileContents == null) return new List<DataMigration>();

        return ReadFromCsv(fileContents);
    }

    private static List<DataMigration> ReadFromCsv(string csv)
    {
        var lines = csv.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        var migrations = new List<DataMigration>();

        for (var i = 1; i < lines.Count; i++)
        {
            var line = lines[i].Split(',');

            migrations.Add(new DataMigration
            {
                Name = line[0],
                CreatedTime = DateTime.Parse(line[1]),
                UpFileName = line[2],
                DownFileName = line[3]
            });
        }

        return migrations;
    }

    private static string? ReadFromMigrationFile()
    {
        var directory = MigrationDirectoryProvider.MigrationOutputDirectory;

        if (!Directory.Exists(directory)) return null;

        if (!File.Exists($"{directory}/MigrationHistory.csv")) return null;

        return File.ReadAllText($"{directory}/MigrationHistory.csv");
    }
}