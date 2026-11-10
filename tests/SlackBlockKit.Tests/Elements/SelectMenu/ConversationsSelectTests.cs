namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class ConversationsSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var filter = new ConversationFilterObject([ConversationType.Public]);

        var select = new ConversationsSelect("action", "C123", true, confirm, false, filter, true, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal("C123", select.InitialConversation);
        Assert.True(select.DefaultToCurrentConversation);
        Assert.Same(confirm, select.Confirm);
        Assert.False(select.ResponseUrlEnabled);
        Assert.Same(filter, select.Filter);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("conversations_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ConversationsSelect("action", "C123", true, Confirm(), true,
            new ConversationFilterObject([ConversationType.Im]), true, "Pick"));

        AssertProperties(json, "type", "action_id", "initial_conversation", "default_to_current_conversation",
            "confirm", "response_url_enabled", "filter", "focus_on_load", "placeholder");
        Assert.Equal("conversations_select", json.GetProperty("type").GetString());
        Assert.Equal("im", json.GetProperty("filter").GetProperty("include")[0].GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ConversationsSelect()), "type");
    }
}
