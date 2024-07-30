using Newtonsoft.Json;
using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationFileWriterTests : GeneratorTestBase
{
    [Test]
    public void WriteCreatesMigrationFiles()
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

        var directory = MigrationDirectoryProvider.Directory;

        Assert.IsTrue(File.Exists($"{directory}/MigrationHistory.json"));

        var fileContents = File.ReadAllText($"{directory}/MigrationHistory.json");

        var parsed = JsonConvert.DeserializeObject<List<DataMigration>>(fileContents);

        Assert.That(parsed.Count, Is.EqualTo(1));

        Assert.That(parsed[0].Number, Is.EqualTo(1));
        Assert.That(parsed[0].Name, Is.EqualTo("Test"));
        Assert.That(parsed[0].UpFileName, Is.EqualTo("1_Test_Up.sql"));
        Assert.That(parsed[0].DownFileName, Is.EqualTo("1_Test_Down.sql"));
    }

    [Test]
    public void WriteAppendsToExistingMigrationFile()
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
        MigrationFileWriter.Write(migration);

        var readResults = MigrationFileReader.Read().ToList();

        Assert.That(readResults.Count, Is.EqualTo(2));
    }
}