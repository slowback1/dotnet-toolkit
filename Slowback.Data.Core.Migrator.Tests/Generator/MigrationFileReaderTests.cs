using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationFileReaderTests : GeneratorTestBase
{
    [Test]
    public void ReadReturnsAnEmptyListWhenNoFilesAreFound()
    {
        var migrations = MigrationFileReader.Read();
        Assert.IsEmpty(migrations);
    }

    [Test]
    public void ReadsFromTheCsvFileIfItIsPresent()
    {
        var migration = new DataMigration
        {
            Name = "Test",
            CreatedTime = DateTime.Now,
            UpFileName = "1_Test_Up.sql",
            DownFileName = "1_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration);

        var migrations = MigrationFileReader.Read().ToList();

        Assert.That(migrations.Count, Is.EqualTo(1));

        Assert.That(migrations[0].Name, Is.EqualTo("Test"));
        Assert.That(migrations[0].UpFileName, Is.EqualTo("1_Test_Up.sql"));
        Assert.That(migrations[0].DownFileName, Is.EqualTo("1_Test_Down.sql"));
    }
}