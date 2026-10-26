using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a list record element that renders as a link to a Slack list record.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/list-record-element">Slack List Record Element Documentation</seealso>
public class ListRecordElement : Element, IRichTextSubElement
{
    public ListRecordElement(
        string fileId,
        string? recordId = null,
        string? viewId = null,
        string? text = null,
        string? url = null,
        RichTextStyle? style = null)
    {
        FileId = fileId;
        RecordId = recordId;
        ViewId = viewId;
        Text = text;
        Url = url;
        Style = style;
    }

    [JsonPropertyName("file_id")]
    public string FileId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("record_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RecordId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("view_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ViewId
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

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "list_record";
}