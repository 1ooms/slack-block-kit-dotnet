namespace SlackBlockKit.Tests.Blocks;

public class RichTextBlockTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var elements = new List<RichTextElement> { new RichTextSectionElement(new TextElement("a")) };

        var block = new RichTextBlock(elements, "rich_1");

        Assert.Same(elements, block.Elements);
        Assert.Equal("rich_1", block.BlockId);
        Assert.Equal("rich_text", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllRichTextElementTypes()
    {
        var json = ToJson(new RichTextBlock([
            new RichTextSectionElement(new TextElement("Hello "), new UserElement("U1")),
            new RichTextListElement(new RichTextSectionElement(new TextElement("item"))),
            new RichTextPreformattedElement(new TextElement("code")),
            new RichTextQuoteElement(new TextElement("quote"))
        ], "rich_1"));

        AssertProperties(json, "type", "elements", "block_id");
        Assert.Equal("rich_text", json.GetProperty("type").GetString());
        Assert.Equal(["rich_text_section", "rich_text_list", "rich_text_preformatted", "rich_text_quote"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));

        var section = json.GetProperty("elements")[0].GetProperty("elements");
        Assert.Equal("text", section[0].GetProperty("type").GetString());
        Assert.Equal("Hello ", section[0].GetProperty("text").GetString());
        Assert.Equal("user", section[1].GetProperty("type").GetString());
        Assert.Equal("U1", section[1].GetProperty("user_id").GetString());
    }

    [Fact]
    public void Serialize_AsTableCell_WritesTypeOnce()
    {
        var json = ToJson(new List<ITableCell> { RichTextBlockOf() })[0];

        AssertProperties(json, "type", "elements");
        Assert.Equal("rich_text", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Elements_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RichTextBlock(null!));
    }

    [Fact]
    public void Elements_AllowsEmptyList()
    {
        Assert.Empty(new RichTextBlock([]).Elements);
    }

    [Fact]
    public void RoundTrip_PreservesStructure()
    {
        var block = RoundTrip(new RichTextBlock([
            new RichTextSectionElement(new TextElement("a", new RichTextStyle(bold: true)), new EmojiElement("x"))
        ], "rich_1"));

        Assert.Equal("rich_1", block.BlockId);
        var section = Assert.IsType<RichTextSectionElement>(Assert.Single(block.Elements));
        Assert.True(Assert.IsType<TextElement>(section.Elements[0]).Style!.Bold);
        Assert.IsType<EmojiElement>(section.Elements[1]);
    }
}
