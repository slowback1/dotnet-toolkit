using Slowback.Data.Core.EF;

namespace Slowback.TestUtilities;

public class TestDbContextOptions
{
    public static readonly ConnectionOptions InMemoryOptions = new()
    {
        DatabaseType = DatabaseType.InMemory
    };
}