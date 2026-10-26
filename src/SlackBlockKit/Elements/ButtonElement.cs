using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an interactive button element that allows users a direct path to performing basic actions.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/button-element">Slack Button Element Documentation</seealso>
public class ButtonElement : ActionElement, IActionsElement, ISectionElement
{
    /// <summary>
    /// Specifies the visual style of a button element.
    /// </summary>
    public enum ButtonStyle
    {
        Primary,
        Danger
    }

    private const int MaxValueLength = 2000;
    private const int MaxUrlLength = 3000;
    private const int MaxAccessibilityLabelLength = 75;
    private const int MaxTextLength = 75;

    public ButtonElement(
        PlainTextObject text,
        string? actionId = null,
        string? value = null,
        string? url = null,
        ButtonStyle? style = null,
        ConfirmationDialogObject? confirm = null,
        string? accessibilityLabel = null)
    {
        Text = text;
        ActionId = actionId;
        Value = value;
        Url = url;
        Style = style;
        Confirm = confirm;
        AccessibilityLabel = accessibilityLabel;
    }

    public ButtonElement(string text, string? actionId = null, string? value = null, ButtonStyle? style = null)
        : this(new PlainTextObject(text), actionId, value, null, style)
    {
    }

    public override string Type => "button";

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set
        {
            ThrowIfExceedsLength(value.Text, MaxTextLength);
            field = value;
        }
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfEmpty(value);
                ThrowIfExceedsLength(value, MaxUrlLength);
            }

            field = value;
        }
    }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Value
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfEmpty(value);
                ThrowIfExceedsLength(value, MaxValueLength);
            }

            field = value;
        }
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ButtonStyle? Style { get; set; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("accessibility_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessibilityLabel
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfExceedsLength(value, MaxAccessibilityLabelLength);
            }

            field = value;
        }
    }
}