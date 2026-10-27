using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a usergroup element that renders as a mention of a Slack user group.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/usergroup-element">Slack Usergroup Element Documentation</seealso>
public class UsergroupElement : Element, IRichTextSubElement
{
    public UsergroupElement(
        string usergroupId,
        RichTextStyle? style = null)
    {
        UsergroupId = usergroupId;
        Style = style;
    }

    [JsonPropertyName("usergroup_id")]
    public string UsergroupId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "usergroup";
}