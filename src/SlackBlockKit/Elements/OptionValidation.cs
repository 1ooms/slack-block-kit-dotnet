using SlackBlockKit.CompositionObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Provides shared validation logic and constraints for elements that offer a list of options.
/// </summary>
public static class OptionValidation
{
    public const int MaxOptions = 100;

    /// <summary>
    /// Validates that either options or optionGroups is provided, but not both.
    /// </summary>
    public static void ValidateOptionsAndOptionGroups(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        string? targetName = null)
    {
        var target = targetName != null ? $" for {targetName}" : string.Empty;

        if (options == null && optionGroups == null)
            throw new ArgumentException($"Either options or optionGroups must be provided{target}.");

        if (options != null && optionGroups != null)
            throw new ArgumentException($"Provide either options or optionGroups, but not both{target}.");
    }

    /// <summary>
    /// Validates that initialOption matches an option in either options or optionGroups.
    /// </summary>
    public static void ValidateInitialOption(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        OptionObject? initialOption)
    {
        if (initialOption == null)
            return;

        if (!ContainsOption(options, optionGroups, initialOption))
        {
            throw new ArgumentException(
                "InitialOption must match one of the options in either Options or OptionGroups.Options.",
                nameof(initialOption));
        }
    }

    /// <summary>
    /// Validates that all items in initialOptions match an option in either options or optionGroups.
    /// </summary>
    public static void ValidateInitialOptions(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        List<OptionObject>? initialOptions)
    {
        if (initialOptions == null)
            return;

        foreach (var initialOption in initialOptions)
        {
            if (!ContainsOption(options, optionGroups, initialOption))
            {
                throw new ArgumentException(
                    "All OptionObjects within InitialOptions must match an OptionObject in Options or OptionGroups.Options.",
                    nameof(initialOptions));
            }
        }
    }

    /// <summary>
    /// Determines whether the given options or optionGroups contain a matching option.
    /// </summary>
    public static bool ContainsOption(
        List<OptionObject>? options,
        List<OptionGroupObject>? optionGroups,
        OptionObject optionToFind)
    {
        if (options != null)
            return options.Any(option => OptionMatches(option, optionToFind));

        if (optionGroups != null)
            return optionGroups.Any(group => group.Options.Any(option => OptionMatches(option, optionToFind)));

        return false;
    }

    /// <summary>
    /// Determines whether two options match based on instance identity or value and text equivalence.
    /// </summary>
    public static bool OptionMatches(OptionObject option, OptionObject targetOption)
    {
        if (ReferenceEquals(option, targetOption))
            return true;

        if (option.Value != targetOption.Value)
            return false;

        if (option.Text.Text != targetOption.Text.Text)
            return false;

        return true;
    }
}