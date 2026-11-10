using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;

namespace SlackBlockKit.Surfaces;

/// <summary>
/// Base class for view surfaces (modals and the App Home tab), identified by their <c>"type"</c>.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/views">Slack Views Reference</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(HomeSurface), "home")]
[JsonDerivedType(typeof(ModalSurface), "modal")]
public abstract class ViewSurface
{
    private const int MaxBlocks = 100;
    private const int MaxPrivateMetadataLength = 3000;
    private const int MaxCallbackIdLength = 255;

    [JsonIgnore] public abstract string Type { get; }

    [JsonPropertyName("blocks")]
    public required List<Block> Blocks
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