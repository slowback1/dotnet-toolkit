using System.ComponentModel.DataAnnotations;

namespace SampleDatabase.Models;

public class SampleEntity
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
