namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiConversationsSelectTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var filter = new ConversationFilterObject(excludeBotUsers: true);

        var select = new MultiConversationsSelect("action", ["C1"], true, confirm, 4, filter, true, "Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal(["C1"], select.InitialConversations);
        Assert.True(select.DefaultToCurrentConversation);
        Assert.Same(confirm, select.Confirm);
        Assert.Equal(4, select.MaxSelectedItems);
        Assert.Same(filter, select.Filter);
        Assert.True(select.FocusOnLoad);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal("multi_conversations_select", select.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MultiConversationsSelect("action", ["C1"], true, Confirm(), 4,
            new ConversationFilterObject(excludeBotUsers: true), true, "Pick"));

        AssertProperties(json, "type", "action_id", "initial_conversations", "default_to_current_conversation",
            "confirm", "max_selected_items", "filter", "focus_on_load", "placeholder");
        Assert.Equal("multi_conversations_select", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new MultiConversationsSelect()), "type");
    }
}
