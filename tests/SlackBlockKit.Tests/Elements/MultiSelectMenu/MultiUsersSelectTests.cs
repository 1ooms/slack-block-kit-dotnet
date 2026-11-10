namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiUsersSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();

        var select = new MultiUsersSelect("action", ["U1", "U2"], confirm, 3, true, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal(["U1", "U2"], select.InitialUsers);
        Assert.Same(confirm, select.Confirm);
        Assert.Equal(3, select.MaxSelectedItems);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("multi_users_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MultiUsersSelect("action", ["U1"], Confirm(), 3, true, "Pick"));

        AssertProperties(json, "type", "action_id", "initial_users", "confirm", "max_selected_items",
            "focus_on_load", "placeholder");
        Assert.Equal("multi_users_select", json.GetProperty("type").GetString());
        Assert.Equal("U1", json.GetProperty("initial_users")[0].GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new MultiUsersSelect()), "type");
    }
}
