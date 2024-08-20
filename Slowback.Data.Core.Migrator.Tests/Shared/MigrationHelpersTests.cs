using Slowback.Data.Core.Migrator.Models;
using Slowback.Data.Core.Migrator.Shared;

namespace Slowback.Data.Core.Migrator.Tests.Shared;

public class MigrationHelpersTests
{
    [Test]
    public void CanSortMigrationsByDateAndTime()
    {
        var migrations = new List<DataMigration>
        {
            new() { CreatedTime = new DateTime(2021, 1, 1, 1, 1, 3) },
            new() { CreatedTime = new DateTime(2021, 1, 1, 1, 1, 2) },
            new() { CreatedTime = new DateTime(2021, 1, 1, 1, 1, 4) }
        };

        var sortedMigrations = migrations.SortMigrations();

        Assert.IsNotNull(sortedMigrations);
        Assert.That(sortedMigrations, Is.Ordered.By(nameof(DataMigration.CreatedTime)).Ascending);
    }

    [Test]
    public void CanGetMigrationsToApply()
    {
        var allMigrations = new List<DataMigration>
        {
            new() { Name = "Migration1", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) },
            new() { Name = "Migration2", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) },
            new() { Name = "Migration3", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) }
        };

        var alreadyDoneMigrations = new List<DataMigration>
        {
            new() { Name = "Migration1", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) }
        };

        var migrationsToApply = allMigrations.GetMigrationsToApply(alreadyDoneMigrations).ToList();

        Assert.IsNotNull(migrationsToApply);
        Assert.That(migrationsToApply, Has.Count.EqualTo(2));
        Assert.That(migrationsToApply, Has.None.Matches<DataMigration>(m => m.Name == "Migration1"));
    }

    [Test]
    public void AlsoTakesIntoAccountDateWhenGettingMigrationsToApply()
    {
        var allMigrations = new List<DataMigration>
        {
            new() { Name = "Migration1", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) },
            new() { Name = "Migration1", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 6) },
            new() { Name = "Migration3", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) }
        };

        var alreadyDoneMigrations = new List<DataMigration>
        {
            new() { Name = "Migration1", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) },
            new() { Name = "Migration3", CreatedTime = new DateTime(2022, 2, 2, 5, 5, 5) }
        };

        var migrationsToApply = allMigrations.GetMigrationsToApply(alreadyDoneMigrations).ToList();

        Assert.IsNotNull(migrationsToApply);
        Assert.That(migrationsToApply, Has.Count.EqualTo(1));
        Assert.That(migrationsToApply,
            Has.Some.Matches<DataMigration>(m => m.Name == "Migration1" && m.CreatedTime.Second == 6));
    }
}