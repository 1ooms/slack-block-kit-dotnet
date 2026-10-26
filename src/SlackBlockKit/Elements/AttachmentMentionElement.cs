using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an attachment mention element that renders as a rich app attachment or entity reference.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/attachment-mention-element">Slack Attachment Mention Element Documentation</seealso>
public class AttachmentMentionElement : Element, IRichTextSubElement
{
    public AttachmentMentionElement(
        string url,
        string? text = null,
        string? appId = null,
        string? entityId = null,
        string? iconUrl = null,
        string? channelId = null,
        string? ts = null,
        bool? fullSizePreviewEnabled = null,
        string? iconName = null,
        string? referenceObjectType = null,
        string? productName = null,
        RichTextStyle? style = null)
    {
        Url = url;
        Text = text;
        AppId = appId;
        EntityId = entityId;
        IconUrl = iconUrl;
        ChannelId = channelId;
        Ts = ts;
        FullSizePreviewEnabled = fullSizePreviewEnabled;
        IconName = iconName;
        ReferenceObjectType = referenceObjectType;
        ProductName = productName;
        Style = style;
    }

    public override string Type => "attachment_mention";

    [JsonPropertyName("url")]
    public string? Url
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

    [JsonPropertyName("app_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("entity_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntityId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("channel_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChannelId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("ts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Ts
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("full_size_preview_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FullSizePreviewEnabled { get; set; }

    [JsonPropertyName("icon_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconName
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("reference_object_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReferenceObjectType
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

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
}
