using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class MigrationNumberRetrieverTests : GeneratorTestBase
{
    [Test]
    public void GetNextMigrationNumberReturnsOneIfNoMigrationsExist()
    {
        var nextNumber = MigrationNumberRetriever.GetNextMigrationNumber();
        Assert.That(nextNumber, Is.EqualTo(1));
    }

    [Test]
    [TestCase(1, 2)]
    [TestCase(2, 3)]
    [TestCase(3, 4)]
    [TestCase(4, 5)]
    public void GetNextMigrationNumberReturnsTheNextNumber(int numberOfMigrations, int expected)
    {
        for (var i = 0; i < numberOfMigrations; i++)
        {
            var migration = new DataMigration
            {
                Number = i + 1,
                Name = $"Test {i + 1}",
                TimeStamp = DateTime.Now,
                UpFileName = $"{i + 1}_Test_Up.sql",
                DownFileName = $"{i + 1}_Test_Down.sql"
            };

            MigrationFileWriter.Write(migration);
        }

        var nextNumber = MigrationNumberRetriever.GetNextMigrationNumber();
        Assert.That(nextNumber, Is.EqualTo(expected));
    }

    [Test]
    public void UsesTheLatestMigrationNumberInsteadOfJustTheCount()
    {
        var migration = new DataMigration
        {
            Number = 5,
            Name = "Test",
            TimeStamp = DateTime.Now,
            UpFileName = "5_Test_Up.sql",
            DownFileName = "5_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration);

        var migration2 = new DataMigration
        {
            Number = 9,
            Name = "Test",
            TimeStamp = DateTime.Now,
            UpFileName = "6_Test_Up.sql",
            DownFileName = "6_Test_Down.sql"
        };

        MigrationFileWriter.Write(migration2);

        var nextNumber = MigrationNumberRetriever.GetNextMigrationNumber();
        Assert.That(nextNumber, Is.EqualTo(10));
    }
}