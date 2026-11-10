using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a plain-text input element that allows users to enter freeform text data into a single-line or multi-line field.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/plain-text-input-element">Slack Plain-Text Input Element Documentation</seealso>
public class PlainTextInputElement : ActionElement, IInputElement
{
    private const int MaxPlaceholderLength = 150;
    private const int MinMinLength = 0;
    private const int MaxMinLength = 3000;
    private const int MinMaxLength = 1;
    private const int MaxMaxLength = 3000;

    [JsonConstructor]
    public PlainTextInputElement(
        string? actionId = null,
        string? initialValue = null,
        bool? multiline = null,
        int? minLength = null,
        int? maxLength = null,
        DispatchActionConfigurationObject? dispatchActionConfig = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialValue = initialValue;
        Multiline = multiline;
        MinLength = minLength;
        MaxLength = maxLength;
        DispatchActionConfig = dispatchActionConfig;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public PlainTextInputElement(string actionId, string? placeholder)
        : this(actionId, placeholder: placeholder != null ? new PlainTextObject(placeholder) : null)
    {
    }

    public override string Type => "plain_text_input";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialValue { get; set; }

    [JsonPropertyName("multiline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Multiline { get; set; }

    /// <remarks>
    /// Deliberately not validated against <see cref="MaxLength"/>:
    /// the Slack API accepts a minimum length greater than the maximum length.
    /// </remarks>
    [JsonPropertyName("min_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLength
    {
        get;
        set
        {
            if (value < MinMinLength || value > MaxMinLength)
                throw new ArgumentException($"MinLength must be between {MinMinLength} and {MaxMinLength}.",
                    nameof(value));
            field = value;
        }
    }

    /// <remarks>
    /// <para>
    /// Deliberately not validated against <see cref="MinLength"/>:
    /// the Slack API accepts a maximum length smaller than the minimum length.
    /// </para>
    /// <para>
    /// Deliberately not validated against <see cref="InitialValue"/>:
    /// the Slack API accepts an initial value longer than the maximum length.
    /// </para>
    /// </remarks>
    [JsonPropertyName("max_length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLength
    {
        get;
        set
        {
            if (value < MinMaxLength || value > MaxMaxLength)
                throw new ArgumentException($"MaxLength must be between {MinMaxLength} and {MaxMaxLength}.",
                    nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("dispatch_action_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}