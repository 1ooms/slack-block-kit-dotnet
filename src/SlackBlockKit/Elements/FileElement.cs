using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a file element that renders as a link to a Slack file.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/file-element">Slack File Element Documentation</seealso>
public class FileElement : Element, IRichTextSubElement
{
    public FileElement(
        string fileId,
        string? text = null,
        string? url = null,
        bool? isSkillInvocation = null,
        RichTextStyle? style = null)
    {
        FileId = fileId;
        Text = text;
        Url = url;
        IsSkillInvocation = isSkillInvocation;
        Style = style;
    }

    [JsonPropertyName("file_id")]
    public string FileId
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfNullOrEmpty(value);
            }

            field = value;
        }
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfNullOrEmpty(value);
            }

            field = value;
        }
    }

    [JsonPropertyName("is_skill_invocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSkillInvocation { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "file";
}