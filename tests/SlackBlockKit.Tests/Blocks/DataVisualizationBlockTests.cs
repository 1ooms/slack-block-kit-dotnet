namespace SlackBlockKit.Tests.Blocks;

public class DataVisualizationBlockTests
{
    private static PieChart Pie() => new([new ChartSegment("A", 1)]);

    [Fact]
    public void Constructor_SetsProperties()
    {
        var chart = Pie();

        var block = new DataVisualizationBlock("Sales", chart, "viz_1");

        Assert.Equal("Sales", block.Title);
        Assert.Same(chart, block.Chart);
        Assert.Equal("viz_1", block.BlockId);
        Assert.Equal("data_visualization", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new DataVisualizationBlock("Sales", Pie(), "viz_1"));

        AssertProperties(json, "type", "title", "chart", "block_id");
        Assert.Equal("data_visualization", json.GetProperty("type").GetString());
        Assert.Equal("pie", json.GetProperty("chart").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsBlockIdWhenNull()
    {
        AssertProperties(ToJson(new DataVisualizationBlock("Sales", Pie())), "type", "title", "chart");
    }

    [Fact]
    public void Title_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new DataVisualizationBlock(null!, Pie()));
        Assert.Throws<ArgumentException>(() => new DataVisualizationBlock("", Pie()));
        Assert.NotNull(new DataVisualizationBlock(Chars(50), Pie()));
        Assert.Throws<ArgumentException>(() => new DataVisualizationBlock(Chars(51), Pie()));
    }

    [Fact]
    public void Chart_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new DataVisualizationBlock("Sales", null!));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var chart = new BarChart([new ChartDataSeries("2024", [new ChartDataPoint("Q1", 1)])],
            new ChartAxisConfig(["Q1"], "Quarter"));

        var block = RoundTrip(new DataVisualizationBlock("Sales", chart, "viz_1"));

        var bar = Assert.IsType<BarChart>(block.Chart);
        Assert.Equal("Q1", bar.Series[0].Data[0].Label);
        Assert.Equal("Quarter", bar.AxisConfig.XTitle);
        Assert.Equal("Sales", block.Title);
        Assert.Equal("viz_1", block.BlockId);
    }
}
