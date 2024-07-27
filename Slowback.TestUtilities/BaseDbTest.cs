using NUnit.Framework;

namespace Slowback.TestUtilities;

public class BaseDbTest
{
    protected TestContext _context { get; set; }
    protected TestContext _lookupContext { get; set; }

    [SetUp]
    public void CreateContext()
    {
        _context = new TestContext();
        _lookupContext = new TestContext();
    }
}