namespace SlackBlockKit.Tests.CompositionObjects;

public class OptionGroupObjectTests
{
    [Fact]
    public void StringConstructor_WrapsLabel()
    {
        var group = new OptionGroupObject("Group", [Option("a"), Option("b")]);

        Assert.Equal("Group", group.Label.Text);
        Assert.Equal(2, group.Options.Count);
    }

    [Fact]
    public void Serialize_WritesLabelAndOptions()
    {
        var json = ToJson(new OptionGroupObject(new PlainTextObject("Group"), [Option("a")]));

        AssertProperties(json, "label", "options");
        Assert.Equal("plain_text", json.GetProperty("label").GetProperty("type").GetString());
        Assert.Equal("Group", TextOf(json, "label"));
        Assert.Equal("a", json.GetProperty("options")[0].GetProperty("value").GetString());
    }

    [Fact]
    public void Label_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new OptionGroupObject((PlainTextObject)null!, [Option()]));
        Assert.NotNull(new OptionGroupObject(Chars(75), [Option()]));
        Assert.Throws<ArgumentException>(() => new OptionGroupObject(Chars(76), [Option()]));
    }

    [Fact]
    public void Options_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new OptionGroupObject("Group", null!));
        Assert.Throws<ArgumentException>(() => new OptionGroupObject("Group", []));
    }

    [Fact]
    public void Options_AllowsUpTo100()
    {
        var options = Enumerable.Range(0, 100).Select(i => Option(i.ToString())).ToList();

        Assert.Equal(100, new OptionGroupObject("Group", options).Options.Count);

        options.Add(Option("extra"));
        Assert.Throws<ArgumentException>(() => new OptionGroupObject("Group", options));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var group = RoundTrip(new OptionGroupObject("Label", [Option("a"), Option("b")]));

        Assert.Equal("Label", group.Label.Text);
        Assert.Equal(["a", "b"], group.Options.Select(option => option.Value));
    }
}
