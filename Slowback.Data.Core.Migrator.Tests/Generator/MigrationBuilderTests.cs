using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationBuilderTests : GeneratorTestBase
{
    [Test]
    public void BuildMigration_WhenCalled_ReturnsMigration()
    {
        var migration = MigrationBuilder.BuildMigration("Test");

        Assert.IsNotNull(migration);

        Assert.That(migration.Name, Is.EqualTo("Test"));
        Assert.That(migration.Number, Is.EqualTo(1));
        Assert.That(migration.UpFileName, Is.EqualTo("1_Test_Up.sql"));
        Assert.That(migration.DownFileName, Is.EqualTo("1_Test_Down.sql"));
        Assert.That(migration.TimeStamp, Is.EqualTo(TimeEnvironment.Provider.Now()));
    }

    [Test]
    public void BuildMigration_CorrectlySetsTheNumberWhenThereIsAlreadyAMigration()
    {
        var migration = new DataMigration
        {
            Number = 1,
            Name = "Test",
            TimeStamp = DateTime.Now,
            UpFileName = "1_Test_Up.sql",
            DownFileName = "1_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration);

        var newMigration = MigrationBuilder.BuildMigration("Test");

        Assert.That(newMigration.Number, Is.EqualTo(2));
        Assert.That(newMigration.DownFileName, Is.EqualTo("2_Test_Down.sql"));
        Assert.That(newMigration.UpFileName, Is.EqualTo("2_Test_Up.sql"));
    }
}