using Newtonsoft.Json;
using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Generator;

internal static class MigrationFileReader
{
    public static IEnumerable<DataMigration> Read()
    {
        var fileContents = ReadFromMigrationFile();

        if (fileContents == null) return new List<DataMigration>();

        return JsonConvert.DeserializeObject<List<DataMigration>>(fileContents) ?? new List<DataMigration>();
    }

    private static string? ReadFromMigrationFile()
    {
        var directory = MigrationDirectoryProvider.Directory;

        if (!Directory.Exists(directory)) return null;

        if (!File.Exists($"{directory}/MigrationHistory.json")) return null;

        return File.ReadAllText($"{directory}/MigrationHistory.json");
    }
}