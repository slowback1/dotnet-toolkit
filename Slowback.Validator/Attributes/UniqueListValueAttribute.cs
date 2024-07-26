using System.Reflection;

namespace Slowback.Validator.Attributes;

public class UniqueListValueAttribute : ValidationAttribute
{
    private readonly string? _errorMessage;
    private readonly string _propertyName;

    public UniqueListValueAttribute(string propertyName, string? errorMessage = null)
    {
        _propertyName = propertyName;
        _errorMessage = errorMessage;
    }

    public override string? CheckForValidationError(object? value)
    {
        var enumerable = value as IEnumerable<object>;

        if (enumerable is null || !enumerable.Any())
            return null;

        var objects = enumerable.ToList();

        var hasDuplicates = CheckIfListHasDuplicates(objects);


        if (hasDuplicates)
            return ErrorResult();

        return null;
    }

    private bool CheckIfListHasDuplicates(List<object> enumerable)
    {
        var values = GetPropertyValues(enumerable);

        var hasDuplicates = values.GroupBy(v => v).Count() != values.Count();
        return hasDuplicates;
    }

    private string ErrorResult()
    {
        if (_errorMessage != null)
            return _errorMessage;

        return $"Non-unique values found for {_propertyName}";
    }

    private IEnumerable<object?> GetPropertyValues(List<object> enumerable)
    {
        var objectType = GetListType(enumerable);
        var propertyType = GetPropertyType(objectType);

        var values = enumerable.Select(obj => GetValueForProperty(obj, propertyType));
        return values;
    }

    private object? GetValueForProperty(object obj, PropertyInfo propertyValue)
    {
        return propertyValue.GetValue(obj);
    }

    private PropertyInfo GetPropertyType(Type objectType)
    {
        var property = objectType.GetProperty(_propertyName);

        if (property is null)
            throw new InvalidOperationException($"Property Name {_propertyName} is invalid!");

        return property;
    }

    private Type GetListType(IEnumerable<object> list)
    {
        var first = list.First();

        return first.GetType();
    }
}