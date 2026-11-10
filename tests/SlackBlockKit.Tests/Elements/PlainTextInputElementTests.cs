namespace SlackBlockKit.Tests.Elements;

public class PlainTextInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var dispatch = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]);
        var element = new PlainTextInputElement("action", "initial", true, 1, 100, dispatch, true,
            new PlainTextObject("Type here"));

        Assert.Equal("action", element.ActionId);
        Assert.Equal("initial", element.InitialValue);
        Assert.True(element.Multiline);
        Assert.Equal(1, element.MinLength);
        Assert.Equal(100, element.MaxLength);
        Assert.Same(dispatch, element.DispatchActionConfig);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Type here", element.Placeholder!.Text);
        Assert.Equal("plain_text_input", element.Type);
    }

    [Fact]
    public void PlaceholderConstructor_WrapsPlaceholder()
    {
        var element = new PlainTextInputElement("action", "Type here");

        Assert.Equal("action", element.ActionId);
        Assert.Equal("Type here", element.Placeholder!.Text);
        Assert.Null(element.InitialValue);
    }

    [Fact]
    public void PlaceholderConstructor_AllowsNullPlaceholder()
    {
        Assert.Null(new PlainTextInputElement("action", (string?)null).Placeholder);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new PlainTextInputElement("action", "initial", true, 0, 3000,
            new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]), false,
            new PlainTextObject("Type here")));

        AssertProperties(json, "type", "action_id", "initial_value", "multiline", "min_length", "max_length",
            "dispatch_action_config", "focus_on_load", "placeholder");
        Assert.Equal("plain_text_input", json.GetProperty("type").GetString());
        Assert.Equal(0, json.GetProperty("min_length").GetInt32());
        Assert.Equal(3000, json.GetProperty("max_length").GetInt32());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new PlainTextInputElement()), "type");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3000)]
    public void MinLength_AcceptsValuesBetween0And3000(int value)
    {
        Assert.Equal(value, new PlainTextInputElement(minLength: value).MinLength);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3001)]
    public void MinLength_ThrowsWhenOutOfRange(int value)
    {
        Assert.Throws<ArgumentException>(() => new PlainTextInputElement(minLength: value));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3000)]
    public void MaxLength_AcceptsValuesBetween1And3000(int value)
    {
        Assert.Equal(value, new PlainTextInputElement(maxLength: value).MaxLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3001)]
    public void MaxLength_ThrowsWhenOutOfRange(int value)
    {
        Assert.Throws<ArgumentException>(() => new PlainTextInputElement(maxLength: value));
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new PlainTextInputElement("action", Chars(150)));
        Assert.Throws<ArgumentException>(() => new PlainTextInputElement("action", Chars(151)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new PlainTextInputElement("input", "Initial", true, 1, 10, focusOnLoad: true,
            placeholder: new PlainTextObject("Placeholder")));

        Assert.Equal("input", element.ActionId);
        Assert.Equal("Initial", element.InitialValue);
        Assert.True(element.Multiline);
        Assert.Equal(1, element.MinLength);
        Assert.Equal(10, element.MaxLength);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Placeholder", element.Placeholder!.Text);
    }

    // The Slack API accepts these combinations, so the library must not reject them.
    [Fact]
    public void Lengths_AreNotCrossValidated()
    {
        var minAboveMax = new PlainTextInputElement(minLength: 100, maxLength: 5);
        Assert.Equal(100, minAboveMax.MinLength);
        Assert.Equal(5, minAboveMax.MaxLength);

        var initialTooLong = new PlainTextInputElement(initialValue: "too long", maxLength: 3);
        Assert.Equal("too long", initialTooLong.InitialValue);

        var viaSetters = new PlainTextInputElement(maxLength: 3);
        viaSetters.MinLength = 10;
        viaSetters.InitialValue = "too long";
        Assert.Equal(10, viaSetters.MinLength);
    }
}
