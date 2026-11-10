namespace SlackBlockKit.Tests.Elements.RichText;

public class RichTextListElementTests
{
    private static RichTextSectionElement Item(string text) => new(new TextElement(text));

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var items = new List<RichTextSectionElement> { Item("a") };

        var list = new RichTextListElement(items, RichTextListStyle.Ordered, 1, 2, 3);

        Assert.Same(items, list.Elements);
        Assert.Equal(RichTextListStyle.Ordered, list.Style);
        Assert.Equal(1, list.Indent);
        Assert.Equal(2, list.Offset);
        Assert.Equal(3, list.Border);
        Assert.Equal("rich_text_list", list.Type);
    }

    [Fact]
    public void ParamsConstructor_DefaultsToBulletStyle()
    {
        var list = new RichTextListElement(Item("a"), Item("b"));

        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Equal(2, list.Elements.Count);
    }

    [Fact]
    public void StyleParamsConstructor_SetsStyle()
    {
        var list = new RichTextListElement(RichTextListStyle.Ordered, Item("a"));

        Assert.Equal(RichTextListStyle.Ordered, list.Style);
        Assert.Single(list.Elements);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new RichTextListElement([Item("a")], RichTextListStyle.Ordered, 1, 2, 3));

        AssertProperties(json, "type", "elements", "style", "indent", "offset", "border");
        Assert.Equal("rich_text_list", json.GetProperty("type").GetString());
        Assert.Equal("ordered", json.GetProperty("style").GetString());
        Assert.Equal("rich_text_section", json.GetProperty("elements")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        var json = ToJson(new RichTextListElement(Item("a")));

        AssertProperties(json, "type", "elements", "style");
        Assert.Equal("bullet", json.GetProperty("style").GetString());
    }

    [Fact]
    public void Elements_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RichTextListElement((List<RichTextSectionElement>)null!));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var list = RoundTrip(new RichTextListElement([Item("a"), Item("b")], RichTextListStyle.Ordered, indent: 1));

        Assert.Equal(RichTextListStyle.Ordered, list.Style);
        Assert.Equal(1, list.Indent);
        Assert.Equal("b", Assert.IsType<TextElement>(list.Elements[1].Elements[0]).Text);
    }
}
