namespace SlackBlockKit.Tests.Elements.RichText;

public class RichTextPreformattedElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var elements = new List<IRichTextPreformattedSubElement> { new TextElement("code") };

        var element = new RichTextPreformattedElement(elements, 1, "csharp");

        Assert.Same(elements, element.Elements);
        Assert.Equal(1, element.Border);
        Assert.Equal("csharp", element.Language);
        Assert.Equal("rich_text_preformatted", element.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var element = new RichTextPreformattedElement(new TextElement("a"), new LinkElement("https://example.com"));

        Assert.Equal(2, element.Elements.Count);
        Assert.Null(element.Border);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new RichTextPreformattedElement(
            [new TextElement("code"), new LinkElement("https://example.com")], 0, "python"));

        AssertProperties(json, "type", "elements", "border", "language");
        Assert.Equal("rich_text_preformatted", json.GetProperty("type").GetString());
        Assert.Equal(["text", "link"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
        Assert.Equal("python", json.GetProperty("language").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new RichTextPreformattedElement(new TextElement("code"))), "type", "elements");
    }

    [Fact]
    public void Elements_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new RichTextPreformattedElement((List<IRichTextPreformattedSubElement>)null!));
    }

    [Fact]
    public void RoundTrip_PreservesSubElementTypes()
    {
        var element = RoundTrip(new RichTextPreformattedElement(new TextElement("a"),
            new LinkElement("https://example.com")));

        Assert.IsType<TextElement>(element.Elements[0]);
        Assert.IsType<LinkElement>(element.Elements[1]);
    }
}
