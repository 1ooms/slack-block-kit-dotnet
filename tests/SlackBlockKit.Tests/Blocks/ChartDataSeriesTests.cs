namespace SlackBlockKit.Tests.Blocks;

public class ChartDataSeriesTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var data = new List<ChartDataPoint> { new("Q1", 1) };

        var series = new ChartDataSeries("2024", data);

        Assert.Equal("2024", series.Name);
        Assert.Same(data, series.Data);
    }

    [Fact]
    public void Serialize_WritesNameAndData()
    {
        var json = ToJson(new ChartDataSeries("2024", [new ChartDataPoint("Q1", 1)]));

        AssertProperties(json, "name", "data");
        Assert.Equal("Q1", json.GetProperty("data")[0].GetProperty("label").GetString());
    }

    [Fact]
    public void Name_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ChartDataSeries(null!, [new ChartDataPoint("Q1", 1)]));
    }

    [Fact]
    public void Data_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ChartDataSeries("S", null!));
        Assert.Throws<ArgumentException>(() => new ChartDataSeries("S", []));
    }

    [Fact]
    public void Data_AllowsUpTo20Points()
    {
        var data = Enumerable.Range(0, 20).Select(i => new ChartDataPoint($"P{i}", i)).ToList();

        Assert.Equal(20, new ChartDataSeries("S", data).Data.Count);

        data.Add(new ChartDataPoint("extra", 0));
        Assert.Throws<ArgumentException>(() => new ChartDataSeries("S", data));
    }
}
