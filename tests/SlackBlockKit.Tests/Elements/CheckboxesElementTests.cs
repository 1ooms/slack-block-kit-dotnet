namespace SlackBlockKit.Tests.Elements;

public class CheckboxesElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };
        var confirm = Confirm();

        var element = new CheckboxesElement(options, "action", true, [options[0]], confirm, ["U1"]);

        Assert.Same(options, element.Options);
        Assert.Equal("action", element.ActionId);
        Assert.True(element.FocusOnLoad);
        Assert.Single(element.InitialOptions!);
        Assert.Same(confirm, element.Confirm);
        Assert.Equal(["U1"], element.VisibleToUserIds);
        Assert.Equal("checkboxes", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };

        var json = ToJson(new CheckboxesElement(options, "action", true, [options[1]], Confirm(), ["U1"]));

        AssertProperties(json, "type", "options", "action_id", "focus_on_load", "initial_options", "confirm",
            "visible_to_user_ids");
        Assert.Equal("checkboxes", json.GetProperty("type").GetString());
        Assert.Equal(2, json.GetProperty("options").GetArrayLength());
        Assert.Equal("b", json.GetProperty("initial_options")[0].GetProperty("value").GetString());
        Assert.Equal("U1", json.GetProperty("visible_to_user_ids")[0].GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new CheckboxesElement([Option()])), "type", "options");
    }

    [Fact]
    public void Options_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CheckboxesElement(null!));
        Assert.Throws<ArgumentException>(() => new CheckboxesElement([]));
    }

    [Fact]
    public void Options_AllowsUpTo10()
    {
        var options = Enumerable.Range(0, 10).Select(i => Option(i.ToString())).ToList();

        Assert.Equal(10, new CheckboxesElement(options).Options.Count);

        options.Add(Option("extra"));
        Assert.Throws<ArgumentException>(() => new CheckboxesElement(options));
    }

    [Fact]
    public void InitialOptions_ThrowsWhenNotInOptions()
    {
        Assert.Throws<ArgumentException>(() => new CheckboxesElement([Option("a")], initialOptions: [Option("b")]));
    }

    [Fact]
    public void InitialOptions_AllowsUpTo10()
    {
        var initialOptions = Enumerable.Range(0, 10).Select(_ => Option("a")).ToList();

        Assert.Equal(10, new CheckboxesElement([Option("a")], initialOptions: initialOptions).InitialOptions!.Count);

        initialOptions.Add(Option("a"));
        Assert.Throws<ArgumentException>(() => new CheckboxesElement([Option("a")], initialOptions: initialOptions));
    }

    [Fact]
    public void InitialOptions_MatchesEquivalentOptionInstances()
    {
        var element = new CheckboxesElement([Option("a")], initialOptions: [Option("a")]);

        Assert.Single(element.InitialOptions!);
    }

    [Fact]
    public void Options_SetterRevalidatesInitialOptions()
    {
        var original = new List<OptionObject> { Option("a"), Option("b") };
        var element = new CheckboxesElement(original, initialOptions: [original[0]]);

        Assert.Throws<ArgumentException>(() => element.Options = [Option("c")]);
        Assert.Same(original, element.Options);

        element.Options = [Option("a"), Option("c")];
        Assert.Equal(2, element.Options.Count);

        element.InitialOptions = null;
        element.Options = [Option("d")];
        Assert.Single(element.Options);
    }
}
