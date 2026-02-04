using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Elements;

public class FeedbackButtonsElement : Element, IContextActionsElement
{
    [JsonPropertyName("negative_button")] public ButtonObject NegativeButton;

    [JsonPropertyName("positive_button")] public ButtonObject PositiveButton;

    public FeedbackButtonsElement(ButtonObject negativeButton, ButtonObject positiveButton)
    {
        NegativeButton = negativeButton;
        PositiveButton = positiveButton;
    }

    public override string Type => "feedback_buttons";
}

public class ButtonObject
{
    public ButtonObject(PlainTextObject text, string value, string? accessibilityLabel = null)
    {
        Text = text;
        Value = value;
        AccessibilityLabel = accessibilityLabel;
    }

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set
        {
            value.MaxLength = 75;
            field = value;
        }
    }

    [JsonIgnore] private int MaxValueLength { get; } = 2000;

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set
        {
            if (value.Length > MaxValueLength)
                throw new ArgumentException($"Value cannot be longer than {MaxValueLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonIgnore] private int MaxAccessibilityLabelLength { get; } = 75;

    [JsonPropertyName("accessibility_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessibilityLabel
    {
        get;
        set
        {
            if (value != null && value.Length > MaxAccessibilityLabelLength)
                throw new ArgumentException($"Value cannot be longer than {MaxAccessibilityLabelLength} characters.",
                    nameof(value));
            field = value;
        }
    }
}