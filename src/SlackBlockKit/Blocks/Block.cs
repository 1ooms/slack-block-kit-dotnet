using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ActionsBlock), "actions")]
[JsonDerivedType(typeof(ContextActionsBlock), "context_actions")]
[JsonDerivedType(typeof(ContextBlock), "context")]
[JsonDerivedType(typeof(DividerBlock), "divider")]
[JsonDerivedType(typeof(FileBlock), "file")]
[JsonDerivedType(typeof(HeaderBlock), "header")]
[JsonDerivedType(typeof(ImageBlock), "image")]
[JsonDerivedType(typeof(InputBlock), "input")]
[JsonDerivedType(typeof(MarkdownBlock), "markdown")]
[JsonDerivedType(typeof(RichTextBlock), "rich_text")]
[JsonDerivedType(typeof(SectionBlock), "section")]
[JsonDerivedType(typeof(TableBlock), "table")]
[JsonDerivedType(typeof(VideoBlock), "video")]
public abstract class Block
{
    private const int MaxBlockIdLength = 255;

    [JsonPropertyName("block_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BlockId
    {
        get;
        set
        {
            if (value != null && value.Length > MaxBlockIdLength)
                throw new ArgumentException($"BlockId cannot be longer than {MaxBlockIdLength} characters.",
                    nameof(value));

            field = value;
        }
    }

    [JsonIgnore] public abstract string Type { get; }
}