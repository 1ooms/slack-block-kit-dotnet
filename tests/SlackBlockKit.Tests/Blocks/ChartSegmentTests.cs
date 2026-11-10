namespace SlackBlockKit.Tests.Blocks;

public class ChartSegmentTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var segment = new ChartSegment("Apples", 42.5);

        Assert.Equal("Apples", segment.Label);
        Assert.Equal(42.5, segment.Value);
    }

    [Fact]
    public void Serialize_WritesLabelAndValue()
    {
        var json = ToJson(new ChartSegment("Apples", 42.5));

        AssertProperties(json, "label", "value");
        Assert.Equal(42.5, json.GetProperty("value").GetDouble());
    }
}
