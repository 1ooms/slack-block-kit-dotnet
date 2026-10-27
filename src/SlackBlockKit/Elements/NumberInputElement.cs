using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a number input element that allows users to enter numeric data into a single-line field.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/number-input-element">Slack Number Input Element Documentation</seealso>
public partial class NumberInputElement : ActionElement, IInputElement
{
    private const int MaxPlaceholderLength = 150;

    [JsonConstructor]
    public NumberInputElement(
        bool isDecimalAllowed = false,
        string? actionId = null,
        string? initialValue = null,
        string? minValue = null,
        string? maxValue = null,
        DispatchActionConfigurationObject? dispatchActionConfig = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        Validate(isDecimalAllowed, initialValue, minValue, maxValue);

        IsDecimalAllowed = isDecimalAllowed;
        ActionId = actionId;
        MinValue = minValue;
        MaxValue = maxValue;
        InitialValue = initialValue;
        DispatchActionConfig = dispatchActionConfig;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "number_input";

    [JsonPropertyName("is_decimal_allowed")]
    public bool IsDecimalAllowed
    {
        get;
        set
        {
            Validate(value, InitialValue, MinValue, MaxValue);
            field = value;
        }
    }

    [JsonPropertyName("initial_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialValue
    {
        get;
        set
        {
            Validate(IsDecimalAllowed, value, MinValue, MaxValue);
            field = value;
        }
    }

    [JsonPropertyName("min_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MinValue
    {
        get;
        set
        {
            Validate(IsDecimalAllowed, InitialValue, value, MaxValue);
            field = value;
        }
    }

    [JsonPropertyName("max_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MaxValue
    {
        get;
        set
        {
            Validate(IsDecimalAllowed, InitialValue, MinValue, value);
            field = value;
        }
    }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("dispatch_action_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    [GeneratedRegex(@"^[+-]?\d+$")]
    private static partial Regex IntegerRegex();

    [GeneratedRegex(@"^[+-]?\d+(\.\d+)?$")]
    private static partial Regex NumberRegex();

    private static void Validate(bool isDecimalAllowed, string? initialValue, string? minValue, string? maxValue)
    {
        decimal? parsedInitial = null;
        decimal? parsedMin = null;
        decimal? parsedMax = null;

        if (initialValue != null)
            parsedInitial = ParseAndValidateNumber(initialValue, isDecimalAllowed, nameof(InitialValue));

        if (minValue != null)
            parsedMin = ParseAndValidateNumber(minValue, isDecimalAllowed, nameof(MinValue));

        if (maxValue != null)
            parsedMax = ParseAndValidateNumber(maxValue, isDecimalAllowed, nameof(MaxValue));

        if (parsedMin.HasValue && parsedMax.HasValue && parsedMin.Value >= parsedMax.Value)
            throw new ArgumentException("MinValue must be smaller than MaxValue.", nameof(MinValue));

        if (parsedInitial.HasValue)
        {
            if (parsedMin.HasValue && parsedInitial.Value < parsedMin.Value)
                throw new ArgumentException(
                    $"InitialValue ({initialValue}) cannot be smaller than MinValue ({minValue}).",
                    nameof(InitialValue));

            if (parsedMax.HasValue && parsedInitial.Value > parsedMax.Value)
                throw new ArgumentException(
                    $"InitialValue ({initialValue}) cannot be greater than MaxValue ({maxValue}).",
                    nameof(InitialValue));
        }
    }

    private static decimal ParseAndValidateNumber(string value, bool isDecimalAllowed, string propertyName)
    {
        if (!NumberRegex().IsMatch(value) || !decimal.TryParse(value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                out var number))
            throw new ArgumentException($"{propertyName} must be a valid number.", propertyName);

        if (!isDecimalAllowed && !IntegerRegex().IsMatch(value))
            throw new ArgumentException($"{propertyName} cannot contain decimals when IsDecimalAllowed is false.",
                propertyName);

        return number;
    }
}