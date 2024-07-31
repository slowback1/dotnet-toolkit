using Slowback.Data.Core.Migrator.Generator;

namespace Slowback.Data.Core.Migrator;

internal static class MigrationInputHandler
{
    public static int HandleInput(string action, string? version)
    {
        switch (action)
        {
            case MigrationActions.Generate:
                return GenerationHandler.HandleGenerate();

            default:
                throw new NotImplementedException();
        }
    }
}