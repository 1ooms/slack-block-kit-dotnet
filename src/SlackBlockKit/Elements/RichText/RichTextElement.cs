using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements.RichText;

/// <summary>
/// Marks an element as a top-level rich text block element (section, list, preformatted, or quote).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/rich_text">Slack Rich Text Block Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RichTextSectionElement), "rich_text_section")]
[JsonDerivedType(typeof(RichTextListElement), "rich_text_list")]
[JsonDerivedType(typeof(RichTextPreformattedElement), "rich_text_preformatted")]
[JsonDerivedType(typeof(RichTextQuoteElement), "rich_text_quote")]
public interface IRichTextElement
{
    [JsonIgnore] string Type { get; }
}

/// <summary>
/// Marks an element as valid for inclusion within a rich text section, list, quote, or preformatted block element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/rich_text">Slack Rich Text Block Documentation</seealso>
public interface IRichTextSubElement
{
    [JsonIgnore] string Type { get; }
}
