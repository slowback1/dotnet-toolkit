namespace Validator.Attributes;

public class RequiredAttribute : ValidationAttribute
{
    private readonly string? _name;

    public RequiredAttribute(string? name = null)
    {
        _name = name;
    }

    public override string? CheckForValidationError(object? value)
    {
        if (value is null || IsEmptyString(value))
            return $"'{_name ?? PropertyName}' is required.";

        return null;
    }

    private bool IsEmptyString(object? value)
    {
        if (value is null) return false;

        if (value.GetType() != typeof(string))
            return false;

        var stringValue = (string)value;
        return string.IsNullOrWhiteSpace(stringValue);
    }
}