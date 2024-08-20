using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Shared;
using Slowback.Messaging;

namespace Slowback.Data.Core.Migrator.Migration;

public class MigrationHandler
{
    public static int HandleMigration(string? version = null)
    {
        Console.WriteLine("Finding migrations to apply...");

        var context = new MigrationContext(MessageBus.GetLastMessage<ConnectionOptions>(Messages.AppDbConnection));

        var allMigrations = MigrationFileReader.Read();
        var doneMigrations = new MigrationHistoryRetriever(context).GetAlreadyDoneMigrations();

        var migrationsToApply = allMigrations.GetMigrationsToApply(doneMigrations).ToList();

        if (!migrationsToApply.Any())
        {
            Console.WriteLine("No migrations to apply.");
            return 0;
        }

        Console.WriteLine("Applying migrations...");

        var inserter = new MigrationInserter(context);

        foreach (var migration in migrationsToApply)
        {
            Console.WriteLine("Applying migration: " + migration.Name);
            ApplyMigration(context, migration.UpFileName);
            inserter.InsertMigration(migration);
        }

        return 0;
    }

    private static void ApplyMigration(MigrationContext context, string fileName)
    {
        var migration = ReadMigration(fileName);

        context.ExecuteSql(migration);
    }

    private static string ReadMigration(string fileName)
    {
        var path = Path.Combine(MigrationDirectoryProvider.MigrationInputDirectory, fileName);

        return File.ReadAllText(path);
    }
}