namespace SlackBlockKit.Tests.Blocks;

public class ActionCellTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var button = new ButtonElement("Go");
        var fallback = new RawTextCell("Go");

        var cell = new ActionCell(button, fallback);

        Assert.Same(button, cell.Element);
        Assert.Same(fallback, cell.Fallback);
        Assert.Equal("action_cell", cell.Type);
    }

    [Fact]
    public void Serialize_AsCell_WritesTypeOnceAndNestedDiscriminators()
    {
        var json = ToJson(new List<IDataTableCell> { new ActionCell(new ButtonElement("Go"), new RawNumberCell(1, "1")) })[0];

        AssertProperties(json, "type", "element", "fallback");
        Assert.Equal("action_cell", json.GetProperty("type").GetString());
        Assert.Equal("button", json.GetProperty("element").GetProperty("type").GetString());
        Assert.Equal("raw_number", json.GetProperty("fallback").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_Directly_WritesType()
    {
        var json = ToJson(new ActionCell(new ButtonElement("Go")));

        AssertProperties(json, "type", "element");
        Assert.Equal("action_cell", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Element_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ActionCell(null!));
    }
}
