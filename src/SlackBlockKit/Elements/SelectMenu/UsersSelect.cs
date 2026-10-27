using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements.SelectMenu;

/// <summary>
/// Represents a select menu element that allows selecting from a list of workspace users.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/select-menu-element#users_select">Slack Users Select Menu Documentation</seealso>
public class UsersSelect : SelectMenuBase
{
    public UsersSelect(
        string? actionId = null,
        string? initialUser = null,
        ConfirmationDialogObject? confirm = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
        : base(actionId, placeholder, confirm, focusOnLoad)
    {
        InitialUser = initialUser;
    }

    public override string Type => "users_select";

    [JsonPropertyName("initial_user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialUser { get; set; }
}