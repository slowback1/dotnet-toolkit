using Slowback.Common;
using Slowback.Data.Core.Migrator;
using Slowback.Messaging;

var configuration = Startup.LoadConfiguration();

Startup.EnableLogging();
Startup.PublishAppDbConnection(configuration);

MessageBus.Publish(Messages.LogMessage, "Configuration loaded successfully.");

var action = args.FirstOrDefault();

var validActions = new List<string>
{
    "-migrate",
    "-rollback",
    "-generate",
    "-drop",
    "-info",
    "-help"
};

var usageMessage = @"Usage: 
    Slowback.Data.Core.Migrator.exe <action> <version>
    Actions:
        -migrate: Migrate the database to the latest version, or to <version> if specified
        -rollback: Rollback the database to the previous version, or to <version> if specified
        -generate: Generate a new migration.  The user will be prompted to give the migration a name.
        -drop: Drop the database
        -info: Display information about the database
        -help: Display this message";

if (action is null)
{
    MessageBus.Publish(Messages.LogMessage, "No action specified. Exiting.");
    MessageBus.Publish(Messages.LogMessage, usageMessage);

    return 1;
}

if (!validActions.Contains(action))
{
    MessageBus.Publish(Messages.LogMessage, "Invalid action specified. Exiting.");
    MessageBus.Publish(Messages.LogMessage, usageMessage);

    return 1;
}

return 0;