using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element that allows selecting from a list of public/private channels, DMs, or MPIMs.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#conversations_select">Slack Conversations Select Menu Documentation</seealso>
public class ConversationsSelect : SelectMenuBase
{
    public ConversationsSelect(
        string? actionId = null,
        string? initialConversation = null,
        bool? defaultToCurrentConversation = null,
        ConfirmationDialogObject? confirm = null,
        bool? responseUrlEnabled = null,
        ConversationFilterObject? filter = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        InitialConversation = initialConversation;
        DefaultToCurrentConversation = defaultToCurrentConversation;
        Filter = filter;
        ResponseUrlEnabled = responseUrlEnabled;
    }

    public override string Type => "conversations_select";

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
}