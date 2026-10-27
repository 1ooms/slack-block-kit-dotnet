using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.MultiSelectMenu;

/// <summary>
/// Represents a multi-select menu element that allows selecting multiple workspace users.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/multi-select-menu-element#users_multi_select">Slack Multi-Select Menu Users Documentation</seealso>
public class MultiUsersSelect : MultiSelectMenuBase
{
    public MultiUsersSelect(
        string? actionId = null,
        List<string>? initialUsers = null,
        ConfirmationDialogObject? confirm = null,
        int? maxSelectedItems = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, maxSelectedItems)
    {
        InitialUsers = initialUsers;
        FocusOnLoad = focusOnLoad;
    }

    public override string Type => "multi_users_select";

    [JsonPropertyName("initial_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? InitialUsers { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}