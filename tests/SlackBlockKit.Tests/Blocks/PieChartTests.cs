namespace SlackBlockKit.Tests.Blocks;

public class PieChartTests
{
    [Fact]
    public void Constructor_SetsSegments()
    {
        var segments = new List<ChartSegment> { new("A", 1), new("B", 2) };

        var chart = new PieChart(segments);

        Assert.Same(segments, chart.Segments);
        Assert.Equal("pie", chart.Type);
    }

    [Fact]
    public void Serialize_AsChart_WritesTypeAndSegments()
    {
        var json = ToJson(new List<Chart> { new PieChart([new ChartSegment("A", 1.5)]) })[0];

        AssertProperties(json, "type", "segments");
        Assert.Equal("pie", json.GetProperty("type").GetString());
        Assert.Equal("A", json.GetProperty("segments")[0].GetProperty("label").GetString());
        Assert.Equal(1.5, json.GetProperty("segments")[0].GetProperty("value").GetDouble());
    }
}
