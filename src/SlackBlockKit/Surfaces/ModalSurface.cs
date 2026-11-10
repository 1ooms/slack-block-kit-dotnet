using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Surfaces;

/// <summary>
/// Represents a modal view (<c>type: "modal"</c>), opened with the <c>views.open</c>, <c>views.update</c>
/// and <c>views.push</c> Web API methods.
/// </summary>
/// <seealso href="https://docs.slack.dev/surfaces/modals">Slack Modals Documentation</seealso>
/// <seealso href="https://docs.slack.dev/reference/views/modal-views">Slack Modal View Reference</seealso>
public class ModalSurface : ViewSurface
{
    private const int MaxTitleLength = 24;
    private const int MaxButtonTextLength = 24;

    [JsonConstructor]
    public ModalSurface(
        PlainTextObject title,
        List<Block> blocks,
        PlainTextObject? submit = null,
        PlainTextObject? close = null,
        string? callbackId = null,
        string? privateMetadata = null,
        bool? clearOnClose = null,
        bool? notifyOnClose = null,
        string? externalId = null,
        bool? submitDisabled = null)
    {
        Title = title;
        Blocks = blocks;
        Submit = submit;
        Close = close;
        CallbackId = callbackId;
        PrivateMetadata = privateMetadata;
        ClearOnClose = clearOnClose;
        NotifyOnClose = notifyOnClose;
        ExternalId = externalId;
        SubmitDisabled = submitDisabled;
    }

    public ModalSurface(
        string title,
        List<Block> blocks,
        string? submit = null,
        string? close = null,
        string? callbackId = null,
        string? privateMetadata = null,
        bool? clearOnClose = null,
        bool? notifyOnClose = null,
        string? externalId = null,
        bool? submitDisabled = null)
        : this(
            new PlainTextObject(title),
            blocks,
            submit != null ? new PlainTextObject(submit) : null,
            close != null ? new PlainTextObject(close) : null,
            callbackId,
            privateMetadata,
            clearOnClose,
            notifyOnClose,
            externalId,
            submitDisabled)
    {
    }

    public override string Type => "modal";

    [JsonPropertyName("title")]
    public PlainTextObject Title
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTitleLength);
    }

    [JsonPropertyName("submit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Submit
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxButtonTextLength);
    }

    [JsonPropertyName("close")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Close
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxButtonTextLength);
    }

    [JsonPropertyName("clear_on_close")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ClearOnClose { get; set; }

    [JsonPropertyName("notify_on_close")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? NotifyOnClose { get; set; }

    [JsonPropertyName("submit_disabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SubmitDisabled { get; set; }
}