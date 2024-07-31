# Data Migration

## Running the Migrator

To run the migrator, you need to pass in the following arguments:

1. The action you want to take (more on this below)
2. The version you want to migrate to (if applicable)

Additionally, an appsettings.json (or appsettings.{ENVIRONMENT}.json) file is required in the same directory as the
migrator. The file should look like
this:

```json
{
  "Database": {
    "ConnectionString": "your-connection-string-here",
    "DatabaseType": "SqlServer"
  }
}
```

### Actions

#### Migrate (NOT IMPLEMENTED)

Usage:

```shell
dotnet run -migrate <version>
```

The migrate action will run all migrations up to the version specified in the arguments. If no version is specified, it
will run all migrations that need to be run.

#### Rollback (NOT IMPLEMENTED)

Usage:

```shell
dotnet run -rollback <version>
```

The rollback action will run all migrations down to the version specified in the arguments. If no version is specified,
it will roll back to the previous migration.

#### Generate

Usage:

```
dotnet run -generate
```

The generate action will create a new migration file in the Migrations directory. A migration file is a pair of .sql
files with the naming convention of `{number}_{name}_up.sql` and `{number}_{name}_down.sql`. The number should be the
next number in the sequence of migrations. The name comes from a prompt that will ask you for the name of the migration.

#### Drop (NOT IMPLEMENTED)

Usage:

```shell
dotnet run -drop
```

The drop action will drop the database specified in the appsettings.json file.

#### Info (NOT IMPLEMENTED)

Usage:

```shell
dotnet run -info
```

The info action will print out the current version of the database, as well as what migrations need to be run against
the database. This can also be a useful command to run to test the connection to the database.

#### Help (NOT IMPLEMENTED)

```shell
dotnet run -help
```

The help action will print out the help text, describing what actions can be taken.

## How to set up on a new project

1. Make a new console project that references the Slowback.Data.Core.Migrator package.
2. In your Program.cs file, add the following code:

```csharp
using Slowback.Data.Core.Migrator;
using Slowback.Data.Core.Migrator.DirectoryProvider;

var workingDirectory = Environment.CurrentDirectory;

var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

if (projectDirectory == null) projectDirectory = Environment.CurrentDirectory; 

var migrationDirectory = Path.Combine(projectDirectory, "Migrations");

if (!Directory.Exists(migrationDirectory)) Directory.CreateDirectory(migrationDirectory);

MigrationDirectoryProvider.MigrationOutputDirectory = migrationDirectory;
MigrationDirectoryProvider.MigrationInputDirectory = Path.Combine(workingDirectory, "Migrations");

return MigrationStartup.StartMigrator(args);
```

3. In your .CsProj file, add the following item group:

```xml
    <ItemGroup>
        <Content Include="Migrations\*.*">
            <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        </Content>
    </ItemGroup>
```

4. Create a new folder in your project called "Migrations".