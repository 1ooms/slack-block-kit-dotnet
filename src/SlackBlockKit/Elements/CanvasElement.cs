using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a canvas element that renders as a link to a Slack canvas.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/canvas-element">Slack Canvas Element Documentation</seealso>
public class CanvasElement : Element, IRichTextSubElement
{
    public CanvasElement(
        string fileId,
        string? label = null,
        bool? hideTitle = null,
        string? sectionId = null,
        string? text = null,
        string? url = null,
        bool? isSkillInvocation = null,
        RichTextStyle? style = null)
    {
        FileId = fileId;
        Label = label;
        HideTitle = hideTitle;
        SectionId = sectionId;
        Text = text;
        Url = url;
        IsSkillInvocation = isSkillInvocation;
        Style = style;
    }

    public override string Type => "canvas";

    [JsonPropertyName("file_id")]
    public string FileId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("hide_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HideTitle { get; set; }

    [JsonPropertyName("section_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SectionId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("is_skill_invocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSkillInvocation { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }
}
