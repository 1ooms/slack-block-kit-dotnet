namespace SlackBlockKit.Tests.Elements;

public class MemoryCitationDetailsTests
{
    [Fact]
    public void Constructor_SetsMemoryId()
    {
        var details = new MemoryCitationDetails("M1");

        Assert.Equal("M1", details.MemoryId);
        Assert.Equal("memory", details.CitationType);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MemoryCitationDetails("M1"));

        AssertProperties(json, "citation_type", "memory_id");
        Assert.Equal("memory", json.GetProperty("citation_type").GetString());
        Assert.Equal("M1", json.GetProperty("memory_id").GetString());
    }

    [Fact]
    public void MemoryId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MemoryCitationDetails(null!));
    }
}
