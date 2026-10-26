global using static SlackBlockKit.Validation;

using System.Collections;
using System.Runtime.CompilerServices;

namespace SlackBlockKit;

/// <summary>
/// Provides shared validation helpers for Slack Block Kit components.
/// </summary>
public static class Validation
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if <paramref name="value"/> is an empty string.
    /// Does not throw if <paramref name="value"/> is <c>null</c>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is an empty string.</exception>
    public static string? ThrowIfEmpty(string? value, [CallerMemberName] string? propertyName = null)
    {
        if (value is { Length: 0 })
            throw new ArgumentException($"{propertyName} cannot be an empty string.", propertyName);

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is <c>null</c>,
    /// or an <see cref="ArgumentException"/> if <paramref name="value"/> is an empty string.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated non-null string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is an empty string.</exception>
    public static string ThrowIfNullOrEmpty(string? value, [CallerMemberName] string? propertyName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, propertyName);
        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if <paramref name="value"/> exceeds <paramref name="maxLength"/> characters.
    /// Does not throw if <paramref name="value"/> is <c>null</c>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="maxLength">The maximum allowed character length.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> exceeds <paramref name="maxLength"/> characters.</exception>
    public static string? ThrowIfExceedsLength(string? value, int maxLength, [CallerMemberName] string? propertyName = null)
    {
        if (value != null && value.Length > maxLength)
            throw new ArgumentException($"{propertyName} cannot exceed {maxLength} characters.", propertyName);

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if <paramref name="collection"/> is not <c>null</c> but contains 0 items.
    /// Does not throw if <paramref name="collection"/> is <c>null</c>.
    /// </summary>
    /// <typeparam name="TCollection">The collection type.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated collection.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="collection"/> contains 0 items.</exception>
    public static TCollection? ThrowIfEmpty<TCollection>(
        TCollection? collection,
        [CallerMemberName] string? propertyName = null)
        where TCollection : class, ICollection
    {
        if (collection != null && collection.Count == 0)
            throw new ArgumentException($"{propertyName} cannot be empty.", propertyName);

        return collection;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="collection"/> is <c>null</c>,
    /// or an <see cref="ArgumentException"/> if <paramref name="collection"/> contains 0 items.
    /// </summary>
    /// <typeparam name="TCollection">The collection type.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated non-null collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="collection"/> contains 0 items.</exception>
    public static TCollection ThrowIfNullOrEmpty<TCollection>(
        TCollection? collection,
        [CallerMemberName] string? propertyName = null)
        where TCollection : class, ICollection
    {
        ArgumentNullException.ThrowIfNull(collection, propertyName);

        if (collection.Count == 0)
            throw new ArgumentException($"{propertyName} cannot be empty.", propertyName);

        return collection;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if <paramref name="collection"/> contains more than <paramref name="maxCount"/> items.
    /// Does not throw if <paramref name="collection"/> is <c>null</c>.
    /// </summary>
    /// <typeparam name="TCollection">The collection type.</typeparam>
    /// <param name="collection">The collection to validate.</param>
    /// <param name="maxCount">The maximum allowed number of items.</param>
    /// <param name="propertyName">The name of the property being validated, automatically captured.</param>
    /// <returns>The validated collection.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="collection"/> contains more than <paramref name="maxCount"/> items.</exception>
    public static TCollection? ThrowIfExceedsCount<TCollection>(
        TCollection? collection,
        int maxCount,
        [CallerMemberName] string? propertyName = null)
        where TCollection : class, ICollection
    {
        if (collection != null && collection.Count > maxCount)
            throw new ArgumentException($"{propertyName} cannot contain more than {maxCount} items.", propertyName);

        return collection;
    }
}
