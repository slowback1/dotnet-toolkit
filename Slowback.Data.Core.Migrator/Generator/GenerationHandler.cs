namespace Slowback.Data.Core.Migrator.Generator;

internal static class GenerationHandler
{
    public static int HandleGenerate()
    {
        Console.WriteLine("Enter a name for the migration:");
        var migrationName = Console.ReadLine();

        if (migrationName is null)
        {
            Console.WriteLine("Migration name cannot be null.");
            return 1;
        }

        if (migrationName.Length == 0)
        {
            Console.WriteLine("Migration name cannot be empty.");
            return 1;
        }

        var migration = MigrationBuilder.BuildMigration(migrationName);

        MigrationFileWriter.Write(migration);

        var scaffolder = new SqlFileScaffolder(migration);
        scaffolder.Scaffold();

        Console.WriteLine($"Migration {migration.UpFileName} generated successfully.");
        Console.WriteLine($"Migration {migration.DownFileName} generated successfully.");

        return 0;
    }
}