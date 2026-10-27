using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element that allows selecting multiple conversations (channels, DMs, MPIMs).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#conversation_multi_select">Slack Multi-Select Menu Conversations Documentation</seealso>
public class MultiConversationsSelect : MultiSelectMenuBase
{
    public MultiConversationsSelect(
        string? actionId = null,
        List<string>? initialConversations = null,
        bool? defaultToCurrentConversation = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        ConversationFilterObject? filter = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, maxSelectedItems)
    {
        InitialConversations = initialConversations;
        DefaultToCurrentConversation = defaultToCurrentConversation;
        Filter = filter;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "multi_conversations_select";

    [JsonPropertyName("initial_conversations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? InitialConversations { get; set; }

    [JsonPropertyName("default_to_current_conversation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DefaultToCurrentConversation { get; set; }

    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConversationFilterObject? Filter { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}