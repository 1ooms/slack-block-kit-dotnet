namespace SlackBlockKit.Tests.Blocks;

public class RawNumberCellTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var cell = new RawNumberCell(1234.5, "1,234.50");

        Assert.Equal(1234.5, cell.Value);
        Assert.Equal("1,234.50", cell.Text);
        Assert.Equal("raw_number", cell.Type);
    }

    [Fact]
    public void Serialize_WritesTypeValueAndText()
    {
        var json = ToJson(new RawNumberCell(1234.5, "1,234.50"));

        AssertProperties(json, "type", "value", "text");
        Assert.Equal("raw_number", json.GetProperty("type").GetString());
        Assert.Equal(1234.5, json.GetProperty("value").GetDouble());
        Assert.Equal("1,234.50", json.GetProperty("text").GetString());
    }

    [Fact]
    public void Text_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new RawNumberCell(1, null!));
        Assert.Throws<ArgumentException>(() => new RawNumberCell(1, ""));
    }
}
