using System.ComponentModel.DataAnnotations;

namespace Slowback.SampleProject.Data.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }
}