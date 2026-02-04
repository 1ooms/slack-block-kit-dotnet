using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class ButtonElement : Element, IActionsElement, ISectionElement
{
    public enum ButtonStyle
    {
        Primary,
        Danger
    }

    private const int MaxValueLength = 2000;
    private const int MaxUrlLength = 3000;

    private const int MaxAccessibilityLabelLength = 75;

    public ButtonElement(PlainTextObject text, string? actionId = null, string? value = null, string? url = null,
        ButtonStyle? style = null, ConfirmationDialogObject? confirm = null, string? accessibilityLabel = null)
    {
        Text = text;
        ActionId = actionId;
        Value = value;
        Url = url;
        Style = style;
        Confirm = confirm;
        AccessibilityLabel = accessibilityLabel;
    }

    public override string Type => "button";

    [JsonPropertyName("text")] public PlainTextObject Text { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set
        {
            if (value != null && value.Length > MaxUrlLength)
                throw new ArgumentException($"Url cannot be longer than {MaxUrlLength} characters.", nameof(value));
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
            if (value != null && value.Length > MaxValueLength)
                throw new ArgumentException($"Value cannot be longer than {MaxValueLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StyleString => Style?.ToString().ToLowerInvariant();

    [JsonIgnore] public ButtonStyle? Style { get; set; }

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
            if (value != null && value.Length > MaxAccessibilityLabelLength)
                throw new ArgumentException(
                    $"AccessibilityLabel cannot be longer than {MaxAccessibilityLabelLength} characters.",
                    nameof(value));

            field = value;
        }
    }
}