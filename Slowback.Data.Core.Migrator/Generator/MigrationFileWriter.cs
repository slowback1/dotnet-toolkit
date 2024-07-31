using Newtonsoft.Json;
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

        var fileContents = JsonConvert.SerializeObject(existingMigrations);

        File.WriteAllText($"{directory}/MigrationHistory.json", fileContents);
    }
}