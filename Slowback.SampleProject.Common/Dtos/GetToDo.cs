using System;

namespace Slowback.SampleProject.Common.Dtos;

public class GetToDo
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}