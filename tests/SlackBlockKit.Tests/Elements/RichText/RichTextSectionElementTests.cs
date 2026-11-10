namespace SlackBlockKit.Tests.Elements.RichText;

public class RichTextSectionElementTests
{
    [Fact]
    public void ListConstructor_SetsElements()
    {
        var elements = new List<IRichTextSubElement> { new TextElement("a") };

        var section = new RichTextSectionElement(elements);

        Assert.Same(elements, section.Elements);
        Assert.Equal("rich_text_section", section.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var section = new RichTextSectionElement(new TextElement("a"), new UserElement("U1"));

        Assert.Equal(2, section.Elements.Count);
    }

    [Fact]
    public void ParamsConstructor_AllowsNoElements()
    {
        Assert.Empty(new RichTextSectionElement().Elements);
    }

    [Fact]
    public void Serialize_WritesSubElementsWithDiscriminators()
    {
        var json = ToJson(new RichTextSectionElement(
            new TextElement("Hello ", new RichTextStyle(bold: true)),
            new UserElement("U1"),
            new EmojiElement("wave"),
            new LinkElement("https://example.com", "link"),
            new BroadcastElement(BroadcastRange.Here)));

        AssertProperties(json, "type", "elements");
        Assert.Equal("rich_text_section", json.GetProperty("type").GetString());
        Assert.Equal(["text", "user", "emoji", "link", "broadcast"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
        Assert.Equal("Hello ", json.GetProperty("elements")[0].GetProperty("text").GetString());
        Assert.True(json.GetProperty("elements")[0].GetProperty("style").GetProperty("bold").GetBoolean());
    }

    [Fact]
    public void Elements_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RichTextSectionElement((List<IRichTextSubElement>)null!));
    }

    [Fact]
    public void RoundTrip_PreservesSubElementTypes()
    {
        var section = RoundTrip(new RichTextSectionElement(new TextElement("a"), new ChannelElement("C1"),
            new UsergroupElement("S1")));

        Assert.IsType<TextElement>(section.Elements[0]);
        Assert.IsType<ChannelElement>(section.Elements[1]);
        Assert.IsType<UsergroupElement>(section.Elements[2]);
    }
}
