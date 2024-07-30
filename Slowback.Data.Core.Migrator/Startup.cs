using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.Logger.LoggingEngines;
using Slowback.Messaging;

namespace Slowback.Data.Core.Migrator;

public static class Startup
{
    public static IConfigurationRoot LoadConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment}.json", true, true);

        return builder.Build();
    }

    public static void EnableLogging()
    {
        Logger.Logger.EnableLogging(new List<ILoggingEngine>
        {
            new ConsoleLoggingEngine()
        }, Messages.LogMessage);
    }

    public static void PublishAppDbConnection(IConfigurationRoot configuration)
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
}