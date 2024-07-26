namespace Slowback.Validator.Attributes;

public abstract class ValidationAttribute : Attribute
{
    public string PropertyName { get; set; }

    public abstract string? CheckForValidationError(object? value);
}