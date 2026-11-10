namespace SlackBlockKit.Tests.Blocks;

public class BlockTests
{
    private class TestBlock : Block
    {
        public override string Type => "test";
    }

    [Fact]
    public void BlockId_DefaultsToNull()
    {
        Assert.Null(new TestBlock().BlockId);
    }

    [Fact]
    public void BlockId_AllowsUpTo255Characters()
    {
        Assert.Equal(255, new TestBlock { BlockId = Chars(255) }.BlockId!.Length);
    }

    [Fact]
    public void BlockId_ThrowsWhenLongerThan255Characters()
    {
        Assert.Throws<ArgumentException>(() => new TestBlock { BlockId = Chars(256) });
    }

    [Fact]
    public void Serialize_BlockList_WritesDiscriminatorPerBlock()
    {
        var blocks = new List<Block>
        {
            new HeaderBlock("Header"), new DividerBlock(), new SectionBlock("Text"), new MarkdownBlock("**md**")
        };

        var json = ToJson(blocks);

        Assert.Equal(["header", "divider", "section", "markdown"],
            json.EnumerateArray().Select(b => b.GetProperty("type").GetString()));
    }

    [Fact]
    public void Serialize_BlockList_WritesTypeOnlyOnce()
    {
        var json = ToJson(new List<Block> { new DividerBlock("d1") })[0];

        AssertProperties(json, "type", "block_id");
    }

    [Fact]
    public void Serialize_BlockList_WithDataTable_DoesNotThrow()
    {
        var blocks = new List<Block>
        {
            new DataTableBlock("Caption", [[new RawTextCell("Header")], [new ActionCell(new ButtonElement("Go"))]])
        };

        var json = ToJson(blocks)[0];

        Assert.Equal("data_table", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Deserialize_ReturnsConcreteBlockType()
    {
        var block = SlackSerializer.Deserialize<Block>("""{"type":"divider","block_id":"d1"}""");

        Assert.Equal("d1", Assert.IsType<DividerBlock>(block).BlockId);
    }
}
