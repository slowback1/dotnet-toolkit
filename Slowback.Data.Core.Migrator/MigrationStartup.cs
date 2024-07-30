using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.Logger.LoggingEngines;
using Slowback.Messaging;

namespace Slowback.Data.Core.Migrator;

public static class MigrationStartup
{
    private const string UsageMessage = @"Usage: 
    Slowback.Data.Core.Migrator.exe <action> <version>
    Actions:
        -migrate: Migrate the database to the latest version, or to <version> if specified
        -rollback: Rollback the database to the previous version, or to <version> if specified
        -generate: Generate a new migration.  The user will be prompted to give the migration a name.
        -drop: Drop the database
        -info: Display information about the database
        -help: Display this message";

    private static readonly List<string> ValidActions = new()
    {
        "-migrate",
        "-rollback",
        "-generate",
        "-drop",
        "-info",
        "-help"
    };

    private static IConfigurationRoot LoadConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment}.json", true, true);

        return builder.Build();
    }

    private static void EnableLogging()
    {
        Logger.Logger.EnableLogging(new List<ILoggingEngine>
        {
            new ConsoleLoggingEngine()
        }, Messages.LogMessage);
    }

    private static void PublishAppDbConnection(IConfigurationRoot configuration)
    {
        var dbType = configuration.GetRequiredSection("Database")
            .GetRequiredSection("DatabaseType")
            .Value;

        var connectionString = configuration.GetRequiredSection("Database")
            .GetRequiredSection("ConnectionString")
            .Value;

        if (dbType is null || connectionString is null)
            throw new InvalidConfigurationException("Database section is missing from configuration file.");

        var section = new ConnectionOptions
        {
            DatabaseType = dbType,
            ConnectionString = connectionString
        };

        MessageBus.Publish(Messages.AppDbConnection, section);
    }

    public static int StartMigrator(string[] args)
    {
        var configuration = LoadConfiguration();

        EnableLogging();
        PublishAppDbConnection(configuration);

        MessageBus.Publish(Messages.LogMessage, "Configuration loaded successfully.");

        var action = args.FirstOrDefault();

        if (action is null)
        {
            MessageBus.Publish(Messages.LogMessage, "No action specified. Exiting.");
            MessageBus.Publish(Messages.LogMessage, UsageMessage);

            return 1;
        }

        if (!ValidActions.Contains(action))
        {
            MessageBus.Publish(Messages.LogMessage, "Invalid action specified. Exiting.");
            MessageBus.Publish(Messages.LogMessage, UsageMessage);

            return 1;
        }

        return 0;
    }
}