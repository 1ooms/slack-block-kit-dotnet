namespace SlackBlockKit.Tests.Blocks;

public class MarkdownBlockTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var block = new MarkdownBlock("**Bold**", "md_1");

        Assert.Equal("**Bold**", block.Text);
        Assert.Equal("md_1", block.BlockId);
        Assert.Equal("markdown", block.Type);
    }

    [Fact]
    public void Serialize_WritesTextAsPlainString()
    {
        var json = ToJson(new MarkdownBlock("**Bold**", "md_1"));

        AssertProperties(json, "type", "text", "block_id");
        Assert.Equal("markdown", json.GetProperty("type").GetString());
        Assert.Equal(JsonValueKind.String, json.GetProperty("text").ValueKind);
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MarkdownBlock(null!));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new MarkdownBlock("# Title", "md_1"));

        Assert.Equal("# Title", block.Text);
        Assert.Equal("md_1", block.BlockId);
    }
}
