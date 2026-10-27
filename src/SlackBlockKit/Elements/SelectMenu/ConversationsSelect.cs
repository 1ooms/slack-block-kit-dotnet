using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element that allows selecting from a list of public/private channels, DMs, or MPIMs.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#conversations_select">Slack Conversations Select Menu Documentation</seealso>
public class ConversationsSelect : ActionElement, IActionsElement, IInputElement, ISectionElement
{
    private const int MaxPlaceholderLength = 150;

    public ConversationsSelect(
        string? actionId = null,
        string? initialConversation = null,
        bool? defaultToCurrentConversation = null,
        ConfirmationDialogObject? confirm = null,
        bool? responseUrlEnabled = null,
        ConversationFilterObject? filter = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialConversation = initialConversation;
        DefaultToCurrentConversation = defaultToCurrentConversation;
        Confirm = confirm;
        ResponseUrlEnabled = responseUrlEnabled;
        Filter = filter;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "conversations_select";

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("initial_conversation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialConversation { get; set; }

    [JsonPropertyName("default_to_current_conversation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DefaultToCurrentConversation { get; set; }

    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConversationFilterObject? Filter { get; set; }

    [JsonPropertyName("response_url_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ResponseUrlEnabled { get; set; }

    [JsonPropertyName("confirm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogObject? Confirm { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}