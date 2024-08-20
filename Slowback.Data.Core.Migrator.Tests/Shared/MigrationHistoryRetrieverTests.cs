using Slowback.Data.Core.Migrator.Models;
using Slowback.Data.Core.Migrator.Shared;
using Slowback.TestUtilities;

namespace Slowback.Data.Core.Migrator.Tests.Shared;

public class MigrationHistoryRetrieverTests
{
    private readonly MigrationContext _context = new(TestDbContextOptions.InMemoryOptions);

    [Test]
    public void GetAlreadyDoneMigrations_ReturnsMigrations()
    {
        var retriever = new MigrationHistoryRetriever(_context);
        var migrations = retriever.GetAlreadyDoneMigrations();

        Assert.IsNotNull(migrations);
        Assert.That(migrations, Is.TypeOf<List<DataMigration>>());
    }
}