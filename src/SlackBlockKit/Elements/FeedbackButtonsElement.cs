using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents feedback buttons that allow users to indicate positive or negative feedback.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/feedback-buttons-element">Slack Feedback Buttons Element Documentation</seealso>
public class FeedbackButtonsElement : ActionElement, IContextActionsElement
{
    public FeedbackButtonsElement(
        FeedbackButtonObject positiveButton,
        FeedbackButtonObject negativeButton,
        string? actionId = null)
    {
        PositiveButton = positiveButton;
        NegativeButton = negativeButton;
        ActionId = actionId;
    }

    public override string Type => "feedback_buttons";

    [JsonPropertyName("positive_button")]
    public FeedbackButtonObject PositiveButton
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("negative_button")]
    public FeedbackButtonObject NegativeButton
    {
        get;
        set => field = ThrowIfNull(value);
    }
}

/// <summary>
/// Represents an individual button configuration within a feedback buttons element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/feedback-buttons-element">Slack Feedback Buttons Element Documentation</seealso>
public class FeedbackButtonObject
{
    private const int MaxTextLength = 75;
    private const int MaxValueLength = 2000;
    private const int MaxAccessibilityLabelLength = 75;

    public FeedbackButtonObject(PlainTextObject text, string value, string? accessibilityLabel = null)
    {
        Text = text;
        Value = value;
        AccessibilityLabel = accessibilityLabel;
    }

    public FeedbackButtonObject(string text, string value, string? accessibilityLabel = null)
        : this(new PlainTextObject(text), value, accessibilityLabel)
    {
    }

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            ThrowIfExceedsLength(value.Text, MaxTextLength);
            field = value;
        }
    }

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsLength(value, MaxValueLength);
    }

    [JsonPropertyName("accessibility_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessibilityLabel
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxAccessibilityLabelLength);
    }
}