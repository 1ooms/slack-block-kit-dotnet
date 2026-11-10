namespace SlackBlockKit.Tests.Blocks;

public class ChartAxisConfigTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var axis = new ChartAxisConfig(["Q1", "Q2"], "Quarter", "Revenue");

        Assert.Equal(["Q1", "Q2"], axis.Categories);
        Assert.Equal("Quarter", axis.XTitle);
        Assert.Equal("Revenue", axis.YTitle);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ChartAxisConfig(["Q1"], "Quarter", "Revenue"));

        AssertProperties(json, "categories", "x_title", "y_title");
    }

    [Fact]
    public void Serialize_OmitsNullTitles()
    {
        AssertProperties(ToJson(new ChartAxisConfig(["Q1"])), "categories");
    }

    [Fact]
    public void Categories_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ChartAxisConfig(null!));
        Assert.Throws<ArgumentException>(() => new ChartAxisConfig([]));
    }

    [Fact]
    public void Categories_ThrowsOnDuplicates()
    {
        Assert.Throws<ArgumentException>(() => new ChartAxisConfig(["Q1", "Q2", "Q1"]));

        var axis = new ChartAxisConfig(["Q1"]);
        Assert.Throws<ArgumentException>(() => axis.Categories = ["Q2", "Q2"]);
        Assert.Equal(["Q1"], axis.Categories);
    }

    [Fact]
    public void Categories_IsCaseSensitive()
    {
        Assert.Equal(["q1", "Q1"], new ChartAxisConfig(["q1", "Q1"]).Categories);
    }

    [Fact]
    public void Titles_AllowUpTo50Characters()
    {
        Assert.NotNull(new ChartAxisConfig(["Q1"], Chars(50), Chars(50)));
        Assert.Throws<ArgumentException>(() => new ChartAxisConfig(["Q1"], Chars(51)));
        Assert.Throws<ArgumentException>(() => new ChartAxisConfig(["Q1"], yTitle: Chars(51)));
    }
}
