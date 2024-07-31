using Slowback.Data.Core.Migrator;
using Slowback.Data.Core.Migrator.DirectoryProvider;

var workingDirectory = Environment.CurrentDirectory;

var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

if (projectDirectory == null) projectDirectory = workingDirectory;

var migrationDirectory = Path.Combine(projectDirectory, "Migrations");

if (!Directory.Exists(migrationDirectory)) Directory.CreateDirectory(migrationDirectory);

MigrationDirectoryProvider.MigrationOutputDirectory = migrationDirectory;
MigrationDirectoryProvider.MigrationInputDirectory = Path.Combine(workingDirectory, "Migrations");

return MigrationStartup.StartMigrator(args);