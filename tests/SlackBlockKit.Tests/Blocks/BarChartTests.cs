namespace SlackBlockKit.Tests.Blocks;

public class BarChartTests
{
    private static List<ChartDataSeries> Series() =>
        [new ChartDataSeries("2024", [new ChartDataPoint("Q1", 1), new ChartDataPoint("Q2", 2)])];

    [Fact]
    public void Constructor_SetsProperties()
    {
        var series = Series();
        var axis = new ChartAxisConfig(["Q1", "Q2"]);

        var chart = new BarChart(series, axis);

        Assert.Same(series, chart.Series);
        Assert.Same(axis, chart.AxisConfig);
        Assert.Equal("bar", chart.Type);
    }

    [Fact]
    public void Serialize_AsChart_WritesAllFields()
    {
        var json = ToJson(new List<Chart> { new BarChart(Series(), new ChartAxisConfig(["Q1", "Q2"], "Quarter", "Sales")) })[0];

        AssertProperties(json, "type", "series", "axis_config");
        Assert.Equal("bar", json.GetProperty("type").GetString());
        Assert.Equal("2024", json.GetProperty("series")[0].GetProperty("name").GetString());
        Assert.Equal("Quarter", json.GetProperty("axis_config").GetProperty("x_title").GetString());
    }

    [Fact]
    public void Constructor_ThrowsWhenSeriesMissesACategory()
    {
        Assert.Throws<ArgumentException>(() => new BarChart(Series(), new ChartAxisConfig(["Q1", "Q2", "Q3"])));
    }

    [Fact]
    public void Constructor_ThrowsWhenDataPointHasNoCategory()
    {
        Assert.Throws<ArgumentException>(() => new BarChart(Series(), new ChartAxisConfig(["Q1"])));
    }

    [Fact]
    public void Constructor_ThrowsWhenSeriesHasDuplicateDataPoints()
    {
        var series = new List<ChartDataSeries>
        {
            new("A", [new ChartDataPoint("Q1", 1), new ChartDataPoint("Q1", 2)])
        };

        Assert.Throws<ArgumentException>(() => new BarChart(series, new ChartAxisConfig(["Q1"])));
    }

    [Fact]
    public void Constructor_ThrowsWhenCategoriesAreSpreadOverSeries()
    {
        var series = new List<ChartDataSeries>
        {
            new("A", [new ChartDataPoint("Q1", 1)]), new("B", [new ChartDataPoint("Q2", 2)])
        };

        Assert.Throws<ArgumentException>(() => new BarChart(series, new ChartAxisConfig(["Q1", "Q2"])));
    }

    [Fact]
    public void Constructor_AcceptsEverySeriesCoveringEveryCategory()
    {
        var series = new List<ChartDataSeries>
        {
            new("A", [new ChartDataPoint("Q1", 1), new ChartDataPoint("Q2", 2)]),
            new("B", [new ChartDataPoint("Q2", 3), new ChartDataPoint("Q1", 4)])
        };

        Assert.NotNull(new BarChart(series, new ChartAxisConfig(["Q1", "Q2"])));
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new BarChart(null!, new ChartAxisConfig(["Q1"])));
        Assert.Throws<ArgumentNullException>(() => new BarChart(Series(), null!));
    }

    [Fact]
    public void Setters_RevalidateCategories()
    {
        var chart = new BarChart(Series(), new ChartAxisConfig(["Q1", "Q2"]));

        Assert.Throws<ArgumentException>(() => chart.AxisConfig = new ChartAxisConfig(["Q1"]));
        Assert.Throws<ArgumentException>(() => chart.AxisConfig = new ChartAxisConfig(["Q1", "Q2", "Q3"]));
        Assert.Throws<ArgumentException>(() =>
            chart.Series = [new ChartDataSeries("Other", [new ChartDataPoint("Q1", 1)])]);

        chart.AxisConfig = new ChartAxisConfig(["Q2", "Q1"]);
        Assert.Equal(["Q2", "Q1"], chart.AxisConfig.Categories);

        chart.Series = [new ChartDataSeries("Other", [new ChartDataPoint("Q2", 5), new ChartDataPoint("Q1", 6)])];
        Assert.Equal("Other", chart.Series[0].Name);
    }
}
