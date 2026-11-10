using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;

namespace SlackBlockKit.Surfaces;

/// <summary>
/// Represents an App Home tab view (<c>type: "home"</c>), published with the <c>views.publish</c> Web API method.
/// </summary>
/// <seealso href="https://docs.slack.dev/surfaces/app-home">Slack App Home Documentation</seealso>
/// <seealso href="https://docs.slack.dev/reference/views/home-tab-views">Slack Home Tab View Reference</seealso>
public class HomeSurface
{
    private const int MaxBlocks = 100;
    private const int MaxPrivateMetadataLength = 3000;
    private const int MaxCallbackIdLength = 255;

    [JsonConstructor]
    public HomeSurface(
        List<IHomeBlock> blocks,
        string? privateMetadata = null,
        string? callbackId = null,
        string? externalId = null)
    {
        Blocks = blocks;
        PrivateMetadata = privateMetadata;
        CallbackId = callbackId;
        ExternalId = externalId;
    }

    public HomeSurface(params IHomeBlock[] blocks)
        : this(blocks.ToList())
    {
    }

    /// <remarks>
    /// A plain read-only property rather than a polymorphic discriminator, so it's always written and is ignored
    /// when reading, wherever it appears in the JSON Slack sends.
    /// </remarks>
    [JsonPropertyName("type")]
    public string Type => "home";

    [JsonPropertyName("blocks")]
    public List<IHomeBlock> Blocks
    {
        get;
        set => field = ThrowIfNullOrExceedsCount(value, MaxBlocks);
    }

    [JsonPropertyName("private_metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PrivateMetadata
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPrivateMetadataLength);
    }

    [JsonPropertyName("callback_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CallbackId
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxCallbackIdLength);
    }

    [JsonPropertyName("external_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalId { get; set; }
}