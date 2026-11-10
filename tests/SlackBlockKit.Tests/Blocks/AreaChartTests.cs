namespace SlackBlockKit.Tests.Blocks;

public class AreaChartTests
{
    private static List<ChartDataSeries> Series() => [new ChartDataSeries("S", [new ChartDataPoint("Jan", 3)])];

    [Fact]
    public void Constructor_SetsProperties()
    {
        var chart = new AreaChart(Series(), new ChartAxisConfig(["Jan"]));

        Assert.Single(chart.Series);
        Assert.Equal("area", chart.Type);
    }

    [Fact]
    public void Serialize_AsChart_WritesAreaType()
    {
        var json = ToJson(new List<Chart> { new AreaChart(Series(), new ChartAxisConfig(["Jan"])) })[0];

        AssertProperties(json, "type", "series", "axis_config");
        Assert.Equal("area", json.GetProperty("type").GetString());
    }
}
