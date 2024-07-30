using Slowback.Data.Core.Migrator.Generator;

namespace Slowback.Data.Core.Migrator;

internal static class MigrationInputHandler
{
    public static int HandleInput(string action, string? version)
    {
        switch (action)
        {
            case MigrationActions.Generate:
                return HandleGenerate();

            default:
                throw new NotImplementedException();
        }
    }


    private static int HandleGenerate()
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
        
        //todo: scaffolding for up and down sql scripts

        Console.WriteLine($"Migration {migration.UpFileName} generated successfully.");
        Console.WriteLine($"Migration {migration.DownFileName} generated successfully.");

        return 0;
    }
}