// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public static class ReflectionUtility
{
    public static TProperty? GetProperty<TClass, TProperty>(TClass instanceType, string propertyName)
        where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return (TProperty?)obj;
    }

    public static object? GetProperty(Type instanceType, string propertyName)
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        object? obj = null;
        var info = instanceType.GetTypeInfo().GetProperty(propertyName);
        if (info != null)
            obj = info.GetValue(instanceType, null);
        return obj;
    }

    public static void SetProperty<TClass>(TClass instanceType, string propertyName, object propertyValue)
        where TClass : class
    {
        if (propertyName == null || string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var type = instanceType.GetType();
        var info = type.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }

    public static void SetProperty(Type instanceType, string propertyName, object propertyValue)
    {
        if (propertyName == null || string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentNullException(nameof(propertyName), "Value can not be null or empty.");

        var info = instanceType.GetTypeInfo().GetProperty(propertyName);

        if (info != null)
            info.SetValue(instanceType, Convert.ChangeType(propertyValue, info.PropertyType), null);
    }
}