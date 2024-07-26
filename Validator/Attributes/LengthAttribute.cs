using System.Collections;
using System.Text;

namespace Validator.Attributes;

public class LengthAttribute : ValidationAttribute
{
    private readonly int _max;
    private readonly int _min;

    public LengthAttribute(int max = int.MaxValue, int min = 0)
    {
        _max = max;
        _min = min;
    }

    public override string? CheckForValidationError(object? value)
    {
        if (value is null) return null;
        if (!IsString(value) && !IsEnumerable(value))
            throw new InvalidOperationException();

        var length = GetLengthOfObject(value);
        var stringValue = GetStringValueOfObject(value);

        if (length > _max)
            return $"{stringValue} is longer than {_max} {GetCountSuffix(value)}.";
        if (length < _min)
            return $"{stringValue} is shorter than {_min} {GetCountSuffix(value)}.";

        return null;
    }

    private int GetLengthOfObject(object value)
    {
        return IsString(value)
            ? GetStringLength(value)
            : GetEnumerableLength(value as IEnumerable<object> ?? Array.Empty<object>());
    }

    private int GetEnumerableLength(IEnumerable<object> value)
    {
        return value.Count();
    }

    private int GetStringLength(object value)
    {
        var stringValue = value.ToString();
        var length = stringValue.Length;

        return length;
    }

    private string GetStringValueOfObject(object value)
    {
        if (IsString(value))
            return $"'{value}'";

        return BuildIEnumerableToString(value as IEnumerable<object> ?? Array.Empty<object>());
    }

    private string BuildIEnumerableToString(IEnumerable<object> value)
    {
        var stringBuilder = new StringBuilder("List (");

        foreach (var item in value) stringBuilder.Append($"'{item.ToString()}', ");

        stringBuilder.Remove(stringBuilder.Length - 2, 2);

        stringBuilder.Append(")");

        return stringBuilder.ToString();
    }

    private string GetCountSuffix(object value)
    {
        return IsString(value) ? "characters" : "items";
    }

    private bool IsEnumerable(object value)
    {
        return value is IEnumerable;
    }

    private bool IsString(object value)
    {
        return value is string;
    }
}