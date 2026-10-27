using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a team element that renders as a mention of a workspace or team.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/team-element">Slack Team Element Documentation</seealso>
public class TeamElement : Element, IRichTextSubElement
{
    public TeamElement(
        string teamId,
        RichTextStyle? style = null)
    {
        TeamId = teamId;
        Style = style;
    }

    [JsonPropertyName("team_id")]
    public string TeamId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "team";
}