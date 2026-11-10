namespace SlackBlockKit.Tests.Blocks;

public class RawTextCellTests
{
    [Fact]
    public void Constructor_SetsText()
    {
        var cell = new RawTextCell("Hello");

        Assert.Equal("Hello", cell.Text);
        Assert.Equal("raw_text", cell.Type);
    }

    [Fact]
    public void ImplicitConversion_FromString()
    {
        RawTextCell cell = "Hello";

        Assert.Equal("Hello", cell.Text);
    }

    [Fact]
    public void Serialize_WritesTypeAndText()
    {
        var json = ToJson(new RawTextCell("Hello"));

        AssertProperties(json, "type", "text");
        Assert.Equal("raw_text", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_AsTableCell_WritesTypeOnce()
    {
        AssertProperties(ToJson(new List<ITableCell> { new RawTextCell("Hello") })[0], "type", "text");
    }

    [Fact]
    public void Text_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new RawTextCell(null!));
        Assert.Throws<ArgumentException>(() => new RawTextCell(""));
    }
}
