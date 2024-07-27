using Slowback.SampleProject.Data.Core;

namespace Slowback.TestUtilities;

public class TestContext : SampleAppContext
{
    public TestContext() : base(TestDbContextOptions.InMemoryOptions)
    {
    }
}