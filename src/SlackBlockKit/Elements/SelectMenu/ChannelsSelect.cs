using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element that allows selecting from a list of public channels in the workspace.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#channels_select">Slack Channels Select Menu Documentation</seealso>
public class ChannelsSelect : SelectMenuBase
{
    public ChannelsSelect(
        string? actionId = null,
        string? initialChannel = null,
        ConfirmationDialogObject? confirm = null,
        bool? responseUrlEnabled = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        InitialChannel = initialChannel;
        ResponseUrlEnabled = responseUrlEnabled;
    }

    public override string Type => "channels_select";

    [JsonPropertyName("initial_channel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialChannel { get; set; }

    [JsonPropertyName("response_url_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ResponseUrlEnabled { get; set; }
}