global using static SlackBlockKit.Validation;

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
}
