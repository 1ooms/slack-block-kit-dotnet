namespace SlackBlockKit.Tests.Blocks;

public class FileBlockTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var block = new FileBlock("ext_1", "remote", "file_1");

        Assert.Equal("ext_1", block.ExternalId);
        Assert.Equal("remote", block.Source);
        Assert.Equal("file_1", block.BlockId);
        Assert.Equal("file", block.Type);
    }

    [Fact]
    public void Constructor_DefaultsSourceToRemote()
    {
        Assert.Equal("remote", new FileBlock("ext_1").Source);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new FileBlock("ext_1", blockId: "file_1"));

        AssertProperties(json, "type", "external_id", "source", "block_id");
        Assert.Equal("file", json.GetProperty("type").GetString());
        Assert.Equal("ext_1", json.GetProperty("external_id").GetString());
        Assert.Equal("remote", json.GetProperty("source").GetString());
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FileBlock(null!));
        Assert.Throws<ArgumentNullException>(() => new FileBlock("ext_1", null!));
    }
}
