namespace SlackBlockKit.Tests.Elements;

public class OverflowMenuElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var options = new List<OptionObject> { Option("a"), Option("b") };
        var confirm = Confirm();

        var element = new OverflowMenuElement(options, "action", confirm);

        Assert.Same(options, element.Options);
        Assert.Equal("action", element.ActionId);
        Assert.Same(confirm, element.Confirm);
        Assert.Equal("overflow", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new OverflowMenuElement([Option("a")], "action", Confirm()));

        AssertProperties(json, "type", "options", "action_id", "confirm");
        Assert.Equal("overflow", json.GetProperty("type").GetString());
        Assert.Equal("a", json.GetProperty("options")[0].GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new OverflowMenuElement([Option()])), "type", "options");
    }

    [Fact]
    public void Options_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new OverflowMenuElement(null!));
        Assert.Throws<ArgumentException>(() => new OverflowMenuElement([]));
    }

    [Fact]
    public void Options_AllowsUpTo5()
    {
        var options = Enumerable.Range(0, 5).Select(i => Option(i.ToString())).ToList();

        Assert.Equal(5, new OverflowMenuElement(options).Options.Count);

        options.Add(Option("extra"));
        Assert.Throws<ArgumentException>(() => new OverflowMenuElement(options));
    }
}
