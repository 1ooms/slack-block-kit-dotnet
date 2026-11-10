using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element that allows selecting multiple public channels in the workspace.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#channel_multi_select">Slack Multi-Select Menu Channels Documentation</seealso>
public class MultiChannelsSelect : MultiSelectMenuBase
{
    public MultiChannelsSelect(
        string? actionId = null,
        List<string>? initialChannels = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, maxSelectedItems)
    {
        InitialChannels = initialChannels;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "multi_channels_select";

    [JsonPropertyName("initial_channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? InitialChannels
    {
        get;
        set
        {
            ValidateInitialSelectionCount(value?.Count, MaxSelectedItems);
            field = value;
        }
    }

    protected override int? InitialSelectionCount => InitialChannels?.Count;

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}