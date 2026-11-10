namespace SlackBlockKit.Tests.Elements;

public class DateTimePickerElementTests
{
    private static readonly DateTime UtcDateTime = new(2024, 1, 2, 13, 5, 0, DateTimeKind.Utc);

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var element = new DateTimePickerElement("action", UtcDateTime, confirm, true);

        Assert.Equal("action", element.ActionId);
        Assert.Equal(UtcDateTime, element.InitialDateTime);
        Assert.Same(confirm, element.Confirm);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("datetimepicker", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new DateTimePickerElement("action", UtcDateTime, Confirm(), false));

        AssertProperties(json, "type", "action_id", "initial_date_time", "confirm", "focus_on_load");
        Assert.Equal("datetimepicker", json.GetProperty("type").GetString());
        Assert.Equal(1704200700, json.GetProperty("initial_date_time").GetInt64());
        Assert.False(json.GetProperty("focus_on_load").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new DateTimePickerElement()), "type");
    }
}
