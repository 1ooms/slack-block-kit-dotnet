using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Abstract base class for all single-select menu elements in Slack Block Kit.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element">Slack Select Menu Documentation</seealso>
public abstract class SelectMenuBase : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;

    protected SelectMenuBase(
        string? actionId = null,
        PlainTextObject? placeholder = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null)
    {
        ActionId = actionId;
        Placeholder = placeholder;
        Confirm = confirm;
        FocusOnLoad = focusOnLoad;
    }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}