namespace Slowback.SampleProject.Data.Core.Models;

public class ToDo
{
    public int ToDoId { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}