namespace SlackBlockKit.Tests.Blocks;

public class DividerBlockTests
{
    [Fact]
    public void Constructor_SetsBlockId()
    {
        var block = new DividerBlock("divider_1");

        Assert.Equal("divider_1", block.BlockId);
        Assert.Equal("divider", block.Type);
    }

    [Fact]
    public void Serialize_WritesTypeAndBlockId()
    {
        var json = ToJson(new DividerBlock("divider_1"));

        AssertProperties(json, "type", "block_id");
        Assert.Equal("divider", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_WithoutBlockId_WritesOnlyType()
    {
        AssertProperties(ToJson(new DividerBlock()), "type");
    }

    [Fact]
    public void RoundTrip_PreservesBlockId()
    {
        Assert.Equal("divider_1", RoundTrip(new DividerBlock("divider_1")).BlockId);
    }
}
