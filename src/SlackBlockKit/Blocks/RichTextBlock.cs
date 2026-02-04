using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

public class RichTextBlock(List<IRichTextElement> elements) : Block
{
    public override string Type => "rich_text";

    [JsonPropertyName("elements")] public List<IRichTextElement> Elements { get; set; } = elements;
}

public interface IRichTextElement;