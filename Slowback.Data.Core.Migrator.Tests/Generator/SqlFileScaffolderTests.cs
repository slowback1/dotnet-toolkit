using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Models;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public class SqlFileScaffolderTests : GeneratorTestBase
{
    [Test]
    [TestCase("1_Initial_Up.sql")]
    [TestCase("1_Initial_Down.sql")]
    public void MakesSqlFiles(string fileName)
    {
        var migration = new DataMigration
        {
            UpFileName = "1_Initial_Up.sql",
            Name = "Initial",
            Number = 1,
            TimeStamp = TimeEnvironment.Provider.Now(),
            DownFileName = "1_Initial_Down.sql"
        };

        var scaffolder = new SqlFileScaffolder(migration);
        scaffolder.Scaffold();

        var filePath = Path.Combine(MigrationDirectoryProvider.Directory, fileName);
        Assert.True(File.Exists(filePath));
    }

    [Test]
    [TestCase("1_Initial_Up.sql")]
    [TestCase("1_Initial_Down.sql")]
    public void SqlFilesHaveContentDepictingThatTheUserShouldWriteSql(string fileName)
    {
        var migration = new DataMigration
        {
            UpFileName = "1_Initial_Up.sql",
            Name = "Initial",
            Number = 1,
            TimeStamp = TimeEnvironment.Provider.Now(),
            DownFileName = "1_Initial_Down.sql"
        };

        var scaffolder = new SqlFileScaffolder(migration);
        scaffolder.Scaffold();

        var filePath = Path.Combine(MigrationDirectoryProvider.Directory, fileName);
        var fileContent = File.ReadAllText(filePath);

        Assert.That(fileContent, Contains.Substring("-- Write your SQL here"));
    }

    [Test]
    [TestCase("1_Initial_Up.sql")]
    [TestCase("1_Initial_Down.sql")]
    public void SqlFilesHaveContentThatThrowsASqlException(string fileName)
    {
        var migration = new DataMigration
        {
            UpFileName = "1_Initial_Up.sql",
            Name = "Initial",
            Number = 1,
            TimeStamp = TimeEnvironment.Provider.Now(),
            DownFileName = "1_Initial_Down.sql"
        };

        var scaffolder = new SqlFileScaffolder(migration);
        scaffolder.Scaffold();

        var filePath = Path.Combine(MigrationDirectoryProvider.Directory, fileName);
        var fileContent = File.ReadAllText(filePath);

        Assert.That(fileContent, Contains.Substring("THROW 51000, 'This is a placeholder for your SQL.', 1"));
    }
}