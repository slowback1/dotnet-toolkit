using Slowback.SampleProject.Data.UnitOfWork.Mock;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.Data.UnitOfWork.Tests;

public class UnitOfWorkFactoryTests
{
    [Test]
    [TestCase(UnitOfWorkType.Mock, typeof(MockUnitOfWork))]
    [TestCase(UnitOfWorkType.Real, typeof(UnitOfWork))]
    public void GetUnitOfWorkReturnsTheRightImplementationBasedOnSettings(UnitOfWorkType type, Type expectedType)
    {
        // Arrange
        var settings = new UnitOfWorkSettings { Type = type, ConnectionOptions = TestDbContextOptions.InMemoryOptions };

        // Act
        var result = UnitOfWorkFactory.GetUnitOfWork(settings);

        // Assert
        Assert.IsInstanceOf(expectedType, result);
    }

    [Test]
    public void GetUnitOfWork_InvalidType_ThrowsException()
    {
        // Arrange
        var settings = new UnitOfWorkSettings { Type = (UnitOfWorkType)int.MaxValue };

        // Act
        void Act()
        {
            UnitOfWorkFactory.GetUnitOfWork(settings);
        }

        // Assert
        Assert.Throws<InvalidUnitOfWorkSettingsException>(Act);
    }

    [Test]
    public void GetUnitOfWorkThrowsInvalidSettingsExceptionWhenConnectionOptionsAreNullForTheRealUnitOfWork()
    {
        // Arrange
        var settings = new UnitOfWorkSettings { Type = UnitOfWorkType.Real };

        // Act
        void Act()
        {
            UnitOfWorkFactory.GetUnitOfWork(settings);
        }

        // Assert
        Assert.Throws<InvalidUnitOfWorkSettingsException>(Act);
    }
}