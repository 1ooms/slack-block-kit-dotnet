namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class StaticSelectTests
{
    [Fact]
    public void Constructor_WithOptions_SetsAllProperties()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };
        var confirm = Confirm();

        var select = new StaticSelect("action", options, null, options[1], confirm, true, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Same(options, select.Options);
        Assert.Null(select.OptionGroups);
        Assert.Same(options[1], select.InitialOption);
        Assert.Same(confirm, select.Confirm);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("static_select", select.Type);
    }

    [Fact]
    public void Serialize_WithOptions_WritesAllFields()
    {
        var options = new List<OptionObject> { Option("a") };

        var json = ToJson(new StaticSelect("action", options, null, options[0], Confirm(), true, "Pick"));

        AssertProperties(json, "type", "action_id", "options", "initial_option", "confirm", "focus_on_load",
            "placeholder");
        Assert.Equal("static_select", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_WithOptionGroups_WritesGroups()
    {
        var json = ToJson(new StaticSelect(optionGroups: [new OptionGroupObject("Group", [Option("a")])]));

        AssertProperties(json, "type", "option_groups");
        Assert.Equal("Group", TextOf(json.GetProperty("option_groups")[0], "label"));
    }

    [Fact]
    public void Constructor_ThrowsWithoutOptionsOrGroups()
    {
        Assert.Throws<ArgumentException>(() => new StaticSelect());
    }

    [Fact]
    public void Constructor_ThrowsWithBothOptionsAndGroups()
    {
        Assert.Throws<ArgumentException>(() =>
            new StaticSelect(options: [Option()], optionGroups: [new OptionGroupObject("G", [Option()])]));
    }

    [Fact]
    public void Options_ThrowsWhenEmptyOrTooMany()
    {
        Assert.Throws<ArgumentException>(() => new StaticSelect(options: []));
        Assert.Throws<ArgumentException>(() =>
            new StaticSelect(options: Enumerable.Range(0, 101).Select(i => Option(i.ToString())).ToList()));
        Assert.NotNull(new StaticSelect(options: Enumerable.Range(0, 100).Select(i => Option(i.ToString())).ToList()));
    }

    [Fact]
    public void OptionGroups_ThrowsWhenEmptyOrTooMany()
    {
        Assert.Throws<ArgumentException>(() => new StaticSelect(optionGroups: []));
        Assert.Throws<ArgumentException>(() => new StaticSelect(
            optionGroups: Enumerable.Range(0, 101).Select(i => new OptionGroupObject($"G{i}", [Option()])).ToList()));
    }

    [Fact]
    public void InitialOption_MustMatchAnOption()
    {
        Assert.NotNull(new StaticSelect(options: [Option("a")], initialOption: Option("a")));
        Assert.Throws<ArgumentException>(() => new StaticSelect(options: [Option("a")], initialOption: Option("b")));
    }

    [Fact]
    public void InitialOption_MustMatchAGroupOption()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.NotNull(new StaticSelect(optionGroups: groups, initialOption: Option("a")));
        Assert.Throws<ArgumentException>(() => new StaticSelect(optionGroups: groups, initialOption: Option("b")));
    }

    [Fact]
    public void InitialOption_InitializerValidates()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.NotNull(new StaticSelect(options: [Option("a")]) { InitialOption = Option("a") });
        Assert.Throws<ArgumentException>(() => new StaticSelect(options: [Option("a")]) { InitialOption = Option("b") });
        Assert.Throws<ArgumentException>(() => new StaticSelect(optionGroups: groups) { InitialOption = Option("b") });
    }

    [Fact]
    public void Options_InitializerRevalidatesInitialOption()
    {
        Assert.NotNull(new StaticSelect(options: [Option("a")], initialOption: Option("a"))
            { Options = [Option("a"), Option("b")] });
        Assert.Throws<ArgumentException>(() =>
            new StaticSelect(options: [Option("a")], initialOption: Option("a")) { Options = [Option("b")] });
    }

    [Fact]
    public void OptionGroups_InitializerRevalidatesInitialOption()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.Throws<ArgumentException>(() =>
            new StaticSelect(optionGroups: groups, initialOption: Option("a"))
                { OptionGroups = [new OptionGroupObject("G", [Option("b")])] });
    }

    [Fact]
    public void Initializer_ThrowsWhenBothOrNeitherOptionsAndGroupsRemain()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.Throws<ArgumentException>(() => new StaticSelect(options: [Option()]) { OptionGroups = groups });
        Assert.Throws<ArgumentException>(() => new StaticSelect(optionGroups: groups) { Options = [Option()] });
        Assert.Throws<ArgumentException>(() => new StaticSelect(options: [Option()]) { Options = null });
        Assert.Throws<ArgumentException>(() => new StaticSelect(optionGroups: groups) { OptionGroups = null });
    }

    [Fact]
    public void Deserialize_RoundTripsOptionsAndGroupsWithInitialOption()
    {
        var withOptions = SlackSerializer.Serialize(new StaticSelect(options: [Option("a")], initialOption: Option("a")));
        var withGroups = SlackSerializer.Serialize(new StaticSelect(
            optionGroups: [new OptionGroupObject("G", [Option("a")])], initialOption: Option("a")));

        Assert.Equal("a", SlackSerializer.Deserialize<StaticSelect>(withOptions)!.InitialOption!.Value);
        Assert.Equal("a", SlackSerializer.Deserialize<StaticSelect>(withGroups)!.InitialOption!.Value);
    }
}
