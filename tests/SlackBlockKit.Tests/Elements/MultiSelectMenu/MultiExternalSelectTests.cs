namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiExternalSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var initial = new List<PlainTextOptionObject> { Option() };
        var confirm = Confirm();

        var select = new MultiExternalSelect("action", 2, initial, confirm, 5, "Search");

        Assert.Equal("action", select.ActionId);
        Assert.Equal(2, select.MinQueryLength);
        Assert.Same(initial, select.InitialOptions);
        Assert.Same(confirm, select.Confirm);
        Assert.Equal(5, select.MaxSelectedItems);
        Assert.Equal("Search", select.Placeholder!.Text);
        Assert.Equal("multi_external_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MultiExternalSelect("action", 2, [Option()], Confirm(), 5, "Search"));

        AssertProperties(json, "type", "action_id", "min_query_length", "initial_options", "confirm",
            "max_selected_items", "placeholder");
        Assert.Equal("multi_external_select", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new MultiExternalSelect()), "type");
    }

    [Fact]
    public void InitialOptions_CannotExceedMaxSelectedItems()
    {
        Assert.NotNull(new MultiExternalSelect(initialOptions: [Option("a"), Option("b")], maxSelectedItems: 2));
        Assert.Throws<ArgumentException>(() =>
            new MultiExternalSelect(initialOptions: [Option("a"), Option("b")], maxSelectedItems: 1));

        var select = new MultiExternalSelect(maxSelectedItems: 1);
        Assert.Throws<ArgumentException>(() => select.InitialOptions = [Option("a"), Option("b")]);
        Assert.Null(select.InitialOptions);
    }

    [Fact]
    public void MaxSelectedItems_CannotBeLoweredBelowInitialOptions()
    {
        var select = new MultiExternalSelect(initialOptions: [Option("a"), Option("b")]);

        Assert.Throws<ArgumentException>(() => select.MaxSelectedItems = 1);
        select.MaxSelectedItems = 3;
        Assert.Equal(3, select.MaxSelectedItems);
    }
}
