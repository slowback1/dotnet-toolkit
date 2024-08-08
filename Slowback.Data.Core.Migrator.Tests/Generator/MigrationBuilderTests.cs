using Slowback.Data.Core.Migrator.Generator;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationBuilderTests : GeneratorTestBase
{
    private const string ExpectedTimestamp = "20240707120501";

    [Test]
    public void BuildMigration_WhenCalled_ReturnsMigration()
    {
        var migration = MigrationBuilder.BuildMigration("Test");

        Assert.IsNotNull(migration);

        Assert.That(migration.Name, Is.EqualTo("Test"));
        Assert.That(migration.UpFileName, Is.EqualTo($"{ExpectedTimestamp}_Test_Up.sql"));
        Assert.That(migration.DownFileName, Is.EqualTo($"{ExpectedTimestamp}_Test_Down.sql"));
        Assert.That(migration.CreatedTime, Is.EqualTo(TimeEnvironment.Provider.Now()));
    }
}