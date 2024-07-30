using System;
using System.Collections;
using System.Reflection;

namespace Slowback.Mapper;

public static class ObjectMapper
{
    public static void Map<T>(this T source, T destination)
    {
        if (source == null || destination == null)
            throw new ArgumentNullException("Source or/and Destination objects are null");

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (ShouldIgnore(property))
                continue;

            var value = property.GetValue(source);
            property.SetValue(destination, value);
        }
    }

    private static bool ShouldIgnore(PropertyInfo property)
    {
        return !CanReadAndWrite(property) ||
               IsClass(property.PropertyType) ||
               IsEnumerable(property.PropertyType) ||
               HasIgnoreAttribute(property);
    }

    private static bool HasIgnoreAttribute(PropertyInfo property)
    {
        return property.GetCustomAttribute<MapperIgnoreAttribute>() != null;
    }

    private static bool CanReadAndWrite(PropertyInfo property)
    {
        return property is { CanRead: true, CanWrite: true };
    }

    private static bool IsEnumerable(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }

    private static bool IsClass(Type type)
    {
        return type.IsClass && type != typeof(string);
    }
}