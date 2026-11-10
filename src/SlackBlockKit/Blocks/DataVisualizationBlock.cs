using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a data visualization block (<c>type: "data_visualization"</c>) that displays charts such as pie, bar, area, or line charts.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block">Slack Data Visualization Block Documentation</seealso>
public class DataVisualizationBlock : Block, IMessageBlock, IHomeBlock
{
    private const int MaxTitleLength = 50;

    public DataVisualizationBlock(string title, Chart chart, string? blockId = null)
    {
        Title = title;
        Chart = chart;
        BlockId = blockId;
    }

    public override string Type => "data_visualization";

    [JsonPropertyName("title")]
    public string Title
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsLength(value, MaxTitleLength);
    }

    [JsonPropertyName("chart")]
    public Chart Chart
    {
        get;
        set => field = ThrowIfNull(value);
    }
}

/// <summary>
/// Defines a chart used within a data visualization block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block">Slack Data Visualization Block Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PieChart), "pie")]
[JsonDerivedType(typeof(BarChart), "bar")]
[JsonDerivedType(typeof(AreaChart), "area")]
[JsonDerivedType(typeof(LineChart), "line")]
public abstract class Chart
{
    [JsonIgnore] public abstract string Type { get; }
}

/// <summary>
/// Represents a pie chart in a data visualization block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#pie">Slack Data Visualization Block Documentation</seealso>
public class PieChart : Chart
{
    public PieChart(List<ChartSegment> segments)
    {
        Segments = segments;
    }

    [JsonPropertyName("segments")] public List<ChartSegment> Segments { get; set; } = [];

    public override string Type => "pie";
}

/// <summary>
/// Base class for charts that plot one or more data series against categories (bar, area and line charts).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block">Slack Data Visualization Block Documentation</seealso>
public abstract class SeriesChart : Chart
{
    protected SeriesChart(List<ChartDataSeries> series, ChartAxisConfig axisConfig)
    {
        Series = series;
        AxisConfig = axisConfig;
    }

    [JsonPropertyName("series")]
    public List<ChartDataSeries> Series
    {
        get;
        set
        {
            ThrowIfNull(value);
            ValidateCategories(value, AxisConfig);
            field = value;
        }
    }

    [JsonPropertyName("axis_config")]
    public ChartAxisConfig AxisConfig
    {
        get;
        set
        {
            ThrowIfNull(value);
            ValidateCategories(Series, value);
            field = value;
        }
    }

    private static void ValidateCategories(List<ChartDataSeries>? series, ChartAxisConfig? axisConfig)
    {
        // Skipped while the constructor is still assigning the first of the two properties.
        if (series == null || axisConfig == null) return;

        var labels = series.SelectMany(s => s.Data).Select(p => p.Label).ToHashSet();

        foreach (var category in axisConfig.Categories)
            if (!labels.Contains(category))
                throw new ArgumentException(
                    $"Category '{category}' in AxisConfig.Categories does not match any label in Series.Data.",
                    nameof(AxisConfig));
    }
}

/// <summary>
/// Represents a bar chart in a data visualization block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#bar">Slack Data Visualization Block Documentation</seealso>
public class BarChart : SeriesChart
{
    public BarChart(List<ChartDataSeries> series, ChartAxisConfig axisConfig) : base(series, axisConfig)
    {
    }

    public override string Type => "bar";
}

/// <summary>
/// Represents an area chart in a data visualization block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#area">Slack Data Visualization Block Documentation</seealso>
public class AreaChart : SeriesChart
{
    public AreaChart(List<ChartDataSeries> series, ChartAxisConfig axisConfig) : base(series, axisConfig)
    {
    }

    public override string Type => "area";
}

/// <summary>
/// Represents a line chart in a data visualization block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#line">Slack Data Visualization Block Documentation</seealso>
public class LineChart : SeriesChart
{
    public LineChart(List<ChartDataSeries> series, ChartAxisConfig axisConfig) : base(series, axisConfig)
    {
    }

    public override string Type => "line";
}

/// <summary>
/// Represents a data segment in a pie chart.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#segment">Slack Data Visualization Block Documentation</seealso>
public class ChartSegment
{
    public ChartSegment(string label, double value)
    {
        Label = label;
        Value = value;
    }

    [JsonPropertyName("label")] public string Label { get; set; }

    [JsonPropertyName("value")] public double Value { get; set; }
}

/// <summary>
/// Represents a named series of numerical data points in a chart.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#data-series">Slack Data Visualization Block Documentation</seealso>
public class ChartDataSeries
{
    private const int MaxDataPoints = 20;

    public ChartDataSeries(string name, List<ChartDataPoint> data)
    {
        Name = name;
        Data = data;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("data")]
    public List<ChartDataPoint> Data
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxDataPoints);
    }
}

public class ChartDataPoint
{
    private const int MaxLabelLength = 20;

    public ChartDataPoint(string label, double value)
    {
        Label = label;
        Value = value;
    }

    [JsonPropertyName("label")]
    public string Label
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsLength(value, MaxLabelLength);
    }

    [JsonPropertyName("value")]
    public double Value
    {
        get;
        set => field = ThrowIfNull(value);
    }
}

/// <summary>
/// Configures categories and axis titles for a chart.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-visualization-block#axis-config">Slack Data Visualization Block Documentation</seealso>
public class ChartAxisConfig
{
    private const int MaxLabelLength = 50;

    public ChartAxisConfig(List<string> categories, string? xTitle = null, string? yTitle = null)
    {
        Categories = categories;
        XTitle = xTitle;
        YTitle = yTitle;
    }

    [JsonPropertyName("categories")]
    public List<string> Categories
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("x_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? XTitle
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxLabelLength);
    }

    [JsonPropertyName("y_title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? YTitle
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxLabelLength);
    }
}