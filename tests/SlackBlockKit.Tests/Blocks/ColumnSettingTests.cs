namespace SlackBlockKit.Tests.Blocks;

public class ColumnSettingTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var setting = new TableBlock.ColumnSetting(ColumnAlignment.Right, true);

        Assert.Equal(ColumnAlignment.Right, setting.Align);
        Assert.True(setting.IsWrapped);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TableBlock.ColumnSetting(ColumnAlignment.Right, false));

        AssertProperties(json, "align", "is_wrapped");
        Assert.Equal("right", json.GetProperty("align").GetString());
        Assert.False(json.GetProperty("is_wrapped").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        Assert.Equal("{}", SlackSerializer.Serialize(new TableBlock.ColumnSetting()));
    }

    [Theory]
    [InlineData(ColumnAlignment.Left, "left")]
    [InlineData(ColumnAlignment.Center, "center")]
    [InlineData(ColumnAlignment.Right, "right")]
    public void Align_SerializesAsSnakeCase(ColumnAlignment align, string expected)
    {
        Assert.Equal(expected, ToJson(new TableBlock.ColumnSetting(align)).GetProperty("align").GetString());
    }
}
