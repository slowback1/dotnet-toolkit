using Slowback.Data.Core.Migrator.DirectoryProvider;
using Slowback.TestUtilities;
using Slowback.Time;

namespace Slowback.Data.Core.Migrator.Tests.Generator;

public abstract class GeneratorTestBase
{
    [SetUp]
    [TearDown]
    public void DeleteFileDirectory()
    {
        MigrationDirectoryProvider.MigrationOutputDirectory = Directory.GetCurrentDirectory() + "/Something";

        if (Directory.Exists(MigrationDirectoryProvider.MigrationOutputDirectory))
            Directory.Delete(MigrationDirectoryProvider.MigrationOutputDirectory, true);
    }

    [SetUp]
    public void SetUpTime()
    {
        TimeEnvironment.SetProvider(new TestTimeProvider(new DateTime(2024, 7, 7, 12, 5, 1)));
    }
}