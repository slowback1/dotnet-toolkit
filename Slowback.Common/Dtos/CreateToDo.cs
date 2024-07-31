using Slowback.Validator.Attributes;

namespace Slowback.Common.Dtos;

public class CreateToDo
{
    [Required]
    public string Description { get; set; }
}