// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

public class EnumDetail<T> where T : Enum
{
    public string? Description { get; set; }
    public T? Item { get; set; }
    public string? Name { get; set; }
    public int Value { get; set; }
}
public static class EnumUtility
{
    public static bool ContainsName<TEnum>(string? name, bool ignoreCase = false) where TEnum : Enum
    {
        if (name == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetNames<TEnum>().Any(item => item.Contains(name, stringComparison));
    }

    public static bool ContainsValue<TEnum>(string? value, bool ignoreCase = false) where TEnum : Enum
    {
        if (value == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetValuesAsString<TEnum>().Any(item => item.Contains(value, stringComparison));
    }

    public static bool ContainsValue<TEnum>(TEnum value) where TEnum : Enum
    {
        return GetValues<TEnum>().Contains(value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetDescriptions<TEnum>(bool replaceNullWithEnumName = false) where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.GetDescription(replaceNullWithEnumName)).Where(x => x != null)!;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<EnumDetail<T>> GetEnumDetails<T>() where T : Enum
    {
        var result = new List<EnumDetail<T>>();
        var names = Enum.GetNames(typeof(T));
        foreach (var name in names)
        {
            var parsed = Enum.Parse(typeof(T), name);
            var item = (T) parsed;
            var value = Convert.ToInt32(parsed);
            var description = item.GetDescription();
            result.Add(new EnumDetail<T>
            {
                Name = name,
                Value = value,
                Description = description,
                Item = item
            });
        }

        return result;
    }

    public static IEnumerable<EnumDetail<T>> GetEnumDetails<T>(Func<EnumDetail<T>, bool> predicate) where T : Enum
    {
        var result = GetEnumDetails<T>().Where(predicate);
        return result;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetNames<TEnum>() where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
    {
        return (TEnum[]) Enum.GetValues(typeof(TEnum));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetValuesAsString<TEnum>() where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.ToString());
    }

    public static bool IsDefined<TEnum>(this string name) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), name);
    }

    public static bool IsDefined<TEnum>(this TEnum value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }

    public static bool IsInEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetValuesAsString<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    public static bool IsInEnumDescription<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetDescriptions<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    private static string? GetDescription(this Enum @enum, bool returnEnumNameInsteadOfNull = false)
    {
        if (@enum == null) throw new ArgumentNullException(nameof(@enum));

        return
            @enum
                .GetType()
                .GetMember(@enum.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description
            ?? (!returnEnumNameInsteadOfNull ? null : @enum.ToString());
    }
}