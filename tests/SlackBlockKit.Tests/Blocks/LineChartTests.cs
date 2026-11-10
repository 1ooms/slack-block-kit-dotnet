namespace SlackBlockKit.Tests.Blocks;

public class LineChartTests
{
    private static List<ChartDataSeries> Series() => [new ChartDataSeries("S", [new ChartDataPoint("Mon", 7)])];

    [Fact]
    public void Constructor_SetsProperties()
    {
        var chart = new LineChart(Series(), new ChartAxisConfig(["Mon"]));

        Assert.Single(chart.Series);
        Assert.Equal("line", chart.Type);
    }

    [Fact]
    public void Serialize_AsChart_WritesLineType()
    {
        var json = ToJson(new List<Chart> { new LineChart(Series(), new ChartAxisConfig(["Mon"])) })[0];

        AssertProperties(json, "type", "series", "axis_config");
        Assert.Equal("line", json.GetProperty("type").GetString());
    }
}
