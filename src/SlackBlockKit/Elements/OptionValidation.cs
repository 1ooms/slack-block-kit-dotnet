using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Provides shared validation logic and constraints for elements that offer a list of options.
/// </summary>
internal static class OptionValidation
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
    /// Determines whether two options match exactly, as Slack requires for initial options:
    /// same instance, or equal value, text (including its type), description and url.
    /// </summary>
    public static bool OptionMatches(OptionObject option, OptionObject targetOption)
    {
        if (ReferenceEquals(option, targetOption))
            return true;

        return option.Value == targetOption.Value
               && TextMatches(option.Text, targetOption.Text)
               && TextMatches(option.Description, targetOption.Description)
               && option.Url == targetOption.Url;
    }

    private static bool TextMatches(TextObject? text, TextObject? targetText)
    {
        if (text == null || targetText == null)
            return text == null && targetText == null;

        return text.Type == targetText.Type && text.Text == targetText.Text;
    }
}