using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Base class for all Slack Block Kit layout blocks.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks">Slack Blocks Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ActionsBlock), "actions")]
[JsonDerivedType(typeof(AlertBlock), "alert")]
[JsonDerivedType(typeof(CardBlock), "card")]
[JsonDerivedType(typeof(CarouselBlock), "carousel")]
[JsonDerivedType(typeof(ContainerBlock), "container")]
[JsonDerivedType(typeof(ContextActionsBlock), "context_actions")]
[JsonDerivedType(typeof(ContextBlock), "context")]
[JsonDerivedType(typeof(DataTableBlock), "data_table")]
[JsonDerivedType(typeof(DataVisualizationBlock), "data_visualization")]
[JsonDerivedType(typeof(DividerBlock), "divider")]
[JsonDerivedType(typeof(FileBlock), "file")]
[JsonDerivedType(typeof(HeaderBlock), "header")]
[JsonDerivedType(typeof(ImageBlock), "image")]
[JsonDerivedType(typeof(InputBlock), "input")]
[JsonDerivedType(typeof(MarkdownBlock), "markdown")]
[JsonDerivedType(typeof(PlanBlock), "plan")]
[JsonDerivedType(typeof(RichTextBlock), "rich_text")]
[JsonDerivedType(typeof(SectionBlock), "section")]
[JsonDerivedType(typeof(TableBlock), "table")]
[JsonDerivedType(typeof(TaskCardBlock), "task_card")]
[JsonDerivedType(typeof(VideoBlock), "video")]
public abstract class Block
{
    private const int MaxBlockIdLength = 255;

    [JsonPropertyName("block_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BlockId
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxBlockIdLength);
    }

    [JsonIgnore] public abstract string Type { get; }
}

/// <summary>
/// Marks a block as available in messages (<see cref="SlackBlockKit.Surfaces.MessageSurface"/>).
/// </summary>
/// <remarks>Taken from the <c>available_in_surfaces</c> list on each block's Slack documentation page.</remarks>
/// <seealso href="https://docs.slack.dev/messaging">Slack Messages Documentation</seealso>
[JsonConverter(typeof(InterfaceBlockConverter<IMessageBlock>))]
public interface IMessageBlock;

/// <summary>
/// Marks a block as available in modals (<see cref="SlackBlockKit.Surfaces.ModalSurface"/>).
/// </summary>
/// <remarks>Taken from the <c>available_in_surfaces</c> list on each block's Slack documentation page.</remarks>
/// <seealso href="https://docs.slack.dev/surfaces/modals">Slack Modals Documentation</seealso>
[JsonConverter(typeof(InterfaceBlockConverter<IModalBlock>))]
public interface IModalBlock;

/// <summary>
/// Marks a block as available in App Home tabs (<see cref="SlackBlockKit.Surfaces.HomeSurface"/>).
/// </summary>
/// <remarks>Taken from the <c>available_in_surfaces</c> list on each block's Slack documentation page.</remarks>
/// <seealso href="https://docs.slack.dev/surfaces/app-home">Slack App Home Documentation</seealso>
[JsonConverter(typeof(InterfaceBlockConverter<IHomeBlock>))]
public interface IHomeBlock;

/// <summary>
/// Converter for surface marker interfaces whose implementors all derive from <see cref="Block"/>.
/// </summary>
/// <remarks>
/// Delegates to the polymorphic <see cref="Block"/> hierarchy, so the <c>"type"</c> discriminator is written and read
/// without repeating every <see cref="JsonDerivedTypeAttribute"/> on each interface. Reading a block that isn't
/// available in the surface throws a <see cref="JsonException"/>.
/// </remarks>
public class InterfaceBlockConverter<T> : JsonConverter<T> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var block = JsonSerializer.Deserialize<Block>(ref reader, options);
        return block switch
        {
            null => null,
            T typed => typed,
            _ => throw new JsonException($"Block type '{block.Type}' is not supported by {typeof(T).Name}.")
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Block)(object)value, options);
    }
}