namespace Slowback.Validator.Attributes;

public class NumericValueAttribute : ValidationAttribute
{
    protected readonly float Max;
    protected readonly float Min;
    private readonly string NameOverride;


    public NumericValueAttribute(float min = float.MinValue, float max = float.MaxValue, string propertyName = "")
    {
        Min = min;
        Max = max;
        NameOverride = propertyName;

        if (Max < Min)
            throw new ArgumentException("Max value cannot be less than min value");
    }

    public NumericValueAttribute() : this(float.MinValue)
    {
    }

    public NumericValueAttribute(int min = int.MinValue, int max = int.MaxValue, string propertyName = "")
        : this(min, (float)max, propertyName)
    {
    }

    public NumericValueAttribute(decimal min = decimal.MinValue, decimal max = decimal.MaxValue,
        string propertyName = "")
        : this((float)min, (float)max, propertyName)
    {
    }

    public NumericValueAttribute(long min = long.MinValue, long max = long.MaxValue, string propertyName = "")
        : this(min, (float)max, propertyName)
    {
    }

    public override string? CheckForValidationError(object? value)
    {
        var isNumeric = float.TryParse(value?.ToString(), out var number);

        if (!isNumeric)
            return "Value is not a number";

        var propertyName = GetPropertyName();

        if (number < Min)
            return $"{propertyName} is below minimum of {Min}";

        if (number > Max)
            return $"{propertyName} is above maximum value of {Max}";

        return null;
    }

    private string GetPropertyName()
    {
        return string.IsNullOrWhiteSpace(NameOverride) ? PropertyName : NameOverride;
    }
}