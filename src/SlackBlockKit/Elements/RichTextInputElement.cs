using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a rich text input element that allows users to enter formatted text in a WYSIWYG composer.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/rich-text-input-element">Slack Rich Text Input Element Documentation</seealso>
public class RichTextInputElement : ActionElement, IActionsElement, IInputElement
{
    private const int MaxPlaceholderLength = 150;
    private const int MinMinAndMaxLines = 1;
    private const int MaxMinAndMaxLines = 100;

    public RichTextInputElement(
        string? actionId = null,
        RichTextBlock? initialValue = null,
        DispatchActionConfigurationObject? dispatchActionConfig = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null,
        int? minLines = null,
        int? maxLines = null)
    {
        ActionId = actionId;
        InitialValue = initialValue;
        DispatchActionConfig = dispatchActionConfig;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
        MinLines = minLines;
        MaxLines = maxLines;
    }

    public override string Type => "rich_text_input";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextBlock? InitialValue { get; set; }

    [JsonPropertyName("dispatch_action_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    [JsonPropertyName("min_lines")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinLines
    {
        get;
        set
        {
            if (value < MinMinAndMaxLines || value > MaxMinAndMaxLines)
                throw new ArgumentException(
                    $"MinLines must be between {MinMinAndMaxLines} and {MaxMinAndMaxLines} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("max_lines")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLines
    {
        get;
        set
        {
            if (value < MinMinAndMaxLines || value > MaxMinAndMaxLines)
                throw new ArgumentException(
                    $"MaxLines must be between {MinMinAndMaxLines} and {MaxMinAndMaxLines} characters.", nameof(value));
            field = value;
        }
    }
}