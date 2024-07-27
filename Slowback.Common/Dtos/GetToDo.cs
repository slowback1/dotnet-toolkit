namespace Slowback.Common.Dtos;

public class GetToDo
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}