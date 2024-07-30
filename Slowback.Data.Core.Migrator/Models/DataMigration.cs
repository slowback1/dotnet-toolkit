namespace Slowback.Data.Core.Migrator.Models;

internal class DataMigration
{
    public int Number { get; set; }
    public string Name { get; set; }
    public DateTime TimeStamp { get; set; }
    public string UpFileName { get; set; }
    public string DownFileName { get; set; }
}