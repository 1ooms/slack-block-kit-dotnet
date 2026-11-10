namespace SlackBlockKit.Tests.Elements.RichText;

public class RichTextQuoteElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var elements = new List<IRichTextSubElement> { new TextElement("quote") };

        var element = new RichTextQuoteElement(elements, 1);

        Assert.Same(elements, element.Elements);
        Assert.Equal(1, element.Border);
        Assert.Equal("rich_text_quote", element.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var element = new RichTextQuoteElement(new TextElement("a"), new UserElement("U1"));

        Assert.Equal(2, element.Elements.Count);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new RichTextQuoteElement([new TextElement("quote"), new UserElement("U1")], 1));

        AssertProperties(json, "type", "elements", "border");
        Assert.Equal("rich_text_quote", json.GetProperty("type").GetString());
        Assert.Equal(["text", "user"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new RichTextQuoteElement(new TextElement("q"))), "type", "elements");
    }

    [Fact]
    public void Elements_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RichTextQuoteElement((List<IRichTextSubElement>)null!));
    }

    [Fact]
    public void Serialize_AsRichTextElement_WritesDiscriminatorOnce()
    {
        var json = ToJson(new List<RichTextElement> { new RichTextQuoteElement(new TextElement("q")) })[0];

        AssertProperties(json, "type", "elements");
    }
}
