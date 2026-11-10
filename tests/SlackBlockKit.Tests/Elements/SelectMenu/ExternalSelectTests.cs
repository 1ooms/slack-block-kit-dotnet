namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class ExternalSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var initial = Option();
        var confirm = Confirm();

        var select = new ExternalSelect("action", initial, 3, confirm, true, "Search");

        Assert.Equal("action", select.ActionId);
        Assert.Same(initial, select.InitialOption);
        Assert.Equal(3, select.MinQueryLength);
        Assert.Same(confirm, select.Confirm);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Search", select.Placeholder!.Text);
        Assert.Equal("external_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ExternalSelect("action", Option("a"), 3, Confirm(), true, "Search"));

        AssertProperties(json, "type", "action_id", "initial_option", "min_query_length", "confirm",
            "focus_on_load", "placeholder");
        Assert.Equal("external_select", json.GetProperty("type").GetString());
        Assert.Equal(3, json.GetProperty("min_query_length").GetInt32());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ExternalSelect()), "type");
    }
}
