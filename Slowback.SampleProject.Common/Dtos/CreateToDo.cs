using Slowback.Validator.Attributes;

namespace Slowback.SampleProject.Common.Dtos;

public class CreateToDo
{
    [Required]
    public string Description { get; set; }
}