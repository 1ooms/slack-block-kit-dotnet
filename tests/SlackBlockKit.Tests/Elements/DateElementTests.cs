namespace SlackBlockKit.Tests.Elements;

public class DateElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new DateElement(1700000000, DateFormat.DateLong, "Europe/Amsterdam", "https://example.com",
            "Fallback", style);

        Assert.Equal(1700000000, element.Timestamp);
        Assert.Equal("{date_long}", element.Format);
        Assert.Equal("Europe/Amsterdam", element.Timezone);
        Assert.Equal("https://example.com", element.Url);
        Assert.Equal("Fallback", element.Fallback);
        Assert.Same(style, element.Style);
        Assert.Equal("date", element.Type);
    }

    [Fact]
    public void DateTimeOffsetConstructor_ConvertsToUnixSeconds()
    {
        var element = new DateElement(new DateTimeOffset(2024, 1, 2, 13, 5, 0, TimeSpan.FromHours(1)), DateFormat.Ago);

        Assert.Equal(1704197100, element.Timestamp);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new DateElement(1700000000, "{date} at {time}", "UTC", "https://example.com", "Fallback",
            new RichTextStyle(italic: true)));

        AssertProperties(json, "type", "timestamp", "format", "timezone", "url", "fallback", "style");
        Assert.Equal("date", json.GetProperty("type").GetString());
        Assert.Equal(1700000000, json.GetProperty("timestamp").GetInt64());
        Assert.Equal("{date} at {time}", json.GetProperty("format").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new DateElement(0, DateFormat.Date)), "type", "timestamp", "format");
    }

    [Fact]
    public void Format_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new DateElement(0, null!));
        Assert.Throws<ArgumentException>(() => new DateElement(0, ""));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new DateElement(1700000000, DateFormat.Time, fallback: "f"));

        Assert.Equal(1700000000, element.Timestamp);
        Assert.Equal(DateFormat.Time, element.Format);
        Assert.Equal("f", element.Fallback);
    }
}
