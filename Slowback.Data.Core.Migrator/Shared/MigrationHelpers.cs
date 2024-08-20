using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Shared;

internal static class MigrationHelpers
{
    public static IEnumerable<DataMigration> SortMigrations(this IEnumerable<DataMigration> migrations)
    {
        return migrations
            .OrderBy(m => m.CreatedTime);
    }

    public static IEnumerable<DataMigration> GetMigrationsToApply(this IEnumerable<DataMigration> allMigrations,
        IEnumerable<DataMigration> alreadyDoneMigrations)
    {
        return allMigrations
            .Except(alreadyDoneMigrations, new DataMigrationComparer())
            .SortMigrations();
    }

    private class DataMigrationComparer : IEqualityComparer<DataMigration>
    {
        public bool Equals(DataMigration x, DataMigration y)
        {
            return x.Name == y.Name && x.CreatedTime == y.CreatedTime;
        }

        public int GetHashCode(DataMigration obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}