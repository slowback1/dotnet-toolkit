﻿using Microsoft.EntityFrameworkCore;

namespace Slowback.Data.Core.EF;

public class BaseContext : DbContext
{
    private readonly ConnectionOptions _connectionOptions;

    public BaseContext(ConnectionOptions connectionOptions)
    {
        _connectionOptions = connectionOptions;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        HandleOptionsBuilding(optionsBuilder, _connectionOptions);
    }

    public static void HandleOptionsBuilding(DbContextOptionsBuilder optionsBuilder, ConnectionOptions options)
    {
        switch (options.DatabaseType)
        {
            case DatabaseType.SqlServer:
                optionsBuilder.UseSqlServer(options.ConnectionString);
                break;

            case DatabaseType.InMemory:
                optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
                break;
        }
    }
}