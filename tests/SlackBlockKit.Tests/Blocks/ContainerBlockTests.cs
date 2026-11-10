namespace SlackBlockKit.Tests.Blocks;

public class ContainerBlockTests
{
    private static List<Block> Children() => [new SectionBlock("Child")];

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var children = Children();
        var icon = Image();

        var block = new ContainerBlock(children, new PlainTextObject("Title"), null, new MarkdownTextObject("Sub"),
            "container_1", ContainerWidth.Wide, icon, true, false, true);

        Assert.Same(children, block.ChildBlocks);
        Assert.Equal("Title", block.Title!.Text);
        Assert.Null(block.RichTextTitle);
        Assert.Equal("Sub", block.Subtitle!.Text);
        Assert.Equal("container_1", block.BlockId);
        Assert.Equal(ContainerWidth.Wide, block.Width);
        Assert.Same(icon, block.Icon);
        Assert.True(block.IsCollapsible);
        Assert.False(block.DefaultCollapsed);
        Assert.True(block.HasHeaderDivider);
        Assert.Equal("container", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ContainerBlock([new SectionBlock("Child"), new DividerBlock()],
            new PlainTextObject("Title"), RichTextBlockOf("Rich"), new PlainTextObject("Sub"), "container_1",
            ContainerWidth.Full, Image(), true, true, false));

        AssertProperties(json, "type", "child_blocks", "title", "rich_text_title", "subtitle", "block_id", "width",
            "icon", "is_collapsible", "default_collapsed", "has_header_divider");
        Assert.Equal("container", json.GetProperty("type").GetString());
        Assert.Equal("full", json.GetProperty("width").GetString());
        Assert.Equal("rich_text", json.GetProperty("rich_text_title").GetProperty("type").GetString());
        Assert.Equal("image", json.GetProperty("icon").GetProperty("type").GetString());
        Assert.Equal(["section", "divider"],
            json.GetProperty("child_blocks").EnumerateArray().Select(b => b.GetProperty("type").GetString()));
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ContainerBlock(Children(), new PlainTextObject("Title"))), "type",
            "child_blocks", "title");
    }

    [Theory]
    [InlineData(ContainerWidth.Narrow, "narrow")]
    [InlineData(ContainerWidth.Standard, "standard")]
    [InlineData(ContainerWidth.Wide, "wide")]
    [InlineData(ContainerWidth.Full, "full")]
    public void Width_SerializesAsSnakeCase(ContainerWidth width, string expected)
    {
        var json = ToJson(new ContainerBlock(Children(), new PlainTextObject("T"), width: width));

        Assert.Equal(expected, json.GetProperty("width").GetString());
    }

    [Fact]
    public void Constructor_ThrowsWithoutAnyTitle()
    {
        Assert.Throws<ArgumentException>(() => new ContainerBlock(Children()));
    }

    [Fact]
    public void Constructor_AcceptsRichTextTitleAlone()
    {
        Assert.NotNull(new ContainerBlock(Children(), richTextTitle: RichTextBlockOf()));
    }

    [Fact]
    public void Constructor_AcceptsBothTitles()
    {
        var block = new ContainerBlock(Children(), new PlainTextObject("T"), RichTextBlockOf());

        Assert.NotNull(block.Title);
        Assert.NotNull(block.RichTextTitle);
    }

    [Fact]
    public void Initializer_ThrowsWhenNoTitleRemains()
    {
        Assert.Throws<ArgumentException>(() => new ContainerBlock(Children(), new PlainTextObject("T")) { Title = null });
        Assert.Throws<ArgumentException>(() =>
            new ContainerBlock(Children(), richTextTitle: RichTextBlockOf()) { RichTextTitle = null });
    }

    [Fact]
    public void Setters_AllowClearingOneTitleWhenTheOtherRemains()
    {
        var block = new ContainerBlock(Children(), new PlainTextObject("T"));

        Assert.Throws<ArgumentException>(() => block.Title = null);

        block.RichTextTitle = RichTextBlockOf();
        block.Title = null;

        Assert.Null(block.Title);
        Assert.NotNull(block.RichTextTitle);
        Assert.Throws<ArgumentException>(() => block.RichTextTitle = null);
    }

    [Fact]
    public void Deserialize_RoundTripsEitherTitle()
    {
        var withTitle = SlackSerializer.Serialize(new ContainerBlock(Children(), new PlainTextObject("T")));
        var withRichTitle = SlackSerializer.Serialize(new ContainerBlock(Children(), richTextTitle: RichTextBlockOf()));

        Assert.Equal("T", SlackSerializer.Deserialize<ContainerBlock>(withTitle)!.Title!.Text);
        Assert.NotNull(SlackSerializer.Deserialize<ContainerBlock>(withRichTitle)!.RichTextTitle);
    }

    [Fact]
    public void ChildBlocks_ThrowsWhenNullOrEmpty()
    {
        var title = new PlainTextObject("T");

        Assert.Throws<ArgumentNullException>(() => new ContainerBlock(null!, title));
        Assert.Throws<ArgumentException>(() => new ContainerBlock([], title));
    }

    [Fact]
    public void ChildBlocks_AllowsUpTo10()
    {
        var children = Enumerable.Range(0, 10).Select(_ => (Block)new DividerBlock()).ToList();

        Assert.Equal(10, new ContainerBlock(children, new PlainTextObject("T")).ChildBlocks.Count);

        children.Add(new DividerBlock());
        Assert.Throws<ArgumentException>(() => new ContainerBlock(children, new PlainTextObject("T")));
    }

    [Fact]
    public void TitleAndSubtitle_AllowUpTo150Characters()
    {
        Assert.NotNull(new ContainerBlock(Children(), new PlainTextObject(Chars(150)),
            subtitle: new PlainTextObject(Chars(150))));
        Assert.Throws<ArgumentException>(() => new ContainerBlock(Children(), new PlainTextObject(Chars(151))));
        Assert.Throws<ArgumentException>(() =>
            new ContainerBlock(Children(), new PlainTextObject("T"), subtitle: new PlainTextObject(Chars(151))));
    }
}
