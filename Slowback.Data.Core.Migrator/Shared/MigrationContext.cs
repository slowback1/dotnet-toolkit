using Microsoft.EntityFrameworkCore;
using Slowback.Data.Core.EF;

namespace Slowback.Data.Core.Migrator.Shared;

internal class MigrationContext : BaseContext
{
    public readonly MigrationQuerySet QuerySet;

    public MigrationContext(ConnectionOptions connectionOptions) : base(connectionOptions)
    {
        QuerySet = MigrationQuerySets.GetMigrationQuerySet(connectionOptions);
        EnsureMigrationTable();
    }

    private void EnsureMigrationTable()
    {
        if (_connectionOptions.DatabaseType == DatabaseType.InMemory)
            return;

        Database.ExecuteSqlRaw(QuerySet.CreateMigrationTable);
    }

    public IEnumerable<T> Sql<T>(string query) where T : class
    {
        if (_connectionOptions.DatabaseType == DatabaseType.InMemory)
            return new List<T>();

        return Database.SqlQueryRaw<T>(query);
    }

    public void ExecuteSql(string query)
    {
        if (_connectionOptions.DatabaseType == DatabaseType.InMemory)
            return;

        Database.ExecuteSqlRaw(query);
    }
}

public static class MigrationQuerySets
{
    private static readonly MigrationQuerySet SqlServer = new()
    {
        CreateMigrationTable =
            "CREATE TABLE IF NOT EXISTS __DataMigrations (Name TEXT PRIMARY KEY, CreatedTime TEXT, UpFileName TEXT, DownFileName TEXT)",
        InsertMigration =
            "INSERT INTO __DataMigrations (Name, CreatedTime, UpFileName, DownFileName) VALUES (@Name, @CreatedTime, @UpFileName, @DownFileName)",
        GetMigrations = "SELECT * FROM __DataMigrations",
        DeleteMigration = "DELETE FROM __DataMigrations WHERE Name = @Name AND CreatedTime = @CreatedTime"
    };

    private static readonly MigrationQuerySet InMemorySet = new()
    {
        CreateMigrationTable = "",
        InsertMigration = "",
        GetMigrations = "",
        DeleteMigration = ""
    };

    public static MigrationQuerySet GetMigrationQuerySet(ConnectionOptions connectionOptions)
    {
        return connectionOptions.DatabaseType switch
        {
            DatabaseType.SqlServer => SqlServer,
            DatabaseType.InMemory => InMemorySet,
            _ => throw new Exception("Database type not supported")
        };
    }
}

public class MigrationQuerySet
{
    public string CreateMigrationTable { get; set; }
    public string InsertMigration { get; set; }
    public string GetMigrations { get; set; }
    public string DeleteMigration { get; set; }
}