using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents the available icons for the icon button element. The `trash` icon is the only icon available at this time.
/// </summary>
public enum IconButtonIcon
{
    Trash
}

/// <summary>
/// Represents an icon button element used to perform contextual actions.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/icon-button-element">Slack Icon Button Element Documentation</seealso>
public class IconButtonElement : ActionElement, IContextActionsElement
{
    private const int MaxAccessibilityLabelLength = 75;

    public IconButtonElement(
        IconButtonIcon icon,
        PlainTextObject text,
        string? actionId = null,
        string? value = null,
        ConfirmationDialogObject? confirm = null,
        List<string>? visibleToUserIds = null,
        string? accessibilityLabel = null)
    {
        Icon = icon;
        Text = text;
        ActionId = actionId;
        Value = value;
        Confirm = confirm;
        VisibleToUserIds = visibleToUserIds;
        AccessibilityLabel = accessibilityLabel;
    }

    public override string Type => "icon_button";

    [JsonPropertyName("icon")] public IconButtonIcon Icon { get; set; }

    [JsonPropertyName("text")] public PlainTextObject Text { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Value
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("accessibility_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessibilityLabel
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxAccessibilityLabelLength);
    }

    [JsonPropertyName("visible_to_user_ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? VisibleToUserIds { get; set; }
}