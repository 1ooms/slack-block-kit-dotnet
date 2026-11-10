namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class UsersSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();

        var select = new UsersSelect("action", "U123", confirm, true, "Pick a user");

        Assert.Equal("action", select.ActionId);
        Assert.Equal("U123", select.InitialUser);
        Assert.Same(confirm, select.Confirm);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick a user", select.Placeholder!.Text);
        Assert.Equal("users_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new UsersSelect("action", "U123", Confirm(), true, "Pick a user"));

        AssertProperties(json, "type", "action_id", "initial_user", "confirm", "focus_on_load", "placeholder");
        Assert.Equal("users_select", json.GetProperty("type").GetString());
        Assert.Equal("U123", json.GetProperty("initial_user").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new UsersSelect()), "type");
    }
}
