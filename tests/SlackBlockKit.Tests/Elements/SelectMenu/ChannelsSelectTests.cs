namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class ChannelsSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();

        var select = new ChannelsSelect("action", "C123", confirm, true, false, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal("C123", select.InitialChannel);
        Assert.Same(confirm, select.Confirm);
        Assert.True(select.ResponseUrlEnabled);
        Assert.False(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("channels_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ChannelsSelect("action", "C123", Confirm(), true, true, "Pick"));

        AssertProperties(json, "type", "action_id", "initial_channel", "confirm", "response_url_enabled",
            "focus_on_load", "placeholder");
        Assert.Equal("channels_select", json.GetProperty("type").GetString());
        Assert.Equal("C123", json.GetProperty("initial_channel").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ChannelsSelect()), "type");
    }
}
