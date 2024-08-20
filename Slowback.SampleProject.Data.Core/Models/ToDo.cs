using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slowback.SampleProject.Data.Core.Models;

public class ToDo
{
    [ForeignKey("Users")]
    public Guid UserId { get; set; }

    [Key]
    public int ToDoId { get; set; }

    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}