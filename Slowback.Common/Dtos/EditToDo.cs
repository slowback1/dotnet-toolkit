using Slowback.Validator.Attributes;

namespace Slowback.Common.Dtos;

public class EditToDo
{
    [Required]
    [NumericValue(1)]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public bool IsComplete { get; set; }
}