using Elsa.Expressions.Helpers;
using Elsa.Expressions.Models;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

public static class DictionaryExtensions
{
    public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value) => dictionary.TryGetValue<string, T>(key, out value);
    public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, IEnumerable<string> keys, out T value) => dictionary.TryGetValue<string, T>(keys, out value);
    public static bool TryGetValue<T>(this IDictionary<object, object> dictionary, string key, out T value) => dictionary.TryGetValue<object, T>(key, out value);

    public static bool TryGetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, out T value)
    {
        if (!dictionary.TryGetValue(key, out var item))
        {
            value = default!;
            return false;
        }

        value = item;
        return true;
    }
    
    public static bool TryGetValue<TKey, T>(this IDictionary<TKey, object> dictionary, TKey key, out T value)
    {
        if (!dictionary.TryGetValue(key, out var item))
        {
            value = default!;
            return false;
        }

        var result = TryConvertValue<T>(item);
        value = result.Success ? (T)result.Value! : default!;
        return result.Success;
    }
    
    public static bool TryGetValue<TKey, T>(this IDictionary<TKey, object> dictionary, IEnumerable<TKey> keys, out T value)
    {
        foreach (var key in keys)
        {
            if (dictionary.TryGetValue(key, out var item))
            {
                var result = TryConvertValue<T>(item);
                value = result.Success ? (T)result.Value! : default!;
                return result.Success;
            }    
        }
        
        value = default!;
        return false;
    }

    public static T? GetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key) => ConvertValue<T>(dictionary[key]);
    public static T? GetValue<T>(this IDictionary<string, object> dictionary, string key) => ConvertValue<T>(dictionary[key]);
    public static T? GetValueOrDefault<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, Func<T?> defaultValueFactory) => TryGetValue(dictionary, key, out var value) ? value : defaultValueFactory();
    
    public static T? GetValueOrDefault<TKey, T>(this IDictionary<TKey, object> dictionary, TKey key, Func<T?> defaultValueFactory) => TryGetValue<TKey, T>(dictionary, key, out var value) ? value : defaultValueFactory();
    public static T? GetValueOrDefault<TKey, T>(this IDictionary<TKey, object> dictionary, TKey key) => GetValueOrDefault<TKey, T>(dictionary, key, () => default);
    public static T? GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, Func<T?> defaultValueFactory) => TryGetValue<T>(dictionary, key, out var value) ? value : defaultValueFactory();
    public static T? GetValueOrDefault<T>(this IDictionary<string, object> dictionary, IEnumerable<string> keys, Func<T?> defaultValueFactory) => TryGetValue<T>(dictionary, keys, out var value) ? value : defaultValueFactory();
    public static T? GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key) => GetValueOrDefault<T>(dictionary, key, () => default);
    public static object? GetValueOrDefault(this IDictionary<string, object> dictionary, string key) => GetValueOrDefault<object>(dictionary, key, () => null);
    
    public static T GetOrAdd<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, Func<T> valueFactory)
    {
        if(dictionary.TryGetValue(key, out T? value))
           return value;

        value = valueFactory()!;
        dictionary.Add(key, value);
        return value;
    }
    
    public static T GetOrAdd<TKey, T>(this IDictionary<TKey, object> dictionary, TKey key, Func<T> valueFactory)
    {
        if (dictionary.TryGetValue<TKey, T>(key, out var value))
            return value!;

        value = valueFactory()!;
        dictionary.Add(key, value);
        return value;
    }

    public static IDictionary<string, object> AddInput<T>(this IDictionary<string, object> dictionary, T value) where T : notnull => dictionary.AddInput(typeof(T).Name, value);

    public static IDictionary<string, object> AddInput(this IDictionary<string, object> dictionary, string key, object value)
    {
        dictionary.Add(key, value);
        return dictionary;
    }
    
    /// <summary>
    /// Merges the specified dictionary with the other dictionary.
    /// When a key exists in both dictionaries, the value in the other dictionary will overwrite the value in the specified dictionary.
    /// </summary>
    public static void Merge(this IDictionary<string, object> dictionary, IDictionary<string, object> other)
    {
        foreach (var (key, value) in other)
            dictionary[key] = value;
    }

    private static T? ConvertValue<T>(object? value) => value.ConvertTo<T>();
    
    private static Result TryConvertValue<T>(object? value)
    {
        return value.TryConvertTo<T>();
    }
}