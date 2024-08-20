using Slowback.Data.Core.Migrator.Generator;
using Slowback.Data.Core.Migrator.Migration;

namespace Slowback.Data.Core.Migrator;

internal static class MigrationInputHandler
{
    public static int HandleInput(string action, string? version)
    {
        switch (action)
        {
            case MigrationActions.Generate:
                return GenerationHandler.HandleGenerate();
            case MigrationActions.Migrate:
                return MigrationHandler.HandleMigration(version);

            default:
                throw new NotImplementedException();
        }
    }
}