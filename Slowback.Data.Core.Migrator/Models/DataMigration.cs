namespace Slowback.Data.Core.Migrator.Models;

internal class DataMigration
{
    public string Name { get; set; }
    public DateTime CreatedTime { get; set; }
    public string UpFileName { get; set; }
    public string DownFileName { get; set; }
}