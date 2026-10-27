using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a work object mention element that renders as a Work Object reference.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/work-object-mention-element">Slack Work Object Mention Element Documentation</seealso>
public class WorkObjectMentionElement : Element, IRichTextSubElement
{
    public WorkObjectMentionElement(
        string entityId,
        string appId,
        string text,
        string url,
        string? iconUrl = null,
        bool? fullSizePreviewEnabled = null,
        string? productName = null,
        RichTextStyle? style = null)
    {
        EntityId = entityId;
        AppId = appId;
        Text = text;
        Url = url;
        IconUrl = iconUrl;
        FullSizePreviewEnabled = fullSizePreviewEnabled;
        ProductName = productName;
        Style = style;
    }

    [JsonPropertyName("entity_id")]
    public string EntityId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("app_id")]
    public string AppId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("url")]
    public string Url
    {
        get;
        set
        {
            ThrowIfNull(value);
            field = ThrowIfNotValidUrl(value)!;
        }
    }

    [JsonPropertyName("icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("full_size_preview_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FullSizePreviewEnabled { get; set; }

    [JsonPropertyName("product_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProductName
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "work_object_mention";
}