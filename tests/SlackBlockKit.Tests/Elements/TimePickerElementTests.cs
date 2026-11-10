namespace SlackBlockKit.Tests.Elements;

public class TimePickerElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var initial = new DateTime(2024, 1, 1, 9, 30, 0);

        var element = new TimePickerElement("action", initial, confirm, true, "Pick", TimeZoneInfo.Utc);

        Assert.Equal("action", element.ActionId);
        Assert.Equal(initial, element.InitialTime);
        Assert.Same(confirm, element.Confirm);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Pick", element.Placeholder!.Text);
        Assert.Same(TimeZoneInfo.Utc, element.Timezone);
        Assert.Equal("timepicker", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TimePickerElement("action", new DateTime(2024, 1, 1, 21, 5, 59), Confirm(), true,
            "Pick", TimeZoneInfo.Utc));

        AssertProperties(json, "type", "action_id", "initial_time", "confirm", "focus_on_load", "placeholder",
            "timezone");
        Assert.Equal("timepicker", json.GetProperty("type").GetString());
        Assert.Equal("21:05", json.GetProperty("initial_time").GetString());
        Assert.Equal("UTC", json.GetProperty("timezone").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new TimePickerElement()), "type");
    }

    [Fact]
    public void Serialize_UsesTwentyFourHourClock()
    {
        var json = ToJson(new TimePickerElement(initialTime: new DateTime(2024, 1, 1, 0, 7, 0)));

        Assert.Equal("00:07", json.GetProperty("initial_time").GetString());
    }

    [Fact]
    public void TimezoneString_ReturnsIanaId()
    {
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

        Assert.Equal("America/New_York", new TimePickerElement(timezone: timezone).TimezoneString);
    }

    [Fact]
    public void TimezoneString_ConvertsWindowsIdToIanaId()
    {
        Assert.True(TimeZoneInfo.TryConvertIanaIdToWindowsId("Europe/Amsterdam", out var windowsId));
        var timezone = TimeZoneInfo.FindSystemTimeZoneById(windowsId!);

        var timezoneString = new TimePickerElement(timezone: timezone).TimezoneString;

        Assert.True(TimeZoneInfo.TryConvertIanaIdToWindowsId(timezoneString!, out var roundTripped));
        Assert.Equal(windowsId, roundTripped);
        Assert.Contains("/", timezoneString);
    }

    [Fact]
    public void TimezoneString_FallsBackToIdForCustomZones()
    {
        var custom = TimeZoneInfo.CreateCustomTimeZone("Custom/Zone", TimeSpan.FromHours(2), "Custom", "Custom");

        Assert.Equal("Custom/Zone", new TimePickerElement(timezone: custom).TimezoneString);
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new TimePickerElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new TimePickerElement(placeholder: Chars(151)));
    }
}
