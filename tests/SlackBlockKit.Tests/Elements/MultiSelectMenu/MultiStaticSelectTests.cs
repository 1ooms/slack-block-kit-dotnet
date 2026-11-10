namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiStaticSelectTests
{
    [Fact]
    public void Constructor_WithOptions_SetsAllProperties()
    {
        var options = new List<PlainTextOptionObject> { Option("a"), Option("b") };
        var confirm = Confirm();

        var select = new MultiStaticSelect("action", options, null, [options[0]], confirm, 2, true, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Same(options, select.Options);
        Assert.Null(select.OptionGroups);
        Assert.Single(select.InitialOptions!);
        Assert.Same(confirm, select.Confirm);
        Assert.Equal(2, select.MaxSelectedItems);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("multi_static_select", select.Type);
    }

    [Fact]
    public void Serialize_WithOptions_WritesAllFields()
    {
        var options = new List<PlainTextOptionObject> { Option("a"), Option("b") };

        var json = ToJson(new MultiStaticSelect("action", options, null, [options[1]], Confirm(), 2, true, "Pick"));

        AssertProperties(json, "type", "action_id", "options", "initial_options", "confirm", "max_selected_items",
            "focus_on_load", "placeholder");
        Assert.Equal("multi_static_select", json.GetProperty("type").GetString());
        Assert.Equal("b", json.GetProperty("initial_options")[0].GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_WithOptionGroups_WritesGroups()
    {
        var json = ToJson(new MultiStaticSelect(optionGroups: [new OptionGroupObject("G", [Option("a")])]));

        AssertProperties(json, "type", "option_groups");
    }

    [Fact]
    public void Constructor_ThrowsWithoutOptionsOrGroups()
    {
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect());
    }

    [Fact]
    public void Constructor_ThrowsWithBothOptionsAndGroups()
    {
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: [Option()], optionGroups: [new OptionGroupObject("G", [Option()])]));
    }

    [Fact]
    public void Options_ThrowsWhenEmptyOrTooMany()
    {
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(options: []));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: Enumerable.Range(0, 101).Select(i => Option(i.ToString())).ToList()));
        Assert.NotNull(
            new MultiStaticSelect(options: Enumerable.Range(0, 100).Select(i => Option(i.ToString())).ToList()));
    }

    [Fact]
    public void OptionGroups_ThrowsWhenEmptyOrTooMany()
    {
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(optionGroups: []));
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(
            optionGroups: Enumerable.Range(0, 101).Select(i => new OptionGroupObject($"G{i}", [Option()])).ToList()));
    }

    [Fact]
    public void InitialOptions_AllowsUpTo100()
    {
        var initialOptions = Enumerable.Range(0, 100).Select(_ => Option("a")).ToList();

        Assert.Equal(100,
            new MultiStaticSelect(options: [Option("a")], initialOptions: initialOptions).InitialOptions!.Count);

        initialOptions.Add(Option("a"));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: [Option("a")], initialOptions: initialOptions));
    }

    [Fact]
    public void InitialOptions_MustMatchOptions()
    {
        Assert.NotNull(new MultiStaticSelect(options: [Option("a"), Option("b")], initialOptions: [Option("b")]));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: [Option("a")], initialOptions: [Option("a"), Option("c")]));
    }

    [Fact]
    public void InitialOptions_MustMatchGroupOptions()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.NotNull(new MultiStaticSelect(optionGroups: groups, initialOptions: [Option("a")]));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(optionGroups: groups, initialOptions: [Option("b")]));
    }

    [Fact]
    public void InitialOptions_InitializerValidates()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.NotNull(new MultiStaticSelect(options: [Option("a")]) { InitialOptions = [Option("a")] });
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: [Option("a")]) { InitialOptions = [Option("b")] });
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(optionGroups: groups) { InitialOptions = [Option("b")] });
    }

    [Fact]
    public void Options_InitializerRevalidatesInitialOptions()
    {
        Assert.NotNull(new MultiStaticSelect(options: [Option("a")], initialOptions: [Option("a")])
            { Options = [Option("a"), Option("b")] });
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: [Option("a")], initialOptions: [Option("a")]) { Options = [Option("b")] });
    }

    [Fact]
    public void OptionGroups_InitializerRevalidatesInitialOptions()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(optionGroups: groups, initialOptions: [Option("a")])
                { OptionGroups = [new OptionGroupObject("G", [Option("b")])] });
    }

    [Fact]
    public void Initializer_ThrowsWhenBothOrNeitherOptionsAndGroupsRemain()
    {
        var groups = new List<OptionGroupObject> { new("G", [Option("a")]) };

        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(options: [Option()]) { OptionGroups = groups });
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(optionGroups: groups) { Options = [Option()] });
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(options: [Option()]) { Options = null });
        Assert.Throws<ArgumentException>(() => new MultiStaticSelect(optionGroups: groups) { OptionGroups = null });
    }

    [Fact]
    public void Deserialize_RoundTripsOptionsAndGroupsWithInitialOptions()
    {
        var withOptions = SlackSerializer.Serialize(
            new MultiStaticSelect(options: [Option("a")], initialOptions: [Option("a")]));
        var withGroups = SlackSerializer.Serialize(new MultiStaticSelect(
            optionGroups: [new OptionGroupObject("G", [Option("a")])], initialOptions: [Option("a")]));

        Assert.Single(SlackSerializer.Deserialize<MultiStaticSelect>(withOptions)!.InitialOptions!);
        Assert.Single(SlackSerializer.Deserialize<MultiStaticSelect>(withGroups)!.InitialOptions!);
    }

    [Fact]
    public void InitialOptions_CannotExceedMaxSelectedItems()
    {
        var options = new List<PlainTextOptionObject> { Option("a"), Option("b") };

        Assert.NotNull(new MultiStaticSelect(options: options, initialOptions: [Option("a"), Option("b")],
            maxSelectedItems: 2));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: options, initialOptions: [Option("a"), Option("b")], maxSelectedItems: 1));
        Assert.Throws<ArgumentException>(() =>
            new MultiStaticSelect(options: options, maxSelectedItems: 1) { InitialOptions = [Option("a"), Option("b")] });
    }

    [Fact]
    public void MaxSelectedItems_CannotBeLoweredBelowInitialOptions()
    {
        var select = new MultiStaticSelect(options: [Option("a"), Option("b")], initialOptions: [Option("a"), Option("b")]);

        Assert.Throws<ArgumentException>(() => select.MaxSelectedItems = 1);
        Assert.Null(select.MaxSelectedItems);

        select.MaxSelectedItems = 2;
        Assert.Equal(2, select.MaxSelectedItems);
    }
}
