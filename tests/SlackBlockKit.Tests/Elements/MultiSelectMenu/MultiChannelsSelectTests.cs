namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiChannelsSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();

        var select = new MultiChannelsSelect("action", ["C1", "C2"], confirm, 2, false, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal(["C1", "C2"], select.InitialChannels);
        Assert.Same(confirm, select.Confirm);
        Assert.Equal(2, select.MaxSelectedItems);
        Assert.False(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("multi_channels_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MultiChannelsSelect("action", ["C1"], Confirm(), 2, true, "Pick"));

        AssertProperties(json, "type", "action_id", "initial_channels", "confirm", "max_selected_items",
            "focus_on_load", "placeholder");
        Assert.Equal("multi_channels_select", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new MultiChannelsSelect()), "type");
    }

    [Fact]
    public void InitialChannels_CannotExceedMaxSelectedItems()
    {
        Assert.NotNull(new MultiChannelsSelect(initialChannels: ["C1", "C2"], maxSelectedItems: 2));
        Assert.Throws<ArgumentException>(() => new MultiChannelsSelect(initialChannels: ["C1", "C2"], maxSelectedItems: 1));

        var select = new MultiChannelsSelect(maxSelectedItems: 1);
        Assert.Throws<ArgumentException>(() => select.InitialChannels = ["C1", "C2"]);
        Assert.Null(select.InitialChannels);
    }

    [Fact]
    public void MaxSelectedItems_CannotBeLoweredBelowInitialChannels()
    {
        var select = new MultiChannelsSelect(initialChannels: ["C1", "C2"]);

        Assert.Throws<ArgumentException>(() => select.MaxSelectedItems = 1);
        select.MaxSelectedItems = 2;
        Assert.Equal(2, select.MaxSelectedItems);
    }
}
