namespace SlackBlockKit.Tests.Blocks;

public class ChartDataPointTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var point = new ChartDataPoint("Q1", 12.5);

        Assert.Equal("Q1", point.Label);
        Assert.Equal(12.5, point.Value);
    }

    [Fact]
    public void Serialize_WritesLabelAndValue()
    {
        var json = ToJson(new ChartDataPoint("Q1", 12.5));

        AssertProperties(json, "label", "value");
        Assert.Equal(12.5, json.GetProperty("value").GetDouble());
    }

    [Fact]
    public void Label_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new ChartDataPoint(null!, 1));
        Assert.Throws<ArgumentException>(() => new ChartDataPoint("", 1));
        Assert.NotNull(new ChartDataPoint(Chars(20), 1));
        Assert.Throws<ArgumentException>(() => new ChartDataPoint(Chars(21), 1));
    }
}
