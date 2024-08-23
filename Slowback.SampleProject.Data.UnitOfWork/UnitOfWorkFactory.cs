using Slowback.Data.Core.EF;
using Slowback.SampleProject.Data.Core;
using Slowback.SampleProject.Data.UnitOfWork.Mock;

namespace Slowback.SampleProject.Data.UnitOfWork;

public enum UnitOfWorkType
{
    Mock,
    Real
}

public struct UnitOfWorkSettings
{
    public UnitOfWorkType Type { get; set; }
    public ConnectionOptions? ConnectionOptions { get; set; }
}

public static class UnitOfWorkFactory
{
    public static IUnitOfWork GetUnitOfWork(UnitOfWorkSettings settings)
    {
        switch (settings.Type)
        {
            case UnitOfWorkType.Mock:
                return new MockUnitOfWork();
            case UnitOfWorkType.Real:
                return CreateRealUnitOfWork(settings);
        }

        throw new InvalidUnitOfWorkSettingsException();
    }

    private static UnitOfWork CreateRealUnitOfWork(UnitOfWorkSettings settings)
    {
        if (settings.ConnectionOptions == null) throw new InvalidUnitOfWorkSettingsException();

        return new UnitOfWork(new SampleAppContext(settings.ConnectionOptions));
    }
}

public class InvalidUnitOfWorkSettingsException : Exception
{
}