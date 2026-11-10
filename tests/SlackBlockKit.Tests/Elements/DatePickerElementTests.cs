namespace SlackBlockKit.Tests.Elements;

public class DatePickerElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var element = new DatePickerElement("action", new DateTime(2024, 3, 9), confirm, true, "Pick");

        Assert.Equal("action", element.ActionId);
        Assert.Equal(new DateTime(2024, 3, 9), element.InitialDate);
        Assert.Same(confirm, element.Confirm);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Pick", element.Placeholder!.Text);
        Assert.Equal("datepicker", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new DatePickerElement("action", new DateTime(2024, 3, 9, 15, 30, 0), Confirm(), true,
            "Pick"));

        AssertProperties(json, "type", "action_id", "initial_date", "confirm", "focus_on_load", "placeholder");
        Assert.Equal("datepicker", json.GetProperty("type").GetString());
        Assert.Equal("2024-03-09", json.GetProperty("initial_date").GetString());
        Assert.Equal("Pick", TextOf(json, "placeholder"));
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new DatePickerElement()), "type");
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new DatePickerElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new DatePickerElement(placeholder: Chars(151)));
    }
}
