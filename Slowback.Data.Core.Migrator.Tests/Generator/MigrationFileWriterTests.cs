using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationFileWriterTests : GeneratorTestBase
{
    [Test]
    public void WriteCreatesMigrationFiles()
    {
        var migration = new DataMigration
        {
            Name = "Test",
            CreatedTime = TimeEnvironment.Provider.Now(),
            UpFileName = "1_Test_Up.sql",
            DownFileName = "1_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration);

        var directory = MigrationDirectoryProvider.MigrationOutputDirectory;

        Assert.IsTrue(File.Exists($"{directory}/MigrationHistory.csv"));

        var fileContents = File.ReadAllText($"{directory}/MigrationHistory.csv");

        var parsed = MigrationFileReader.Read().ToList();

        Assert.That(parsed.Count, Is.EqualTo(1));

        Assert.That(parsed[0].Name, Is.EqualTo("Test"));
        Assert.That(parsed[0].UpFileName, Is.EqualTo("1_Test_Up.sql"));
        Assert.That(parsed[0].DownFileName, Is.EqualTo("1_Test_Down.sql"));
    }

    [Test]
    public void WriteAppendsToExistingMigrationFile()
    {
        var migration = new DataMigration
        {
            Name = "Test",
            CreatedTime = TimeEnvironment.Provider.Now(),
            UpFileName = "1_Test_Up.sql",
            DownFileName = "1_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration);
        MigrationFileWriter.Write(migration);

        var readResults = MigrationFileReader.Read().ToList();

        Assert.That(readResults.Count, Is.EqualTo(2));
    }
}