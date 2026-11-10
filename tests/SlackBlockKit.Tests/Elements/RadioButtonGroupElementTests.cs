namespace SlackBlockKit.Tests.Elements;

public class RadioButtonGroupElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };
        var confirm = Confirm();

        var element = new RadioButtonGroupElement(options, "action", options[1], confirm, true);

        Assert.Same(options, element.Options);
        Assert.Equal("action", element.ActionId);
        Assert.Same(options[1], element.InitialOption);
        Assert.Same(confirm, element.Confirm);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("radio_buttons", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };

        var json = ToJson(new RadioButtonGroupElement(options, "action", options[0], Confirm(), false));

        AssertProperties(json, "type", "options", "action_id", "initial_option", "confirm", "focus_on_load");
        Assert.Equal("radio_buttons", json.GetProperty("type").GetString());
        Assert.Equal("a", json.GetProperty("initial_option").GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new RadioButtonGroupElement([Option()])), "type", "options");
    }

    [Fact]
    public void Options_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new RadioButtonGroupElement(null!));
        Assert.Throws<ArgumentException>(() => new RadioButtonGroupElement([]));
    }

    [Fact]
    public void Options_AllowsUpTo10()
    {
        var options = Enumerable.Range(0, 10).Select(i => Option(i.ToString())).ToList();

        Assert.Equal(10, new RadioButtonGroupElement(options).Options.Count);

        options.Add(Option("extra"));
        Assert.Throws<ArgumentException>(() => new RadioButtonGroupElement(options));
    }

    [Fact]
    public void InitialOption_ThrowsWhenNotInOptions()
    {
        Assert.Throws<ArgumentException>(() => new RadioButtonGroupElement([Option("a")], initialOption: Option("b")));
    }

    [Fact]
    public void InitialOption_MatchesEquivalentOptionInstance()
    {
        var element = new RadioButtonGroupElement([Option("a")], initialOption: Option("a"));

        Assert.Equal("a", element.InitialOption!.Value);
    }

    [Fact]
    public void Options_SetterRevalidatesInitialOption()
    {
        var original = new List<OptionObject> { Option("a"), Option("b") };
        var element = new RadioButtonGroupElement(original, initialOption: original[0]);

        Assert.Throws<ArgumentException>(() => element.Options = [Option("c")]);
        Assert.Same(original, element.Options);

        element.Options = [Option("a"), Option("c")];
        Assert.Equal(2, element.Options.Count);

        element.InitialOption = null;
        element.Options = [Option("d")];
        Assert.Single(element.Options);
    }

    [Fact]
    public void InitialOption_SurvivesSerializationRoundTrip()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };
        var json = SlackSerializer.Serialize(new RadioButtonGroupElement(options, initialOption: options[1]));

        var element = SlackSerializer.Deserialize<RadioButtonGroupElement>(json);

        Assert.Equal("b", element!.InitialOption!.Value);
    }
}
